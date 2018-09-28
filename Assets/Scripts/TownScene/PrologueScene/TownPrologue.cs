using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace AlchemyPlanet.TownScene
{
    public class TownPrologue : MonoBehaviour
    {
        public List<Sprite> tutorialSprite;
        public List<Button> tutorialButton;
        public Image tutorialImage;
        public Image hand;

        private int stage = 0;
        private bool movingHand = false;
        private Sequence sequence;

        // Use this for initialization
        void Start()
        {
            tutorialButton[0].onClick.AddListener(() =>
            {
                stage = 1;
                movingHand = false;
                sequence.Kill();
                TownScene.TownUI.Instance.StartCoroutine("MoveBar");
            });
            tutorialButton[1].onClick.AddListener(() =>
            {
                stage = 2;
                movingHand = false;
                sequence.Kill();
            Data.DataManager.Instance.CurrentPlayerData.buildings.Add(Data.DataManager.Instance.buildingInfo["House"].Clone());
                Data.DataManager.Instance.CurrentPlayerData.GiveId(Data.DataManager.Instance.CurrentPlayerData.buildings[0]);
                Data.WebSocketManager.Instance.SendInsertBuilding("0",
                    Data.DataManager.Instance.CurrentPlayerData.player_id,
                    Data.DataManager.Instance.CurrentPlayerData.buildings[0].id.ToString(),
                    Data.DataManager.Instance.CurrentPlayerData.buildings[0].buildingName,
                    Data.DataManager.Instance.CurrentPlayerData.buildings[0].buildingLevel,
                    Data.DataManager.Instance.CurrentPlayerData.buildings[0].position,
                    Data.DataManager.Instance.CurrentPlayerData.buildings[0].setup,
                    Data.DataManager.Instance.CurrentPlayerData.buildings[0].flip,
                    Data.DataManager.Instance.CurrentPlayerData.buildings[0].upgrading,
                    Data.DataManager.Instance.CurrentPlayerData.buildings[0].UpgradeEndTime);
                Data.DataManager.Instance.CurrentPlayerData.interiors.Add(Data.DataManager.Instance.interiorInfo["Tree"].Clone());
                Data.DataManager.Instance.CurrentPlayerData.GiveId(Data.DataManager.Instance.CurrentPlayerData.interiors[0]);
                Data.WebSocketManager.Instance.SendInsertInterior("0",
                    Data.DataManager.Instance.CurrentPlayerData.player_id,
                    Data.DataManager.Instance.CurrentPlayerData.interiors[0].id.ToString(),
                    Data.DataManager.Instance.CurrentPlayerData.interiors[0].interiorName,
                    Data.DataManager.Instance.CurrentPlayerData.interiors[0].position,
                    Data.DataManager.Instance.CurrentPlayerData.interiors[0].setup,
                    Data.DataManager.Instance.CurrentPlayerData.interiors[0].flip);
                Data.DataManager.Instance.CurrentPlayerData.interiors.Add(Data.DataManager.Instance.interiorInfo["CherryBlossom"].Clone());
                Data.DataManager.Instance.CurrentPlayerData.GiveId(Data.DataManager.Instance.CurrentPlayerData.interiors[1]);
                Data.WebSocketManager.Instance.SendInsertInterior("0",
                    Data.DataManager.Instance.CurrentPlayerData.player_id,
                    Data.DataManager.Instance.CurrentPlayerData.interiors[1].id.ToString(),
                    Data.DataManager.Instance.CurrentPlayerData.interiors[1].interiorName,
                    Data.DataManager.Instance.CurrentPlayerData.interiors[1].position,
                    Data.DataManager.Instance.CurrentPlayerData.interiors[1].setup,
                    Data.DataManager.Instance.CurrentPlayerData.interiors[1].flip);
                TownUI.Instance.mainCamera.GetComponent<MainCamera>().StartCoroutine("ZoomOut");
                UIManager.Instance.TownUIOff();
                UIManager.Instance.OpenMenu<BuildingPlacement>();
            });
            tutorialButton[2].onClick.AddListener(() =>
            {
                stage = 3;
                movingHand = false;
                sequence.Kill();
                BuildingPlacement.Instance.SendMessage("Build", "House");
            });
            tutorialButton[3].onClick.AddListener(() =>
            {
                stage = 4;
                movingHand = false;
                sequence.Kill();
                BuildingPlacement.Instance.SendMessage("Exit");
            });
            tutorialButton[4].onClick.AddListener(() =>
            {
                SceneChangeManager.Instance.ChangeSceneWithLoading("PlanetSelect");
            });
        }

        // Update is called once per frame
        void Update()
        {
            checkTutorial();
        }

        void checkTutorial()
        {
            if (!movingHand)
            {
                movingHand = true;
                switch (stage)
                {
                    case 0:
                        tutorialImage.sprite = tutorialSprite[0];
                        MoveHand(0);
                        break;
                    case 1:
                        tutorialImage.sprite = tutorialSprite[1];
                        MoveHand(1);
                        break;
                    case 2:
                        tutorialImage.sprite = tutorialSprite[2];
                        MoveHand(2);
                        break;
                    case 3:
                        tutorialImage.sprite = tutorialSprite[3];
                        MoveHand(3);
                        break;
                    case 4:
                        tutorialImage.sprite = tutorialSprite[4];
                        MoveHand(4);
                        break;
                }
            }
        }

        void MoveHand(int n)
        {
            sequence = DOTween.Sequence();
            sequence.Append(hand.transform.DOMove(
                new Vector3(tutorialButton[n].transform.position.x - 80, tutorialButton[n].transform.position.y - 80)
                , 0.5f, false))
                .OnComplete(() =>
                {
                    hand.transform.DOMove(
    new Vector3(hand.transform.position.x + 30, hand.transform.position.y + 30)
    , 0.5f, false)
    .OnComplete(() => { movingHand = false; });
                });
        }
    }

}