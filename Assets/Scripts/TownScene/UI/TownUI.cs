using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;
using DG.Tweening;

namespace AlchemyPlanet.TownScene
{
    public class TownUI : Common.UI<TownUI>
    {
        public GameObject player;
        public Camera mainCamera;
        public bool turnOnBuildBar;

        [SerializeField] private Button buildingbutton;
        [SerializeField] private GameObject buildBar;
        [SerializeField] private Button UIOffButton;
        [SerializeField] private Button TownManageButton;
        [SerializeField] private Button TownUpgradeButton;
        [SerializeField] private Button InventoryButton;

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(LateAwake());
        }

        private void Start()
        {
            DataManager.Instance.LoadPlayerData();
            GetComponent<CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;

            List<GameObject> setupBuildingList = new List<GameObject>();

            foreach (GameObject obj in DataManager.Instance.CurrentPlayerData.setupBuildings.Keys)
                setupBuildingList.Add(obj);

            foreach (GameObject obj in setupBuildingList) // 저장된 타운 불러오기
            {
                string str = DataManager.Instance.CurrentPlayerData.setupBuildings[obj];
                DataManager.Instance.CurrentPlayerData.setupBuildings.Remove(obj);
                DataManager.Instance.CurrentPlayerData.setupBuildings.Add(Instantiate(obj), str);
            }
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
                UIManager.Instance.menuStack.Peek().gameObject.SetActive(false);
                UIManager.Instance.OpenMenu<EmptyUI>();
            });
            TownManageButton.onClick.AddListener(() =>
            {
                UIManager.Instance.menuStack.Peek().gameObject.SetActive(false);
                UIManager.Instance.OpenMenu<TownManager>();
            });
            TownUpgradeButton.onClick.AddListener(() =>
            {
                UIManager.Instance.OpenMenu<TownUpgrade>();
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
                //buildBar.transform.DOMoveX(535, 1).SetEase(Ease.OutQuint);
                Debug.Log(Screen.width);
                buildBar.transform.DOMoveX(Screen.width - 200 * (Screen.width / 720.0f), 1).SetEase(Ease.OutQuint);
                turnOnBuildBar = true;
            }
            else
            {
                //buildBar.transform.DOMoveX(900, 1).SetEase(Ease.OutQuint);
                buildBar.transform.DOMoveX(Screen.width + 150 * (Screen.width / 720.0f), 1).SetEase(Ease.OutQuint);
                turnOnBuildBar = false;
            }

            yield return null;
        }
    }
}