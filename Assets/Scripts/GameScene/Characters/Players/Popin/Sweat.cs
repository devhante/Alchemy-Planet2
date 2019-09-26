using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Sweat : MonoBehaviour
    {
        public static Sweat Instance;

        public GameObject[] sweats;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
        }

        public void StartSweatAnimation()
        {
            StartCoroutine(SweatCoroutine());
        }

        public void StopSweatAnimation()
        {
            StopCoroutine(SweatCoroutine());
            foreach (var item in sweats)
                item.SetActive(false);
        }

        IEnumerator SweatCoroutine()
        {
            while(true)
            {
                foreach(var item in sweats)
                {
                    item.SetActive(true);
                    yield return new WaitForSeconds(0.1f);
                }

                foreach (var item in sweats)
                    item.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}