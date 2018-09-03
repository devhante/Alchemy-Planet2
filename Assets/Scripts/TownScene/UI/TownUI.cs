﻿using System.Collections;
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
            GetComponent<CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;

            DataManager.Instance.CurrentPlayerData.inventory.Add("Red", 1);
            DataManager.Instance.CurrentPlayerData.inventory.Add("Blue", 2);

            DataManager.Instance.CurrentPlayerData.structures.Add(DataManager.Instance.structures["House"]);
            DataManager.Instance.CurrentPlayerData.structures[0].setup = true;
            DataManager.Instance.CurrentPlayerData.structures.Add(DataManager.Instance.structures["Tree"]);
            DataManager.Instance.CurrentPlayerData.structures[1].setup = true;
            DataManager.Instance.CurrentPlayerData.structures.Add(DataManager.Instance.structures["Tree"]);
            DataManager.Instance.CurrentPlayerData.structures[2].setup = false;

            foreach (Structure strc in DataManager.Instance.CurrentPlayerData.structures) // 저장된 타운 불러오기
            {
                Debug.Log(strc.structureName + " : " + strc.setup);
                if(strc.setup)
                    strc.StructureObject = Instantiate(strc.StructureObject);
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
                buildBar.transform.DOMoveX(Screen.width - 190 * (Screen.width / 720.0f), 1).SetEase(Ease.OutQuint);
                turnOnBuildBar = true;
            }
            else
            {
                //buildBar.transform.DOMoveX(900, 1).SetEase(Ease.OutQuint);
                buildBar.transform.DOMoveX(Screen.width + 190 * (Screen.width / 720.0f), 1).SetEase(Ease.OutQuint);
                turnOnBuildBar = false;
            }

            yield return null;
        }
    }
}