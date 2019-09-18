using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class SynthesizeManager : Common.UI<SynthesizeManager>
    {
        [SerializeField]
        private GameObject synthesizeSelectUI;
        [SerializeField]
        private GameObject synthesizeInfoUI;
        [SerializeField]
        private GameObject synthesizeMiniGame;
        [SerializeField]
        private GameObject loadingPage;
        [SerializeField]
        private GameObject synthesizeResultUI;
        
        private GameObject alchemyStateBar;
        private GameObject stateBarUI;
        private GameObject topDownUI;

        public string itemName{get; private set;}
        public int itemCount{get; private set;}
        public int completionTime{get; private set;}

        private void Start()
        {
            stateBarUI = GameObject.Find("StateBar");
            topDownUI = GameObject.Find("TopDownUI(Clone)");
            stateBarUI.SetActive(false);
            OpenSynthesizeSelectUI();
        }

        public void OpenSynthesizeSelectUI()
        {
            topDownUI.SetActive(true);
            Instantiate(synthesizeSelectUI, gameObject.transform);
        }

        public void OpenSynthesizeInfoUI(string itemName)
        {
            this.itemName = itemName;
            topDownUI.SetActive(false);
            Instantiate(synthesizeInfoUI, gameObject.transform);
        }

        public void OpenSynthesizeMiniGame(int itemCount)
        {
            this.itemCount = itemCount;
            Instantiate(synthesizeMiniGame, gameObject.transform);
        }

        public void OpenLoadingPage()
        {
            Instantiate(loadingPage, gameObject.transform);
        }

        public void OpenSynthesizeResultUI(int completionTime)
        {
            this.completionTime = completionTime;
            
            Instantiate(synthesizeResultUI, gameObject.transform);
        }
    }
}