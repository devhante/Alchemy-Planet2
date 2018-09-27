using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.StoryLobbyScene
{
    public class Star : MonoBehaviour
    {
        public RectTransform mask;
        public Transform on;

        //[HideInInspector]
        public float rate = 0;

        private float height = 68;

        private void Start()
        {
            StartCoroutine("StarCoroutine");
        }

        IEnumerator StarCoroutine()
        {
            while (true)
            {
                rate = Mathf.Clamp(rate, 0, 1);

                float offsetMaxY = (1 - rate) * -68;
                mask.offsetMax = new Vector2(mask.offsetMax.x, offsetMaxY);
                on.localPosition = new Vector3(on.localPosition.x, -offsetMaxY / 2.0f);
                yield return null;
            }
        }
    }
}