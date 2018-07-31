using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.TownScene
{
    public class TownManager : Common.UI
    {
        public List<Image> buildingImages;        // 건물 미리보기
        public GameObject player;           // 플레이어
        public GameObject NPC;              // NPC
        public Camera mainCamera;           // 카메라

        private List<GameObject> Building;      // 소유중인 건물
        private GameObject clickedBuilding;     // 선택된 건물
        private Touch tempTouch;                // 터치들
        private Vector3 touchedPos;             // 터치위치
        private bool moving;                    // 건물 이동중

        private void OnEnable()
        {
            mainCamera.orthographicSize = 12f;
            clickedBuilding = null;
            player.SetActive(false);
            NPC.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            DetectTouch();
            DetectClick();
        }

        void GetBuilding()
        {
            
        }

        void UpdatePosition()
        {
            foreach(GameObject building in Building)
            {
                
            }
        }
        
        void DetectTouch()    // 터치감지
        {
            if (Input.touchCount > 0)
            {
                tempTouch = Input.GetTouch(0);
                if (tempTouch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(0) == false)
                {
                    touchedPos = Camera.main.ScreenToWorldPoint(tempTouch.position);
                    RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);
                    if (hit.collider.tag == "Building"|| hit.collider.tag == "BP")
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
                if (hit)
                {
                    if (hit.collider.tag == "Building"|| hit.collider.tag == "BP")
                    {
                        clickedBuilding = hit.collider.gameObject;
                    }
                }
            }
        }

        public void Exit() // 타운관리모드 나가기
        {
            clickedBuilding = null;
            mainCamera.orthographicSize = 7f;
            player.SetActive(true);
            NPC.SetActive(true);
            UIManager.Instance.CloseMenu();
            UIManager.Instance.menuStack.Peek().gameObject.SetActive(true);
        }   

        void MoveBuilding()     //건물 위치 변경
        {
            if (Input.GetMouseButton(0) || tempTouch.phase == TouchPhase.Moved)
            {
                clickedBuilding.transform.position = new Vector3(
                    Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                    clickedBuilding.transform.position.y,
                    clickedBuilding.transform.position.z);
            }
        }   

        void RotateBuilding() { clickedBuilding.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX ? false : true; }   // 건물 회전

        void Build(GameObject obj) { clickedBuilding = Instantiate(obj, Camera.main.ScreenToWorldPoint(new Vector3(0f, -1.3f, 0f)), Quaternion.Euler(0, 0, 0)); }   // 건물 생성    
    }
}
