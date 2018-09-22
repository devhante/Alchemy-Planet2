using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class GameSettings : MonoBehaviour
    {
        public static GameSettings Instance { get; private set; }

        public bool isAbilityActivated;

        public int monsterNumber;
        public float monsterCooltime;

        public ItemName[] itemChanges_Key;
        public float[] itemChanges_Value;

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