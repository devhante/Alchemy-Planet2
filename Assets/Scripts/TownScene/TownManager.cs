using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.TownScene
{
    public class TownManager : MonoBehaviour
    {

        public List<GameObject> BuildingPrefabs;
        public GameObject player;
        public GameObject NPC;

        private GameObject clickedBuilding;
        private Touch tempTouch;   // 터치들
        private Vector3 touchedPos; // 터치위치

        private void OnEnable()
        {
            player.SetActive(false);
            NPC.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            DetectTouch();
            DetectClick();
        }

        void DetectTouch()    // 터치감지
        {
            if (Input.touchCount > 0)
            {
                tempTouch = Input.GetTouch(0);
                if (tempTouch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject() == false)
                {
                    touchedPos = Camera.main.ScreenToWorldPoint(tempTouch.position);
                    RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);
                    if (hit.collider.tag == "Building")
                    {
                        clickedBuilding = hit.collider.gameObject;
                    }
                }
            }
        }

        void DetectClick()    // 클릭감지
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);
                if (hit && EventSystem.current.IsPointerOverGameObject() == false)
                {
                    if (hit.collider.tag == "Building")
                    {
                        clickedBuilding = hit.collider.gameObject;
                    }
                }
            }
        }

        void Exit()
        {
            player.SetActive(true);
            NPC.SetActive(true);
            UIManager.Instance.menuStack.Peek().gameObject.SetActive(true);
        }

        void MoveBuilding()
        {
            if (Input.GetMouseButton(0) || tempTouch.phase == TouchPhase.Moved)
            {
                clickedBuilding.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, clickedBuilding.transform.position.y, clickedBuilding.transform.position.z);
            }
        }

        void RotateBuilding() { clickedBuilding.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX ? false : true; }

        void Build(GameObject obj)
        {
            clickedBuilding = Instantiate(obj, Camera.main.ScreenToWorldPoint(new Vector3(0f, -1.3f, 0f)), Quaternion.Euler(0, 0, 0));
        }
    }
}
