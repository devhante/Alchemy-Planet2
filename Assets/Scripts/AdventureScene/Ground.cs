using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AlchemyPlanet.AdventureScene
{
    public class Ground : MonoBehaviour
    {
        public const float ANGLE = 40;

        private Sequence moveLeftSeq;
        private Sequence moveRightSeq;

        public void MoveLeft()
        {
            StartCoroutine(MoveLeftCoroutine());
        }

        public void MoveRight()
        {
            StartCoroutine(MoveRightCoroutine());
        }

        private IEnumerator MoveLeftCoroutine()
        {
            Vector3 startAngle = transform.localEulerAngles;
            Vector3 endAngle = new Vector3(0, 0, transform.localEulerAngles.z + ANGLE);

            while(true)
            {
                float t = AdventureManager.Instance.progress;
                transform.localEulerAngles = Vector3.Lerp(startAngle, endAngle, t);

                if (AdventureManager.Instance.progress == 1) break;
                yield return null;
            }

            if (Mathf.Abs(transform.localEulerAngles.z) > 1) Destroy(gameObject);
            yield return null;
        }

        private IEnumerator MoveRightCoroutine()
        {
            Vector3 startAngle = transform.localEulerAngles;
            Vector3 endAngle = new Vector3(0, 0, transform.localEulerAngles.z - ANGLE);

            while (true)
            {
                float t = AdventureManager.Instance.progress;
                transform.localEulerAngles = Vector3.Lerp(startAngle, endAngle, t);

                if (AdventureManager.Instance.progress == 1) break;
                yield return null;
            }

            if (Mathf.Abs(transform.localEulerAngles.z) > 1) Destroy(gameObject);
            yield return null;
        }
    }
}

