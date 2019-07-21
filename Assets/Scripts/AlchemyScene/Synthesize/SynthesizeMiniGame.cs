using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class SynthesizeMiniGame : MonoBehaviour
    {
        [SerializeField]
        private Image miniGameType;
        [SerializeField]
        private Image mixImage;
        [SerializeField]
        private List<Image> arrowImageList;
        [SerializeField]
        private List<Sprite> arrowSpriteList;

        private List<int> arrowType;
        private Touch touch;
        private Vector2 firstTouchPosition;
        private int arrowIndex;
        private bool canSuccess;
        private int completionTime;
        private float circleAngle;

        // Use this for initialization
        void Start()
        {

        }

        void SetMiniGame2()
        {
            arrowIndex = 0;
            completionTime = 0;
            mixImage.gameObject.SetActive(false);
            arrowType = new List<int>();
            SetArrow();
            StartCoroutine("GetTouchGesture");
            StartCoroutine("MeasureTime");
        }

        #region MiniGame2

        void SetArrow()
        {
            for (int i = 0; i < arrowImageList.Count; i++)
            {
                arrowType.Add(Random.Range(0, 4));
                arrowImageList[i].sprite = arrowSpriteList[arrowType[i]];
            }
        }

        IEnumerator GetTouchGesture()
        {
            while (arrowIndex < arrowImageList.Count)
            {
                if (Input.touchCount > 0)
                {
                    touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        firstTouchPosition = touch.position;
                        canSuccess = true;
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        if (!CheckTouchDirection(arrowType[arrowIndex], touch.position) && canSuccess)
                        {
                            canSuccess = false;
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        if (canSuccess)
                        {
                            arrowImageList[arrowIndex].gameObject.SetActive(false);
                            arrowIndex++;
                        }
                    }
                }
                yield return null;
            }

            StartCoroutine("GetTouchCircleGesture");
            yield return null;
        }

        IEnumerator MeasureTime()
        {
            while (completionTime < 13)
            {
                completionTime++;
                yield return new WaitForSecondsRealtime(1);
            }

            SendResult();
            yield return null;
        }

        IEnumerator GetTouchCircleGesture()
        {
            float previous = 0;
            float current = 0;

            mixImage.gameObject.SetActive(true);

            while (circleAngle < 300)
            {
                if (Input.touchCount > 0)
                {
                    touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        previous = 0;
                        current = GetAngle(Vector3.zero, Camera.main.ScreenToWorldPoint(touch.position));
                        circleAngle = 0;
                    }
                    else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
                    {
                        previous = current;
                        current = GetAngle(Vector3.zero, Camera.main.ScreenToWorldPoint(touch.position));

                        if (previous - current < 200 && previous - current > 0)
                            circleAngle += previous - current;

                        Debug.Log(circleAngle);
                    }
                }
                yield return null;
            }

            SendResult();
            yield return null;
        }

        float GetAngle(Vector3 vStart, Vector3 vEnd)
        {
            Vector3 v = vEnd - vStart;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            return angle;
        }

        void SendResult()
        {
            StopAllCoroutines();
            foreach (var image in arrowImageList)
            {
                image.gameObject.SetActive(true);
            }
            SynthesizeUI.Instance.OpenSynthesizeResultUI(completionTime);
        }

        bool CheckTouchDirection(int type, Vector2 touchPosition)
        {
            float distanceX = firstTouchPosition.x - touchPosition.x;
            float distanceY = firstTouchPosition.y - touchPosition.y;

            //오른쪽
            if (type == 0 && distanceX <= 0)
            {
                if (distanceY < 0)
                    distanceY *= -1;
                distanceX *= -1;
                if (distanceY < distanceX)
                    return true;
                else
                    return false;
            }
            //아래쪽
            else if (type == 1 && distanceY >= 0)
            {
                if (distanceX < 0)
                    distanceX *= -1;
                if (distanceY > distanceX)
                    return true;
                else
                    return false;
            }
            //왼쪽
            else if (type == 2 && distanceX >= 0)
            {
                if (distanceY < 0)
                    distanceY *= -1;
                if (distanceY < distanceX)
                    return true;
                else
                    return false;
            }
            //위쪽
            else if (type == 3 && distanceY <= 0)
            {
                if (distanceX < 0)
                    distanceX *= -1;
                distanceY *= -1;
                if (distanceY > distanceX)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }

    #endregion
}