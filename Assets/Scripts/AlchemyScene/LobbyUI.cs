using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField]
        private Button SynthesizeButton;
        [SerializeField]
        private Button DecopmoseButton;
        [SerializeField]
        private Button TransformButton;
        [SerializeField]
        private Button MaterialDictionaryButton;
        [SerializeField]
        private Button RequestButton;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void ButtonMapping()
        {
            SynthesizeButton.onClick.AddListener(() => UIManager.Instance.OpenMenu<SynthesizeUI>());
            DecopmoseButton.onClick.AddListener(() => UIManager.Instance.OpenMenu<DecomposeUI>());
        }
    }
}