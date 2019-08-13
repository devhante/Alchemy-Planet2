using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.MainScene
{
    public class DeveloperModeButton : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                Data.PlayGamesScript.Instance.isDeveloperMode = !Data.PlayGamesScript.Instance.isDeveloperMode;
            });
        }
    }
}