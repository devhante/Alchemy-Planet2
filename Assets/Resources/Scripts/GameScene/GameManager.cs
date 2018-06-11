using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public float PlayTime;
        

        private void Awake()
        {
            Instance = this;
        }

        public void StartGame()
        {
            PlayTime = 0;
            StartCoroutine("TimeCount");
        }

        public void EndGame()
        {
            StopCoroutine("TimeCount");
        }

        IEnumerator TimeCount()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                ++PlayTime;
            }
        }
    }
}
