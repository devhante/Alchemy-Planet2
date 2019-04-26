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
            NotFirstTimeFunc();
        }

        public void FirstTimeFunc()
        {
            Debug.Log("FirstTimeFunc");
            prologueButton.gameObject.SetActive(true);
        }

        public void NotFirstTimeFunc()
        {
            Debug.Log("NotFirstTimeFunc");
            townButton.gameObject.SetActive(true);
            // DataManager.Instance.LoadPlayerData();
        }

        private void LoadPrologueScene()
        {
            Debug.Log("LoadPrologueScene");
            SceneChangeManager.Instance.ChangeSceneWithLoading("PrologueScene");
        }

        private void LoadTownScene()
        {
            Debug.Log("LoadPrologueScene");
            SceneChangeManager.Instance.ChangeSceneWithLoading("TownScene");
        }
    }
}