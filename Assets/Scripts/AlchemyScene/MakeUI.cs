using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class MakeUI : Common.UI<AlchemyUI> {
        [SerializeField] private Button[] MateraiButtons = new Button[5];
        //제작할 아이템의 5 가지 종류를 나타내는 선반위의 UI

        [SerializeField] private Button ResultButton;
        [SerializeField] private Button MakeButton;

        protected override void Awake()
        {
            base.Awake();

            foreach (var item in MateraiButtons)
                item.onClick.AddListener(OnClickMateraiButton);

            MakeButton.onClick.AddListener(OnClickMakeButton);

            ResultButton.onClick.AddListener(OnClickResultButton);
        }
        
        private void OnClickMateraiButton()
        {

        }

        private void OnClickMakeButton()
        {

        }

        private void OnClickInventoryButton()
        {
            TownScene.UIManager.Instance.OpenMenu<TownScene.InventoryCell>();
        }

        private void OnClickResultButton()
        {

        }
    }
}

