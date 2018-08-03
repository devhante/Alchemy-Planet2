using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class AlchemyUI : Common.UI<AlchemyUI>
    {
        [SerializeField] private Button InventoryButton;
        [SerializeField] private Button resultButton;
        [SerializeField] private Button[] ingredientButtons;
        [SerializeField] private Button makeButton;

        public GameObject resultSelectUI;
        public GameObject ingredientSelectUI;

        protected override void Awake()
        {
            base.Awake();

            InventoryButton.onClick.AddListener(OnClickInventoryButton);

            resultButton.onClick.AddListener(OnClickResultButton);

            foreach (var item in ingredientButtons)
                item.onClick.AddListener(OnClickIngredientButton);

            makeButton.onClick.AddListener(OnClickMakeButton);
        }

        private void OnClickInventoryButton()
        {
            TownScene.UIManager.Instance.OpenMenu<TownScene.InventoryCell>();
        }

        private void OnClickResultButton()
        {
            Instantiate(resultSelectUI, Vector3.zero, Quaternion.identity);
        }

        private void OnClickIngredientButton()
        {
            Instantiate(ingredientSelectUI, Vector3.zero, Quaternion.identity);
        }

        private void OnClickMakeButton()
        {
            
        }   
    }
}