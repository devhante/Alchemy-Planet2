using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.AdventureScene
{
    public class AdventureManager : MonoBehaviour
    {
        public static AdventureManager Instance { get; private set; }

        public readonly float animationTime = 1;
        public readonly float fastAnimationTime = 0.3f;
        public int planetCount;

        [HideInInspector] public int planetNumber;
        [HideInInspector] public float progress;
        [HideInInspector] public bool isFast;

        private bool isLeft;
        private float remainingTime;

        private void OnDestroy() { Instance = null; }
            
        private void Awake()
        {
            Instance = this;
            planetNumber = 0;
            progress = 0;
            isFast = false;
            isLeft = false;
            remainingTime = 0;
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveLeft();
            else if (Input.GetKeyDown(KeyCode.RightArrow)) MoveRight();

            remainingTime = Mathf.Max(remainingTime - Time.deltaTime, 0);
            if (isFast) progress = 1 - (remainingTime / fastAnimationTime);
            else progress = 1 - (remainingTime / animationTime);
        }

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
            while (isFast == true) yield return null;

            if (planetNumber > 0)
            {
                if (remainingTime > 0)
                {
                    if (isLeft == false) progress = 1 - progress;

                    isFast = true;
                    remainingTime = (1 - progress) * fastAnimationTime;

                    bool flag = true;
                    while (flag)
                    {
                        if (remainingTime == 0) flag = false;
                        yield return null;
                    }
                }

                isLeft = true;
                isFast = false;
                remainingTime = animationTime;
                progress = 0;
                planetNumber--;
                UIManager.Instance.MoveLeft();

                yield return null;
            }
        }

        private IEnumerator MoveRightCoroutine()
        {
            while (isFast == true) yield return null;

            if (planetNumber < planetCount - 1)
            {
                if (remainingTime > 0)
                {
                    if (isLeft == true) progress = 1 - progress;

                    isFast = true;
                    remainingTime = (1 - progress) * fastAnimationTime;

                    bool flag = true;
                    while(flag)
                    {
                        if (remainingTime == 0) flag = false;
                        yield return null;
                    }
                }

                isLeft = false;
                isFast = false;
                remainingTime = animationTime;
                progress = 0;
                planetNumber++;
                UIManager.Instance.MoveRight();

                yield return null;
            }
        }
    }
}