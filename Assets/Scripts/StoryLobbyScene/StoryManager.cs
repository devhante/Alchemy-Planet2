using AlchemyPlanet.Data;
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

        public GameObject storyTutorial;
        public GameObject stars;
        public Sprite starOn;
        public Sprite starOff;

        public int CurrentChaper { get; set; }
        public int CurrentStage { get; set; }
        public int CurrentMaxStage { get; set; }

        private Stars starsScript;

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
                starsScript = Instantiate(stars, GameUI.Instance.transform).GetComponent<Stars>();

                StartCoroutine("ChallengeCoroutine");
                StartCoroutine("StarCoroutine");
            }
        }

        IEnumerator ChallengeCoroutine()
        {
            if(CurrentChaper == 1)
            {
                if(CurrentStage == 1)
                {
                    Instantiate(storyTutorial);
                    GameSettings.Instance.isAbilityActivated = false;
                    GameSettings.Instance.itemPercent = 0;
                    yield return null;
                }

                //if(CurrentStage == 2)
                //{
                //    GameSettings.Instance.isAbilityActivated = false;
                //    for(int i = 0; i < GameSettings.Instance.itemChanges_Key.Length; i++)
                //    {
                //        if (GameSettings.Instance.itemChanges_Key[i] == ItemName.RainbowColorBall)
                //            GameSettings.Instance.itemChanges_Value[i] = 1;
                //        else
                //            GameSettings.Instance.itemChanges_Value[i] = 0;
                //    }

                //    while (ItemManager.Instance.CreatedItemNumber[ItemName.RainbowColorBall] <= 0)
                //        yield return null;

                //    for (int i = 0; i < GameSettings.Instance.itemChanges_Key.Length; i++)
                //    {
                //        if (GameSettings.Instance.itemChanges_Key[i] == ItemName.SlowReducedOxygen)
                //            GameSettings.Instance.itemChanges_Value[i] = 1;
                //        else
                //            GameSettings.Instance.itemChanges_Value[i] = 0;
                //    }

                //    while (ItemManager.Instance.CreatedItemNumber[ItemName.SlowReducedOxygen] <= 0)
                //        yield return null;

                //    for (int i = 0; i < GameSettings.Instance.itemChanges_Key.Length; i++)
                //    {
                //        if (GameSettings.Instance.itemChanges_Key[i] == ItemName.IncreasePurify)
                //            GameSettings.Instance.itemChanges_Value[i] = 1;
                //        else
                //            GameSettings.Instance.itemChanges_Value[i] = 0;
                //    }

                //    yield return null;
                //}

                else if(CurrentStage == 3)
                {
                    GameSettings.Instance.isAbilityActivated = false;
                    GameSettings.Instance.monsterCooltime = 10;
                }

                else if(CurrentStage == 4)
                {
                    GameSettings.Instance.isAbilityActivated = false;
                    GameSettings.Instance.monsterCooltime = 10;

                    for (int i = 0; i < GameSettings.Instance.itemChanges_Key.Length; i++)
                    {
                        GameSettings.Instance.itemChanges_Value[i] = 0;
                    }
                }

                else if(CurrentStage == 5)
                {
                    GameSettings.Instance.monsterCooltime = 10;

                    for(int i = 0; i < GameSettings_Popin.Instance.popinPotionChances_Key.Length; i++)
                    {
                        if (GameSettings_Popin.Instance.popinPotionChances_Key[i] == PopinPotionColor.Red)
                            GameSettings_Popin.Instance.popinPotionChanges_Value[i] = 1;
                        else
                            GameSettings_Popin.Instance.popinPotionChanges_Value[i] = 0;
                    }

                    while (Popin.Instance.UsedSkillNumber[PopinPotionColor.Red] < 1)
                            yield return null;

                    for (int i = 0; i < GameSettings_Popin.Instance.popinPotionChances_Key.Length; i++)
                    {
                        if (GameSettings_Popin.Instance.popinPotionChances_Key[i] == PopinPotionColor.Blue)
                            GameSettings_Popin.Instance.popinPotionChanges_Value[i] = 1;
                        else
                            GameSettings_Popin.Instance.popinPotionChanges_Value[i] = 0;
                    }

                    while (Popin.Instance.UsedSkillNumber[PopinPotionColor.Blue] < 1)
                        yield return null;

                    for (int i = 0; i < GameSettings_Popin.Instance.popinPotionChances_Key.Length; i++)
                    {
                        if (GameSettings_Popin.Instance.popinPotionChances_Key[i] == PopinPotionColor.Green)
                            GameSettings_Popin.Instance.popinPotionChanges_Value[i] = 1;
                        else
                            GameSettings_Popin.Instance.popinPotionChanges_Value[i] = 0;
                    }
                }

                else if(CurrentStage == 6)
                {
                    GameSettings.Instance.monsterCooltime = 10;

                    for (int i = 0; i < GameSettings_Popin.Instance.popinPotionChances_Key.Length; i++)
                    {
                        if (GameSettings_Popin.Instance.popinPotionChances_Key[i] == PopinPotionColor.Rainbow)
                            GameSettings_Popin.Instance.popinPotionChanges_Value[i] = 1;
                        else
                            GameSettings_Popin.Instance.popinPotionChanges_Value[i] = 0;
                    }

                    while (Popin.Instance.UsedSkillNumber[PopinPotionColor.Rainbow] < 1)
                        yield return null;

                    for (int i = 0; i < GameSettings_Popin.Instance.popinPotionChances_Key.Length; i++)
                    {
                        if (GameSettings_Popin.Instance.popinPotionChances_Key[i] == PopinPotionColor.Black)
                            GameSettings_Popin.Instance.popinPotionChanges_Value[i] = 1;
                        else
                            GameSettings_Popin.Instance.popinPotionChanges_Value[i] = 0;
                    }

                    while (Popin.Instance.UsedSkillNumber[PopinPotionColor.Black] < 1)
                        yield return null;

                    for (int i = 0; i < GameSettings_Popin.Instance.popinPotionChances_Key.Length; i++)
                    {
                        if (GameSettings_Popin.Instance.popinPotionChances_Key[i] == PopinPotionColor.Rainbow || GameSettings_Popin.Instance.popinPotionChances_Key[i] == PopinPotionColor.Black)
                            GameSettings_Popin.Instance.popinPotionChanges_Value[i] = 0.4f;
                        else
                            GameSettings_Popin.Instance.popinPotionChanges_Value[i] = 0.06f;
                    }
                }
            }
        }

        IEnumerator StarCoroutine()
        {
            bool[] isStarOn = new bool[3];

            if (CurrentChaper == 1)
            {
                if(CurrentStage == 1)
                {
                    while(true)
                    {
                        if(StoryTutorial.Instance.isTutorialFinished == true)
                        {
                            starsScript.stars[0].rate = 1;
                            isStarOn[0] = true;
                        }

                        starsScript.stars[1].rate = MaterialManager.Instance.DestroyedMaterialNumber / 30.0f;
                        if (MaterialManager.Instance.DestroyedMaterialNumber >= 30)
                            isStarOn[1] = true;

                        starsScript.stars[2].rate = MaterialManager.Instance.ChainedNumber / 5.0f;
                        if (MaterialManager.Instance.ChainedNumber >= 5)
                            isStarOn[2] = true;

                        if (isStarOn[0] == true && isStarOn[1] == true && isStarOn[2] == true)
                            break;

                        yield return null;
                    }

                    BackendManager.Instance.UpdateStageNumber(BackendManager.Instance.GetInDate("stage"), "1-1", 3);
                    DataManager.Instance.CurrentPlayerData.stroystar["1-1"] = 3;

                    if (DataManager.Instance.CurrentPlayerData.stroystar.ContainsKey("1-2") == false)
                    {
                        BackendManager.Instance.AddStage(BackendManager.Instance.GetInDate("stage"), "1-2", 0);
                        DataManager.Instance.CurrentPlayerData.stroystar.Add("1-2", 0);
                    }

                    yield return new WaitForSeconds(2);
                    UIManager.Instance.OpenMenu<EndUI>();
                }

                if (CurrentStage == 2)
                {
                    //while(true)
                    //{
                    //    if (ItemManager.Instance.UsedItemNumber[ItemName.RainbowColorBall] > 0)
                    //    {
                    //        starsScript.stars[0].rate = 1;
                    //        isStarOn[0] = true;
                    //    }
                    //    if(ItemManager.Instance.UsedItemNumber[ItemName.SlowReducedOxygen] > 0)
                    //    {
                    //        starsScript.stars[1].rate = 1;
                    //        isStarOn[1] = true;
                    //    }
                    //    if(ItemManager.Instance.UsedItemNumber[ItemName.IncreasePurify] > 0)
                    //    {
                    //        starsScript.stars[2].rate = 1;
                    //        isStarOn[2] = true;
                    //    }

                    //    if (isStarOn[0] == true && isStarOn[1] == true && isStarOn[2] == true)
                    //        break;

                    //    yield return null;
                    //}

                    BackendManager.Instance.UpdateStageNumber(BackendManager.Instance.GetInDate("stage"), "1-2", 3);
                    DataManager.Instance.CurrentPlayerData.stroystar["1-2"] = 3;

                    if (DataManager.Instance.CurrentPlayerData.stroystar.ContainsKey("1-3") == false)
                    {
                        BackendManager.Instance.AddStage(BackendManager.Instance.GetInDate("stage"), "1-3", 0);
                        DataManager.Instance.CurrentPlayerData.stroystar.Add("1-3", 0);
                    }

                    yield return new WaitForSeconds(2);
                    UIManager.Instance.OpenMenu<EndUI>();
                }

                else if(CurrentStage == 3)
                {
                    starsScript.stars[1].rate = 1;
                    isStarOn[1] = true;

                    while (true)
                    {
                        starsScript.stars[0].rate = MonsterManager.Instance.DeadMonsterNumber / 5.0f;
                        if(MonsterManager.Instance.DeadMonsterNumber >= 5)
                            isStarOn[0] = true;

                        if(Player.Instance.PlayerHitNumber > 15)
                        {
                            starsScript.stars[1].rate = 0;
                            isStarOn[1] = false;
                        }

                        starsScript.stars[2].rate = MaterialManager.Instance.ChainedNumber / 10.0f;
                        if(MaterialManager.Instance.ChainedNumber >= 10)
                            isStarOn[2] = true;

                        if (isStarOn[0] == true && isStarOn[2] == true)
                            break;

                        yield return null;
                    }

                    int starNumber = 2;
                    if (isStarOn[1] == true)
                        starNumber++;

                    BackendManager.Instance.UpdateStageNumber(BackendManager.Instance.GetInDate("stage"), "1-3", starNumber);
                    DataManager.Instance.CurrentPlayerData.stroystar["1-3"] = starNumber;

                    if (DataManager.Instance.CurrentPlayerData.stroystar.ContainsKey("1-4") == false)
                    {
                        BackendManager.Instance.AddStage(BackendManager.Instance.GetInDate("stage"), "1-4", 0);
                        DataManager.Instance.CurrentPlayerData.stroystar.Add("1-4", 0);
                    }

                    yield return new WaitForSeconds(2);
                    UIManager.Instance.OpenMenu<EndUI>();
                }

                else if(CurrentStage == 4)
                {
                    starsScript.stars[1].rate = 1;
                    isStarOn[1] = true;

                    while(true)
                    {
                        starsScript.stars[0].rate = StageManager.Instance.MovedDistance / 50.0f;
                        if (StageManager.Instance.MovedDistance >= 50)
                            isStarOn[0] = true;

                        if(GameScene.GameManager.Instance.Second > 40)
                        {
                            starsScript.stars[1].rate = 0;
                            isStarOn[1] = false;
                        }

                        isStarOn[2] = true;

                        if (isStarOn[0] == true && isStarOn[2] == true)
                            break;

                        yield return null;
                    }

                    int starNumber = 2;
                    if (isStarOn[1] == true)
                        starNumber++;

                    BackendManager.Instance.UpdateStageNumber(BackendManager.Instance.GetInDate("stage"), "1-4", starNumber);
                    DataManager.Instance.CurrentPlayerData.stroystar["1-4"] = starNumber;

                    if (DataManager.Instance.CurrentPlayerData.stroystar.ContainsKey("1-5") == false)
                    {
                        BackendManager.Instance.AddStage(BackendManager.Instance.GetInDate("stage"), "1-5", 0);
                        DataManager.Instance.CurrentPlayerData.stroystar.Add("1-5", 0);
                    }

                    yield return new WaitForSeconds(2);
                    UIManager.Instance.OpenMenu<EndUI>();
                }

                else if(CurrentStage == 5)
                {
                    while (true)
                    {
                        int usedSkillNumber = 0;
                        foreach (var item in Popin.Instance.UsedSkillNumber)
                            usedSkillNumber += item.Value;

                        starsScript.stars[2].rate = 1;
                        isStarOn[2] = true;

                        starsScript.stars[0].rate = usedSkillNumber / 3.0f;
                        if (usedSkillNumber >= 3)
                            isStarOn[0] = true;

                        starsScript.stars[1].rate = MaterialManager.Instance.DestroyedMaterialNumber / 50.0f;
                        if (MaterialManager.Instance.DestroyedMaterialNumber >= 50)
                            isStarOn[1] = true;

                        if (GameScene.GameManager.Instance.Second > 90)
                        {
                            starsScript.stars[2].rate = 0;
                            isStarOn[2] = false;
                        }

                        if (isStarOn[0] == true && isStarOn[1] == true)
                            break;

                        yield return null;
                    }

                    int starNumber = 2;
                    if (isStarOn[2] == true)
                        starNumber++;

                    BackendManager.Instance.UpdateStageNumber(BackendManager.Instance.GetInDate("stage"), "1-5", starNumber);
                    DataManager.Instance.CurrentPlayerData.stroystar["1-5"] = starNumber;

                    if (DataManager.Instance.CurrentPlayerData.stroystar.ContainsKey("1-6") == false)
                    {
                        BackendManager.Instance.AddStage(BackendManager.Instance.GetInDate("stage"), "1-6", 0);
                        DataManager.Instance.CurrentPlayerData.stroystar.Add("1-6", 0);
                    }

                    yield return new WaitForSeconds(2);
                    UIManager.Instance.OpenMenu<EndUI>();
                }

                else if(CurrentStage == 6)
                {
                    while(true)
                    {
                        starsScript.stars[0].rate = Popin.Instance.UsedSkillNumber[PopinPotionColor.Rainbow] / 2.0f;
                        if(Popin.Instance.UsedSkillNumber[PopinPotionColor.Rainbow] >= 2)
                            isStarOn[0] = true;

                        starsScript.stars[1].rate = Popin.Instance.UsedSkillNumber[PopinPotionColor.Black] / 2.0f;
                        if(Popin.Instance.UsedSkillNumber[PopinPotionColor.Black] >= 2)
                            isStarOn[1] = true;

                        starsScript.stars[2].rate = MonsterManager.Instance.DeadMonsterNumber / 5.0f;
                        if(MonsterManager.Instance.DeadMonsterNumber >= 5)
                            isStarOn[2] = true;

                        if (isStarOn[0] == true && isStarOn[1] == true && isStarOn[2] == true)
                            break;

                        yield return null;
                    }

                    BackendManager.Instance.UpdateStageNumber(BackendManager.Instance.GetInDate("stage"), "1-6", 3);
                    DataManager.Instance.CurrentPlayerData.stroystar["1-6"] = 3;

                    if (DataManager.Instance.CurrentPlayerData.stroystar.ContainsKey("1-7") == false)
                    {
                        BackendManager.Instance.AddStage(BackendManager.Instance.GetInDate("stage"), "1-7", 0);
                        DataManager.Instance.CurrentPlayerData.stroystar.Add("1-7", 0);
                    }

                    yield return new WaitForSeconds(2);
                    UIManager.Instance.OpenMenu<EndUI>();
                }
            }
        }
    }
}
