using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class MiniGame2 : MonoBehaviour
    {
        enum ArrowType { Right, Up, Left, Down }

        [SerializeField]
        private Image mixImage;
        [SerializeField]
        private List<Image> arrowImageList;
        [SerializeField]
        private List<Sprite> arrowSpriteList;

        private SynthesizeMiniGame synthesizeMiniGame;
        private List<int> arrowTypeList;
        private Touch touch;
        private Vector2 firstTouchPosition;
        private int arrowIndex;
        private bool canSuccess;
        private int completionTime;
        private float circleAngle;

        private void Start()
        {
            arrowIndex = 0;
            completionTime = 0;
            arrowTypeList = new List<int>();
            mixImage.gameObject.SetActive(false);
            synthesizeMiniGame = GetComponentInParent<SynthesizeMiniGame>();
            SetArrow();
            StartCoroutine("GetTouchGesture");
        }

        void SetArrow()
        {
            for (int i = 0; i < arrowImageList.Count; i++)
            {
                arrowTypeList.Add(Random.Range(0, 4));
                arrowImageList[i].sprite = arrowSpriteList[arrowTypeList[i]];
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
                        if (!CheckGestureDirection(arrowTypeList[arrowIndex], touch.position) && canSuccess)
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

        IEnumerator GetTouchCircleGesture()
        {
            float previous = 0;
            float current = 0;

            mixImage.gameObject.SetActive(true);

            while (circleAngle < 270)
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
                    }
                }
                yield return null;
            }

            synthesizeMiniGame.SendResult();
            yield return null;
        }

        float GetAngle(Vector3 vStart, Vector3 vEnd)
        {
            Vector3 v = vEnd - vStart;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            return angle;
        }

        bool CheckGestureDirection(int type, Vector2 touchPosition)
        {
            ArrowType arrowType;

            float distanceX = firstTouchPosition.x - touchPosition.x;
            float distanceY = firstTouchPosition.y - touchPosition.y;

            if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY))
                arrowType = distanceX < 0 ? ArrowType.Right : ArrowType.Left;
            else
                arrowType = distanceY < 0 ? ArrowType.Up : ArrowType.Down;

            return (int)arrowType == type;
        }
    }
}