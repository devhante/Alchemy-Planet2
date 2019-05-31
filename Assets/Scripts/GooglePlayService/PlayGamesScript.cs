using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Text;
using UnityEngine;
using AlchemyPlanet.Data;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using AlchemyPlanet.Common;
using BackEnd;

namespace AlchemyPlanet.Data
{
    public class PlayGamesScript : MonoBehaviour
    {
        public static PlayGamesScript Instance { get; private set; }
        [SerializeField] private Button townButton;
        [SerializeField] private Button prologueButton;

        const string SAVE_NAME = "PlayData";
        bool isSaving;
        bool isCloudDataLoaded = false;

        private void Awake()
        {
            townButton.onClick.AddListener(LoadTownScene);
            prologueButton.onClick.AddListener(LoadPrologueScene);
        }

        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (Instance != this)
            {
                Destroy(this.gameObject);
            }

            DataManager.Instance.CurrentPlayerData = new PlayerData();

            if (Backend.BMember.IsAccessTokenAlive().GetStatusCode().ToString() == "204")
            {
                Debug.Log(Backend.BMember.IsAccessTokenAlive().GetStatusCode());
                NotFirstTimeFunc();
            }
            else
            {
                Debug.Log(Backend.BMember.IsAccessTokenAlive().GetStatusCode());
                FirstTimeFunc();
            }
        }

        public void FirstTimeFunc()
        {
            //prologueButton.gameObject.SetActive(true);
            BackendManager.Instance.CustomLogin();
            CreatePlayer();
            townButton.gameObject.SetActive(true);
            Debug.Log("first");
        }

        public void NotFirstTimeFunc()
        {
            townButton.gameObject.SetActive(true);
            Debug.Log("notfirst");
        }

        private void LoadPrologueScene()
        {
            Debug.Log("LoadPrologueScene");

            // 임시 코드
            BackendManager.Instance.CustomLogin();

            SceneChangeManager.Instance.ChangeSceneWithLoading("PrologueScene");
        }

        private void LoadTownScene()
        {
            Debug.Log("LoadPrologueScene");
            SceneChangeManager.Instance.ChangeSceneWithLoading("TownScene");
        }

        private void CreatePlayer()
        {
            DataManager.Instance.CurrentPlayerData.player_id = DataManager.Instance.CreatePlayerId();
            DataManager.Instance.CurrentPlayerData.player_name = "player";
            DataManager.Instance.CurrentPlayerData.unicoin = 0;
            DataManager.Instance.CurrentPlayerData.cosmostone = 0;
            DataManager.Instance.CurrentPlayerData.oxygentank = 0;
            BackendManager.Instance.CreatePlayer(DataManager.Instance.CurrentPlayerData.player_id, DataManager.Instance.CurrentPlayerData.player_name);

            BackendManager.Instance.CreateItem(DataManager.Instance.CurrentPlayerData.player_id);
            BackendManager.Instance.CreateTown(DataManager.Instance.CurrentPlayerData.player_id);
            BackendManager.Instance.CreateCharacter(DataManager.Instance.CurrentPlayerData.player_id);

            BackendManager.Instance.CreateParty(DataManager.Instance.CurrentPlayerData.player_id);
            DataManager.Instance.CurrentPlayerData.party[0, 0] = CharacterEnum.Popin;
            BackendManager.Instance.AddParty(BackendManager.Instance.GetInDate("party"), 1, "0", "", "");

            BackendManager.Instance.CreateStage(DataManager.Instance.CurrentPlayerData.player_id);
            DataManager.Instance.CurrentPlayerData.stroystar["1-1"] = 0;
            BackendManager.Instance.AddStage(BackendManager.Instance.GetInDate("stage"), "1-1", 0);

            BackendManager.Instance.CreateRequest(DataManager.Instance.CurrentPlayerData.player_id);
        }
    }
}