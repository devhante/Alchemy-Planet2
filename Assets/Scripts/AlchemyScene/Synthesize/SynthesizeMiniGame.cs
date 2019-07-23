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
        private GameObject miniGamePrefab1;
        [SerializeField]
        private GameObject miniGamePrefab2;
        [SerializeField]
        private Sprite miniGameTypeSprite1;
        [SerializeField]
        private Sprite miniGameTypeSprite2;


        void SetMiniGame1()
        {
            miniGamePrefab1.SetActive(true);
            miniGamePrefab2.SetActive(false);
            miniGameType.sprite = miniGameTypeSprite1;
            miniGamePrefab1.SendMessage("SetMniGame1");
        }

        void SetMiniGame2()
        {
            miniGamePrefab1.SetActive(false);
            miniGamePrefab2.SetActive(true);
            miniGameType.sprite = miniGameTypeSprite2;
            miniGamePrefab1.SendMessage("SetMniGame2");
        }
    }
}