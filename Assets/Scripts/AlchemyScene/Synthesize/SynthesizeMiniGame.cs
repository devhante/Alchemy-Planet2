using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class SynthesizeMiniGame : MonoBehaviour
    {
        [SerializeField]
        private Image miniGameType;
        [SerializeField]
        private GameObject miniGame1Prefab;
        [SerializeField]
        private GameObject miniGame2Prefab;
        [SerializeField]
        private Sprite miniGameTypeSprite1;
        [SerializeField]
        private Sprite miniGameTypeSprite2;

        private int greatProbability;

        private void Start()
        {
            StartMiniGame1();
        }

        public void StartMiniGame1()
        {
            Instantiate(miniGame1Prefab, gameObject.transform);
            miniGameType.sprite = miniGameTypeSprite1;
        }

        public void StartMiniGame2()
        {
            Instantiate(miniGame2Prefab, gameObject.transform);
            miniGameType.sprite = miniGameTypeSprite2;
        }
        
        void SendResult()
        {
            
            SynthesizeManager.Instance.OpenSynthesizeResultUI(greatProbability);
        }

        public void AddGreatProbability(int probability)
        {
            greatProbability += probability;
        }
    }
}