using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class SynthesizeUI : Common.UI<SynthesizeUI>
    {
        [SerializeField]
        private GameObject SynthesizeSelectUI;
        [SerializeField]
        private GameObject SynthesizeInfoUI;
        [SerializeField]
        private GameObject SynthesizeMiniGame;
        [SerializeField]
        private GameObject SynthesizeResultUI;
        [SerializeField]
        private GameObject AlchemyStateBar;

        private GameObject StateBarUI;
        private GameObject TopDownUI;

        public string itemName { get; private set; }
        public int itemCount { get; private set; }

        // Use this for initialization
        private void Start()
        {
            StateBarUI = GameObject.Find("StateBar");
            TopDownUI = GameObject.Find("TopDownUI");
            StateBarUI.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {

        }

        public void OpenSynthesizeSelectUI()
        {
            SynthesizeSelectUI.SetActive(true);
            AlchemyStateBar.SetActive(true);
            TopDownUI.SetActive(true);
            SynthesizeInfoUI.SetActive(false);
            SynthesizeResultUI.SetActive(false);
        }

        public void OpenSynthesizeInfoUI(string itemName)
        {
            this.itemName = itemName;

            SynthesizeSelectUI.SetActive(false);
            AlchemyStateBar.SetActive(false);
            TopDownUI.SetActive(false);
            SynthesizeInfoUI.SetActive(true);

            SynthesizeInfoUI.SendMessage("GetFomula",itemName);
        }

        public void OpenSynthesizeMiniGame(int itemCount)
        {
            this.itemCount = itemCount;
            
            SynthesizeInfoUI.SetActive(false);
            SynthesizeMiniGame.SetActive(true);
            SynthesizeMiniGame.SendMessage("SetMiniGame2");
        }

        public void OpenSynthesizeResultUI(int completionTime)
        {
            SynthesizeMiniGame.SetActive(false);
            SynthesizeResultUI.SetActive(true);

            SynthesizeResultUI.SendMessage("GetResult", completionTime);
        }
    }
}