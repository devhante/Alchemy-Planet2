using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.TownScene {
    public class EmptyUI : Common.UI
    {

        [SerializeField] private Button button;

        private void Awake()
        {
            button.onClick.AddListener(() => {
                UIManager.Instance.CloseMenu();
                UIManager.Instance.TownUIOn();
            });
        }
    }
}
