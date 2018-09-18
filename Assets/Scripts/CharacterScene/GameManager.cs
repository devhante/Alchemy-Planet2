using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.CharacterScene
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; set; }

        public Data.Character[,] Parties { get; set; } 

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            Parties = new Data.Character[9, 3];
        }
    }
}