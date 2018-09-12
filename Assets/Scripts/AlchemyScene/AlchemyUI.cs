﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class AlchemyUI : Common.UI<AlchemyUI>
    {
        [SerializeField] private Button[] ItemKindButtons;
        //제작할 아이템의 5 가지 종류를 나타내는 선반위의 UI

        [SerializeField] private GameObject Book;
        //아이템 리스트를 띄울 책
        
        [SerializeField] private Button ProducePrefab;

        [SerializeField] private Button RequestButton;
        [SerializeField] private Text PageInfo;
        [SerializeField] private Button BookmarkButton;
        [SerializeField] private Button SortButton;

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(LateAwake());
        }

        public IEnumerator LateAwake()
        {
            while (AlchemyManager.Instance == null)
            {
                yield return null;
            }

            OnClickItemKindButton(Data.ItemKind.Diffuser);

            for (int i=0; i<ItemKindButtons.Length; ++i)
            {
                int num = i;
                ItemKindButtons[i].onClick.AddListener(() => OnClickItemKindButton((Data.ItemKind)num));
            }

            RequestButton.onClick.AddListener(OnClickRequestButton);
        }

        private void OnClickItemKindButton(Data.ItemKind kind)
        {
            for(int i=0; i<Book.transform.childCount; ++i)
            {
                Destroy(Book.transform.GetChild(i).gameObject);
            }
            foreach (var produce in AlchemyManager.Instance.formulas)
            {
                Data.Material m = Data.DataManager.Instance.materials[produce.result];
                if (m.item_kind.Equals(kind))
                {
                    var button = GameObject.Instantiate(ProducePrefab, Book.transform);
                    button.GetComponent<Image>().sprite = m.image;
                    button.onClick.AddListener(ProduceButton);
                }
            }
        }

        public void ProduceButton()
        {
            UIManager.Instance.OpenMenu<MakeUI>();
        }

        private void OnClickRequestButton()
        {
            UIManager.Instance.OpenMenu<RequestUI>();
        }

        private void OnClickInventoryButton()
        {
            TownScene.UIManager.Instance.OpenMenu<TownScene.InventoryCell>();
        }
    }
}