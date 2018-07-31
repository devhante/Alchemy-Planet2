using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyTest
{
    public class AlchemyUI : Common.UI<AlchemyUI>
    {
        public Button resultButton;
        public Button[] ingredientButtons;
        public Button makeButton;

        public GameObject resultSelectUI;
        public GameObject ingredientSelectUI;

        protected override void Awake()
        {
            base.Awake();
            resultButton.onClick.AddListener(OnClickResultButton);

            foreach (var item in ingredientButtons)
                item.onClick.AddListener(OnClickIngredientButton);

            makeButton.onClick.AddListener(OnClickMakeButton);
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