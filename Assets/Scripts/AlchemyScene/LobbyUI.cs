using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField]
        private Button synthesizeButton;
        [SerializeField]
        private Button decopmoseButton;
        [SerializeField]
        private Button transformButton;
        [SerializeField]
        private Button materialDictionaryButton;
        [SerializeField]
        private Button requestButton;
        [SerializeField]
        private GameObject synthesizeManager;

        void Start()
        {
            ButtonMapping();
        }

        void ButtonMapping()
        {
            synthesizeButton.onClick.AddListener(()=>Instantiate(synthesizeManager));
            
        }
    }
}