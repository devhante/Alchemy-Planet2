using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AlchemyPlanet.AdvectureScene
{
    public class Planet : MonoBehaviour
    {
        public UIManager.PlanetCurveVertex vertex;

        public void MoveIn(bool isClockwise)
        {
            StartCoroutine("MoveInCoroutine", isClockwise);
        }

        public void MoveOut(bool isClockwise)
        {
            StartCoroutine("MoveOutCoroutine", isClockwise);
        }

        public IEnumerator MoveInCoroutine(bool isClockwise)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, AdventureManager.Instance.animationTime).SetEase(Ease.OutQuint);

            float time = 0;

            while (true)
            {
                time += Time.deltaTime;
                if (time > AdventureManager.Instance.animationTime) time = AdventureManager.Instance.animationTime;

                float t = time / AdventureManager.Instance.animationTime;

                t = Mathf.Pow((t - 1), 5) + 1;

                if (isClockwise == true) transform.position = GetPointOnBezierCurve(vertex.back.position, vertex.left2.position, vertex.left1.position, vertex.front.position, t);
                else transform.position = GetPointOnBezierCurve(vertex.back.position, vertex.right2.position, vertex.right1.position, vertex.front.position, t);

                if (time == AdventureManager.Instance.animationTime)
                {
                    AdventureManager.Instance.isMoving = false;
                    break;
                }

                yield return null;
            }
        }

        public IEnumerator MoveOutCoroutine(bool isClockwise)
        {
            transform.localScale = Vector3.one;
            transform.DOScale(0, AdventureManager.Instance.animationTime).SetEase(Ease.OutQuint);

            float time = 0;

            while (true)
            {
                time += Time.deltaTime;
                if (time > AdventureManager.Instance.animationTime) time = AdventureManager.Instance.animationTime;

                float t = time / AdventureManager.Instance.animationTime;

                t = Mathf.Pow((t - 1), 5) + 1;

                if (isClockwise == true) transform.position = GetPointOnBezierCurve(vertex.front.position, vertex.right1.position, vertex.right2.position, vertex.back.position, t);
                else transform.position = GetPointOnBezierCurve(vertex.front.position, vertex.left1.position, vertex.left2.position, vertex.back.position, t);

                if (time == AdventureManager.Instance.animationTime)
                {
                    AdventureManager.Instance.isMoving = false;
                    Destroy(gameObject);
                }

                yield return null;
            }
        }

        private Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float u = 1f - t;
            float t2 = t * t;
            float u2 = u * u;
            float u3 = u2 * u;
            float t3 = t2 * t;

            Vector3 result =
                (u3) * p0 +
                (3f * u2 * t) * p1 +
                (3f * u * t2) * p2 +
                (t3) * p3;

            return result;
        }
    }
}