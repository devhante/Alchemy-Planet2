using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AlchemyPlanet.GameScene
{
    public class Line : MonoBehaviour
    {
        RectTransform rt;
        public Vector3 start, end;
        bool isMouseButtonDown;
        float width;
        float multiplierX = 720f / Screen.width;
        float multiplierY = 1280f / Screen.height;

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
            isMouseButtonDown = true;
            width = 20;
        }

        private void Start()
        {
            StartCoroutine("DrawCoroutine");
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
                isMouseButtonDown = false;
        }

        IEnumerator DrawCoroutine()
        {
            while (isMouseButtonDown)
            {
                end = Input.mousePosition;

                Draw();
                yield return null;
            }
        }

        public void Draw()
        {
            Vector3 differenceVector;
            if (1f * Screen.height / Screen.width >= 16f / 9f)
                differenceVector = (end - start) * multiplierX;
            else
                differenceVector = (end - start) * multiplierY;

            rt.sizeDelta = new Vector2(differenceVector.magnitude, width);
            rt.pivot = new Vector2(0, 0.5f);
            rt.position = start;
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            rt.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}