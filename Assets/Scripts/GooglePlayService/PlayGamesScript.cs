using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

using GooglePlayGames.BasicApi.SavedGame;
using System.Text;
using System;

public class PlayGamesScript : MonoBehaviour {
    public static PlayGamesScript Instance { get; private set; }

    const string SAVE_NAME = "Test";
    bool isSaving;
    bool isCloudDataLoaded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start () {

        if (!PlayerPrefs.HasKey(SAVE_NAME))
            PlayerPrefs.SetString(SAVE_NAME, "0");
        if (!PlayerPrefs.HasKey("IsFirstTime"))
            PlayerPrefs.SetInt("IsFirstTime", 1);

        LoadLocal();

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames().Build();

        // enables saving game progress.
        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.Activate();

        SignIn();
    }

    void SignIn()
    {
        Social.localUser.Authenticate((bool succcess) => {
            if (succcess)
            {
                LoadData();
                ShowAchievementsUI();
                SampleUIscript.Instance.SetState(true);
            }
            else
                SampleUIscript.Instance.SetState(false);
        });
    }

    #region Saved Games

    string GameDataToString()
    {
        return CloudVariables.Highscore.ToString();
    }

    void StringToGameData(string cloudData, string localData)
    {
        if(PlayerPrefs.GetInt("IsFirstTime") == 1)
        {
            PlayerPrefs.SetInt("IsFirstTime", 0);
            if(int.Parse(cloudData) > int.Parse(localData))
            {
                PlayerPrefs.SetString(SAVE_NAME, cloudData);
            }
        }
        else
        {
            if (int.Parse(localData) > int.Parse(cloudData))
            {
                CloudVariables.Highscore = int.Parse(localData);
                AddScoreToLeaderboard(GPGSIds.leaderboard_point, CloudVariables.Highscore);
                isCloudDataLoaded = true;
                SaveData();
                return;
            }
        }
        CloudVariables.Highscore = int.Parse(cloudData);
        isCloudDataLoaded = true;
    }

    void StringToGameData(string localData)
    {
        CloudVariables.Highscore = int.Parse(localData);
    }

    public void LoadData()
    {
        if (Social.localUser.authenticated)
        {
            isSaving = false;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithManualConflictResolution(SAVE_NAME,
                DataSource.ReadCacheOrNetwork, true, ResolveConflict, OnSavedGameOpened);
        }
        else
        {
            LoadLocal();
        }
    }

    private void LoadLocal()
    {
        StringToGameData(PlayerPrefs.GetString(SAVE_NAME));
    }

    public void SaveData()
    {
        if (!isCloudDataLoaded)
        {
            SaveLocal();
            return;
        }
        if (Social.localUser.authenticated)
        {
            isSaving = true;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithManualConflictResolution(SAVE_NAME,
                DataSource.ReadCacheOrNetwork, true, ResolveConflict, OnSavedGameOpened);
        }
        else
        {
            SaveLocal();
        }
    }

    private void SaveLocal()
    {
        PlayerPrefs.SetString(SAVE_NAME, GameDataToString());
    }

    private void ResolveConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData,
        ISavedGameMetadata unmerged, byte[] unmergedData)
    {
        if (originalData == null)
            resolver.ChooseMetadata(unmerged);
        if (unmergedData == null)
            resolver.ChooseMetadata(original);
        else
        {
            string originalStr = Encoding.Default.GetString(originalData);
            string unmergedStr = Encoding.Default.GetString(unmergedData);

            int originalNum = int.Parse(originalStr);
            int unmergedNum = int.Parse(unmergedStr);

            if(originalNum > unmergedNum)
            {
                resolver.ChooseMetadata(original);
                return;
            }
            else if (unmergedNum > originalNum)
            {
                resolver.ChooseMetadata(unmerged);
                return;
            }
            resolver.ChooseMetadata(original);
        }
    }

    private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            if (!isSaving)
                LoadGame(game);
            else
                SavedGame(game);
        }
        else
        {
            if (!isSaving)
                LoadLocal();
            else
                SaveLocal();
        }
    }

    private void LoadGame(ISavedGameMetadata game)
    {
        ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, OnSavedGameDataRead);
    }

    private void SavedGame(ISavedGameMetadata game)
    {
        string stringToSave = GameDataToString();
        PlayerPrefs.SetString(SAVE_NAME, stringToSave);

        byte[] dataToSave = Encoding.Default.GetBytes(stringToSave);

        SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();

        ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(game, update, dataToSave,
            OnSavedGameDataWritten);
    }

    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] savedData)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            string cloudDataString;
            if (savedData.Length == 0)
                cloudDataString = "0";
            else
                cloudDataString = Encoding.Default.GetString(savedData);

            string localDataString = PlayerPrefs.GetString(SAVE_NAME);

            StringToGameData(cloudDataString, localDataString);
        }
    }

    private void OnSavedGameDataWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {

    }



    #endregion Saved Games

    #region Achievements
    public static void UnloadAchievement(string Id)
    {
        Social.ReportProgress(Id, 100, success => { });
    }

    public static void IncerementAchievement(string id, int stepsToIncerement)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncerement, success => { });
    }

    public static void ShowAchievementsUI()
    {
        Social.ShowAchievementsUI();
    }
    #endregion /

    #region Leaderboards
    public static void AddScoreToLeaderboard(string leaderboardId, long score)
    {
        Social.ReportScore(score, leaderboardId, success => { });
    }

    public static void ShowLeaderboardsUI()
    {
        Social.ShowLeaderboardUI();
    }
    #endregion /
}
