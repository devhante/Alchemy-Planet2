using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;
using DG.Tweening;
using UnityEditor;

namespace AlchemyPlanet.TownScene
{
    public class TownUI : Common.UI<TownUI>
    {
        public GameObject player;
        public Camera mainCamera;
        public bool turnOnBuildBar;
        public GameObject tutorial;
        public List<GameObject> npc;

        [SerializeField] private Button buildingbutton;
        [SerializeField] private GameObject buildBar;
        [SerializeField] private Button UIOffButton;
        [SerializeField] private Button BuildingPlacementButton;
        [SerializeField] private Button BuildingManagementButton;
        [SerializeField] private Button InventoryButton;

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(LateAwake());
        }

        private void Start()
        {

            GetComponent<CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;

            //if(DataManager.Instance.CurrentPlayerData.buildings.Count <= 0)
            //{
            //    Instantiate(tutorial);
            //}

            DataManager.Instance.CurrentPlayerData.setupBuildilngs = new List<GameObject>();

            foreach (Building strc in DataManager.Instance.CurrentPlayerData.buildings) // 저장된 타운 불러오기
            {
                if (strc.setup)
                {
                    strc.Build();
                    DataManager.Instance.CurrentPlayerData.setupBuildilngs.Add(Instantiate(strc.buildingObject));
                }
            }
            foreach (Interior strc in DataManager.Instance.CurrentPlayerData.interiors) // 저장된 타운 불러오기
            {
                if (strc.setup)
                {
                    strc.Build();
                    DataManager.Instance.CurrentPlayerData.setupBuildilngs.Add(Instantiate(strc.interiorObject));
                }
            }
            Common.SoundManager.Instance.PlayBGM(1);
        }

        IEnumerator LateAwake()
        {
            while (UIManager.Instance == null)
            {
                yield return null;
            }
            UIManager.Instance.Clear();
            UIManager.Instance.menuStack.Push(Instance);
            this.transform.SetParent(UIManager.Instance.transform);

            turnOnBuildBar = false;
            buildingbutton.onClick.AddListener(() =>
            {
                StartCoroutine("MoveBar");
            });
            UIOffButton.onClick.AddListener(() =>
            {
                UIManager.Instance.TownUIOff();
                UIManager.Instance.OpenMenu<EmptyUI>();
            });
            BuildingPlacementButton.onClick.AddListener(() =>
            {
                foreach(GameObject obj in npc)
                {
                    obj.SetActive(false);
                }
                mainCamera.GetComponent<MainCamera>().StartCoroutine("ZoomOut");
                UIManager.Instance.TownUIOff();
                UIManager.Instance.OpenMenu<BuildingPlacement>();
            });
            BuildingManagementButton.onClick.AddListener(() =>
            {
                UIManager.Instance.OpenMenu<BuildingManagement>();
            });
            InventoryButton.onClick.AddListener(() =>
            {
                UIManager.Instance.OpenMenu<InventoryCell>();

                InventoryCell.Instance.SetItem();
            });
        }

        IEnumerator MoveBar()
        {
            if (!turnOnBuildBar)
            {
                Debug.Log(Screen.width);
                buildBar.transform.DOMoveX(Screen.width - 190 * (Screen.width / 720.0f), 1).SetEase(Ease.OutQuint);
                turnOnBuildBar = true;
            }
            else
            {
                buildBar.transform.DOMoveX(Screen.width + 190 * (Screen.width / 720.0f), 1).SetEase(Ease.OutQuint);
                turnOnBuildBar = false;
            }

            yield return null;
        }
    }
}