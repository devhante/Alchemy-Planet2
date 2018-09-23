﻿using AlchemyPlanet.Data;
using AlchemyPlanet.GameScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AlchemyPlanet.StoryLobbyScene
{
    public class StoryManager : MonoBehaviour
    {
        public static StoryManager Instance { get; private set; }

        public GameObject stars;
        public Sprite starOn;
        public Sprite starOff;

        public int CurrentChaper { get; set; }
        public int CurrentStage { get; set; }
        public int CurrentMaxStage { get; set; }

        private GameObject starsObject;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "GameScene" && scene.name != "StoryLobbyScene" && scene.name != "LoadingScene")
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                Destroy(gameObject);
            }

            if (scene.name == "GameScene")
            {
                starsObject = Instantiate(stars, GameUI.Instance.transform);

                StartCoroutine("ChallengeCoroutine");
                StartCoroutine("StarCoroutine");
            }
        }

        IEnumerator ChallengeCoroutine()
        {
            if(CurrentChaper == 1)
            {
                if(CurrentStage == 2)
                {
                    GameSettings.Instance.isAbilityActivated = false;
                    GameSettings.Instance.monsterNumber = 0;
                    
                    for(int i = 0; i < GameSettings.Instance.itemChanges_Key.Length; i++)
                    {
                        if (GameSettings.Instance.itemChanges_Key[i] == ItemName.RainbowColorBall)
                            GameSettings.Instance.itemChanges_Value[i] = 1;
                        else
                            GameSettings.Instance.itemChanges_Value[i] = 0;
                    }

                    while (ItemManager.Instance.CreatedItemNumber[ItemName.RainbowColorBall] <= 0)
                        yield return null;

                    for (int i = 0; i < GameSettings.Instance.itemChanges_Key.Length; i++)
                    {
                        if (GameSettings.Instance.itemChanges_Key[i] == ItemName.SlowReducedOxygen)
                            GameSettings.Instance.itemChanges_Value[i] = 1;
                        else
                            GameSettings.Instance.itemChanges_Value[i] = 0;
                    }

                    while (ItemManager.Instance.CreatedItemNumber[ItemName.SlowReducedOxygen] <= 0)
                        yield return null;

                    for (int i = 0; i < GameSettings.Instance.itemChanges_Key.Length; i++)
                    {
                        if (GameSettings.Instance.itemChanges_Key[i] == ItemName.IncreasePurify)
                            GameSettings.Instance.itemChanges_Value[i] = 1;
                        else
                            GameSettings.Instance.itemChanges_Value[i] = 0;
                    }

                    yield return null;
                }
            }
        }

        IEnumerator StarCoroutine()
        {
            bool[] isStarOn = new bool[3];

            if (CurrentChaper == 1)
            {
                if (CurrentStage == 2)
                {
                    while(true)
                    {
                        if (ItemManager.Instance.UsedItemNumber[ItemName.RainbowColorBall] > 0)
                        {
                            starsObject.transform.GetChild(0).GetComponent<Image>().sprite = starOn;
                            isStarOn[0] = true;
                        }
                        if(ItemManager.Instance.UsedItemNumber[ItemName.SlowReducedOxygen] > 0)
                        {
                            starsObject.transform.GetChild(1).GetComponent<Image>().sprite = starOn;
                            isStarOn[1] = true;
                        }
                        if(ItemManager.Instance.UsedItemNumber[ItemName.IncreasePurify] > 0)
                        {
                            starsObject.transform.GetChild(2).GetComponent<Image>().sprite = starOn;
                            isStarOn[2] = true;
                        }

                        if (isStarOn[0] == true && isStarOn[1] == true && isStarOn[2] == true)
                            break;

                        yield return null;
                    }

                    WebSocketManager.Instance.SendUpdatePlayerStoryStar("0", "0", "1-2", 3);
                    DataManager.Instance.CurrentPlayerData.stroystar["1-2"] = 3;

                    if (DataManager.Instance.CurrentPlayerData.stroystar.ContainsKey("1-3") == false)
                    {
                        WebSocketManager.Instance.SendInsertStoryStar("0", "0", "1-3", 0);
                        DataManager.Instance.CurrentPlayerData.stroystar.Add("1-3", 0);
                    }
                    UIManager.Instance.OpenMenu<EndUI>();
                }
            }
        }
    }
}
