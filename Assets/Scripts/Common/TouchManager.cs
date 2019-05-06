using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.Common
{
    public class TouchManager : MonoBehaviour
    {
        public static TouchManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private int touchId;
        private Vector3 startTouchPos;
        private Vector3 currentTouchPos;
        private bool touched = false;

        public GameObject joystickPrefab;
        private Joystick joystick;

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                if (touched == false)
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        if (Input.GetTouch(i).phase == TouchPhase.Began)
                        {
                            Touch touch = Input.GetTouch(i);
                            touchId = touch.fingerId;
                            startTouchPos = touch.position;
                            joystick = Instantiate(joystickPrefab).transform.GetChild(0).GetComponent<Joystick>();
                            joystick.gameObject.transform.position = startTouchPos;
                            touched = true;
                            break;
                        }
                    }
                }
                else
                {
                    Touch touch = new Touch();
                    for(int i = 0; i < Input.touchCount; i++)
                    {
                        if(Input.GetTouch(i).fingerId == touchId)
                        {
                            touch = Input.GetTouch(i);
                            break;
                        }
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        Destroy(joystick.transform.parent.gameObject);
                        touched = false;
                    }
                    else
                    {
                        currentTouchPos = touch.position;
                    }
                }
            }
        }

        public Vector3 GetStartTouchPos()
        {
            return startTouchPos;
        }

        public Vector3 GetCurrentTouchPos()
        {
            return currentTouchPos;
        }
    }
}