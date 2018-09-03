using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Text;
using UnityEngine;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.Data
{
    public class PlayGamesScript : MonoBehaviour
    {
        [SerializeField] private GameObject Tutorial_Prefab;
        public static PlayGamesScript Instance { get; private set; }

        const string SAVE_NAME = "PlayData";
        bool isSaving;
        bool isCloudDataLoaded = false;

        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);

                GameObject.Instantiate(Tutorial_Prefab);
            }
            else if (Instance != this)
            {
                Destroy(this.gameObject);
            }

            //처음으로 접속했을 경우 데이터를 디폴트로 세팅한다
            if (!PlayerPrefs.HasKey(SAVE_NAME))
            {
                PlayerPrefs.SetString(SAVE_NAME, DataManager.Instance.PlayerDataToString(new PlayerData()));

                GameObject.Instantiate(Tutorial_Prefab);

                //첫 접속시 이름 등을 적용하는 UI를 띄운다.
                //TownScene.UIManager.Instance.OpenMenu<TownScene.NewPlayerUI>();
            }

            //게임을 설치, 실행했음을 알리는 키를 설정한다 - 0 = no, 1 = yes 
            if (!PlayerPrefs.HasKey("IsFirstTime"))
                PlayerPrefs.SetInt("IsFirstTime", 1);

            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                .EnableSavedGames().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();

            SignIn();
        }

        void SignIn()
        {
            //구글 인증을 완료하면 클라우드 데이터를 불러온다.
            Social.localUser.Authenticate(success => {

                if (success)
                {
                    LoadData();
                    ShowAchievementsUI();
                }
                else
                    Debug.Log("Failed!");
            });
        }

        #region Saved Games

        //parsing string to game data (stored in CloudVariables), also deciding if we should use local or cloud save
        void StringToGameData(string cloudData, string localData)
        {
            if (cloudData == string.Empty)
            {
                //StringToGameData(localData);
                cloudData = DataManager.Instance.PlayerDataToString(new PlayerData());
                isCloudDataLoaded = true;
                return;
            }
            PlayerData cloudArray = DataManager.PlayerStringToData(cloudData);

            if (localData == string.Empty)
            {
                DataManager.Instance.CurrentPlayerData = cloudArray;
                PlayerPrefs.SetString(SAVE_NAME, cloudData);
                isCloudDataLoaded = true;
                return;
            }
            PlayerData localArray = DataManager.PlayerStringToData(localData);

            //if it's the first time that game has been launched after installing it and successfuly logging into Google Play Games
            if (PlayerPrefs.GetInt("IsFirstTime") == 1)
            {
                //set playerpref to be 0 (false)
                PlayerPrefs.SetInt("IsFirstTime", 0);
                PlayerPrefs.SetString(SAVE_NAME, cloudData);
            }
            //if it's not the first time, start comparing
            else
            {
                DataManager.Instance.CurrentPlayerData = localArray;
                isCloudDataLoaded = true;

                //saving the updated CloudVariables to the cloud
                SaveData();
                return;
            }

            //if the code above doesn't trigger return and the code below executes,
            //cloud save and local save are identical, so we can load either one
            DataManager.Instance.CurrentPlayerData = cloudArray;
            isCloudDataLoaded = true;
        }

        //used for loading data from the cloud or locally
        public void LoadData()
        {
            //basically if we're connected to the internet, do everything on the cloud
            if (Social.localUser.authenticated)
            {
                isSaving = false;
                ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithManualConflictResolution(SAVE_NAME,
                    DataSource.ReadCacheOrNetwork, true, ResolveConflict, OnSavedGameOpened);
            }
        }

        public void SaveData()
        {
            if (isCloudDataLoaded && Social.localUser.authenticated)
            {
                isSaving = true;
                ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithManualConflictResolution(SAVE_NAME,
                    DataSource.ReadCacheOrNetwork, true, ResolveConflict, OnSavedGameOpened);
            }
        }

        private void ResolveConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
        {
            if (originalData == null)
                resolver.ChooseMetadata(unmerged);
            else if (unmergedData == null)
                resolver.ChooseMetadata(original);
            else
            {
                /*
                //decoding byte data into string
                string originalStr = Encoding.ASCII.GetString(originalData);
                string unmergedStr = Encoding.ASCII.GetString(unmergedData);

                //parsing
                PlayerData originalArray = DataManager.PlayerStringToData(originalStr);
                PlayerData unmergedArray = DataManager.PlayerStringToData(unmergedStr);

                //데이터 비교.. 필요없음
                */

                resolver.ChooseMetadata(unmerged);
                return;
            }
        }

        private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            //if we are connected to the internet
            if (status == SavedGameRequestStatus.Success)
            {
                //if we're LOADING game data
                if (!isSaving)
                    LoadGame(game);
                //if we're SAVING game data
                else
                    SaveGame(game);
            }

            //클라우드 연결에 실패했을 경우
            else
            {

            }
        }

        private void LoadGame(ISavedGameMetadata game)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, OnSavedGameDataRead);
        }

        private void SaveGame(ISavedGameMetadata game)
        {
            string stringToSave = DataManager.Instance.PlayerDataToString();
            //saving also locally (can also call SaveLocal() instead)
            PlayerPrefs.SetString(SAVE_NAME, stringToSave);

            //encoding to byte array
            byte[] dataToSave = Encoding.ASCII.GetBytes(stringToSave);
            //updating metadata with new description
            SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
            //uploading data to the cloud
            ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(game, update, dataToSave, OnSavedGameDataWritten);
        }

        //callback for ReadBinaryData
        private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] savedData)
        {
            //if reading of the data was successful
            if (status == SavedGameRequestStatus.Success)
            {
                string cloudDataString;
                //저장된 데이터가 없을 경우
                if (savedData.Length == 0)
                    cloudDataString = DataManager.Instance.PlayerDataToString(new PlayerData());
                //저장된 데이터가 존재할 경우, byteArray를 string으로 converting
                else
                    cloudDataString = Encoding.ASCII.GetString(savedData);

                //getting local data (if we've never played before on this device, localData is already
                //string.Empty, so there's no need for checking as with cloudDataString)
                string localDataString = PlayerPrefs.GetString(SAVE_NAME);

                //this method will compare cloud and local data
                StringToGameData(cloudDataString, localDataString);
            }
        }

        //callback for CommitUpdate
        private void OnSavedGameDataWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            //디버깅을 위해 로그인마다 코인 10추가
            DataManager.Instance.CurrentPlayerData.unicoin += 10;
            SaveData();
        }
        #endregion /Saved Games

        #region Achievements
        public static void UnlockAchievement(string id)
        {
            Social.ReportProgress(id, 100, success => { });
        }

        public static void IncrementAchievement(string id, int stepsToIncrement)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
        }

        public static void ShowAchievementsUI()
        {
            Social.ShowAchievementsUI();
        }
        #endregion /Achievements

        #region Leaderboards
        public static void AddScoreToLeaderboard(string leaderboardId, long score)
        {
            Social.ReportScore(score, leaderboardId, success => { });
        }

        public static void ShowLeaderboardsUI()
        {
            Social.ShowLeaderboardUI();
        }
        #endregion /Leaderboards

    }
}