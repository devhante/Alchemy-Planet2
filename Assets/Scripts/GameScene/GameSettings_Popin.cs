using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class GameSettings_Popin : MonoBehaviour
    {
        public static GameSettings_Popin Instance { get; private set; }

        public PopinPotionColor[] popinPotionChances_Key;
        public float[] popinPotionChanges_Value;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}