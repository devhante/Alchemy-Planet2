using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.Common
{
    public class Joystick : MonoBehaviour
    {
        public Vector3 Normal { get; private set; }
        Transform controller;
        float radius;

        private void Awake()
        {
            controller = transform.GetChild(0);
            radius = GetComponent<RectTransform>().sizeDelta.x * 0.5f;
        }

        private void Start()
        {
            StartCoroutine(ChangeControllerPositionCoroutine());
            StartCoroutine(PointerDown());
        }

        IEnumerator PointerDown()
        {
            while (true)
            {
                Vector3 centerPos = TouchManager.Instance.StartTouchPos;
                Vector3 touchPos = TouchManager.Instance.CurrentTouchPos;

                float distance = Vector3.Distance(centerPos, touchPos);
                if (distance > radius) touchPos = centerPos + Vector3.ClampMagnitude((touchPos - centerPos), radius);
                controller.position = touchPos;

                Normal = Vector3.Normalize(touchPos - centerPos);

                yield return null;
            }
        }

        private IEnumerator ChangeControllerPositionCoroutine()
        {
            while (true)
            {
                Vector3 centerPos = transform.position;
                Vector3 controllerPos = controller.position;
                Vector3 destination = centerPos;
                Vector3 destinationFrame = controllerPos + (destination - controllerPos).normalized * radius * 4 * Time.deltaTime;
                float distance = Vector3.Distance(centerPos, destinationFrame);

                if (distance != 0)
                {
                    if (distance > radius) destinationFrame = centerPos + Vector3.ClampMagnitude((destinationFrame - centerPos), radius);
                    if ((destination - destinationFrame).magnitude <= 10.0f) destinationFrame = destination;
                    controller.position = destinationFrame;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}