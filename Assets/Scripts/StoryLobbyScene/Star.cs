using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AlchemyPlanet.StoryLobbyScene
{
    public class Star : MonoBehaviour
    {
        public RectTransform mask;
        public Transform on;

        //[HideInInspector]
        public float rate = 0;

        private bool isRateOne = false;


        private void Start()
        {
            StartCoroutine(StarCoroutine());
        }

        IEnumerator StarCoroutine()
        {
            while (true)
            {
                rate = Mathf.Clamp(rate, 0, 1);

                if(isRateOne == false && rate == 1)
                {
                    isRateOne = true;
                    Sequence sq = DOTween.Sequence();
                    sq.Append(transform.DOScale(Vector3.one * 2, 0.3f)).SetEase(Ease.OutQuint);
                    sq.Append(transform.DOScale(Vector3.one * 1, 0.5f)).SetEase(Ease.OutQuint);
                }

                float offsetMaxY = (1 - rate) * -68;
                mask.offsetMax = new Vector2(mask.offsetMax.x, offsetMaxY);
                on.localPosition = new Vector3(on.localPosition.x, -offsetMaxY / 2.0f);
                yield return null;
            }
        }
    }
}