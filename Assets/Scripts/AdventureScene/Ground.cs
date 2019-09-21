using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AlchemyPlanet.AdvectureScene
{
    public class Ground : MonoBehaviour
    {
        public const float ANGLE = 40;

        public void MoveLeft()
        {
            Vector3 destination = new Vector3(0, 0, transform.localEulerAngles.z + ANGLE);

            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DORotate(destination, AdventureManager.Instance.animationTime));
            seq.OnComplete(() => { if (Mathf.Abs(transform.localEulerAngles.z) > 1) Destroy(gameObject); });
            seq.Play();
        }

        public void MoveRight()
        {
            Vector3 destination = new Vector3(0, 0, transform.localEulerAngles.z - ANGLE);
            
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DORotate(destination, AdventureManager.Instance.animationTime));
            seq.OnComplete(() => { if (Mathf.Abs(transform.localEulerAngles.z) > 1) Destroy(gameObject); });
            seq.Play();
        }
    }
}

