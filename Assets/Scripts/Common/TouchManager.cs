using AlchemyPlanet.PrologueScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.Common
{
    public class TouchManager : MonoBehaviour
    {
        public static TouchManager Instance { get; private set; }

        private int touchId = 0;
        public Vector3 StartTouchPos { get; private set; }
        public Vector3 CurrentTouchPos { get; private set; }
        public bool IsMoving { get; private set; }

        public GameObject joystickPrefab;
        public Joystick Joystick { get; private set; }

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

            StartTouchPos = Vector3.zero;
            CurrentTouchPos = Vector3.zero;
            IsMoving = false;
            Joystick = null;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                if (IsMoving == false)
                {
                    bool isBegan = false;

                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        if (Input.GetTouch(i).phase == TouchPhase.Began)
                        {
                            Touch touch = Input.GetTouch(i);
                            touchId = touch.fingerId;
                            StartTouchPos = touch.position;
                            isBegan = true;
                            break;
                        }
                    }

                    if (EventSystem.current.IsPointerOverGameObject(touchId) == false)
                    {
                        Vector3 worldPos = Camera.main.ScreenToWorldPoint(StartTouchPos);
                        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0, LayerMask.GetMask("SmallStructure"));

                        if (hit && (hit.transform.CompareTag("NPC") || hit.transform.CompareTag("InteractiveObject")))
                        {
                            GameObject.FindGameObjectWithTag("Player").SendMessage("Interact", hit);
                        }
                        else if(isBegan == true)
                        {
                            Joystick = Instantiate(joystickPrefab).transform.GetChild(0).GetComponent<Joystick>();
                            Joystick.gameObject.transform.position = StartTouchPos;
                            IsMoving = true;
                        }
                    }
                }
                else
                {
                    if (EventSystem.current.IsPointerOverGameObject(touchId) == false)
                    {
                        Touch touch = new Touch();
                        for (int i = 0; i < Input.touchCount; i++)
                        {
                            if (Input.GetTouch(i).fingerId == touchId)
                            {
                                touch = Input.GetTouch(i);
                                break;
                            }
                        }
                        CurrentTouchPos = touch.position;
                    }
                    else if(Joystick != null)
                    {
                        Destroy(Joystick.transform.parent.gameObject);
                        Joystick = null;
                        IsMoving = false;
                    }
                }
            }
            else if(Joystick != null)
            {
                Destroy(Joystick.transform.parent.gameObject);
                Joystick = null;
                IsMoving = false;
            }
        }
    }
}