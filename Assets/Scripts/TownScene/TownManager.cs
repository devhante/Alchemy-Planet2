using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.TownScene
{
    public class TownManager : Common.UI
    {
        public List<Image> buildingImages;  // 건물 미리보기
        public Button leftButton;           // 건물이미지 페이지 왼쪽으로 넘기기
        public Button rightButton;          // 건물이미지 페이지 오른쪽으로 넘기기
        public Button rotateButton;         // 건물 회전 버튼
        public Button removeButton;         // 건물 보관 버튼
        public Button exitButton;           // 타운관리 나가기 버튼
        public GameObject BuildingObject;   // 건물이 될 프리팹

        private Dictionary<string, int> ownBuildings;                   // 소유중인 건물
        private Dictionary<GameObject, string> setupBuildings;           // 설치된 건물
        private GameObject clickedBuilding;                             // 선택된 건물
        private Touch tempTouch;                                        // 터치들
        private Vector3 touchedPos;                                     // 터치위치
        private bool moving;                                            // 건물 이동중
        private int page;                                               // 현재 건물이미지 페이지

        private void OnEnable()
        {
            page = 0;
            GetOwnBuilding();
            clickedBuilding = null;
            SetImage();

            leftButton.onClick.AddListener(() =>{ page-=page>0?1:0; });
            rightButton.onClick.AddListener(() =>{ page+=ownBuildings.Count>(page+1)*5?1:0; });
            rotateButton.onClick.AddListener(() => { RotateBuilding(); });
            removeButton.onClick.AddListener(() => { RemoveBuilding(); });
            exitButton.onClick.AddListener(() => { Exit(); });
        
        }

        void Update()
        {
            DetectClick();
        }

        void GetOwnBuilding() { ownBuildings = DataManager.Instance.CurrentPlayerData.ownBuildings; }   // 소유중인 건물 받아오기
        void SetOwnBuilding() { DataManager.Instance.CurrentPlayerData.ownBuildings = ownBuildings; }   // 소유중인 건물 변경하기

        void SetImage() // 소유중인 건물이미지 출력하기
        {
            List<string> ownBuildingsImages = new List<string>();

            foreach(string str in ownBuildings.Keys)
            {
                ownBuildingsImages.Add(str);
            }

            for (int i = 0; i < 5; i++)
            {
                if(i<ownBuildings.Count - page * 5)
                {
                    buildingImages[i].sprite = DataManager.Instance.buildings[ownBuildingsImages[i]].image;
                    buildingImages[i].tag = "BP";
                    buildingImages[i].name = ownBuildingsImages[i];
                }
                else
                {
                    buildingImages[i].gameObject.SetActive(false);
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
                    if (hit.collider.tag == "Building")
                    {
                        clickedBuilding = hit.collider.gameObject;
                    }
                    else if (hit.collider.tag == "BP")
                    {
                        Build(hit.collider.gameObject.name);
                    }
                }
            }
            if (Input.touchCount > 0)
            {
                tempTouch = Input.GetTouch(0);
                if (tempTouch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(0) == false)
                {
                    touchedPos = Camera.main.ScreenToWorldPoint(tempTouch.position);
                    RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);
                    if (hit.collider.tag == "Building")
                    {
                        clickedBuilding = hit.collider.gameObject;
                    }
                    else if (hit.collider.tag == "BP")
                    {
                        Build(hit.collider.gameObject.name);
                    }
                }
            }
        }

        public void Exit() // 타운관리모드 나가기
        {
            clickedBuilding = null;
            UIManager.Instance.CloseMenu();
            UIManager.Instance.menuStack.Peek().gameObject.SetActive(true);
        }   

        void MoveBuilding()     //건물 위치 변경
        {
            while (TouchPhase.Ended != Input.GetTouch(0).phase)
            {
                if (Input.GetMouseButton(0))
                {
                    clickedBuilding.transform.position = new Vector3(
                        Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                        clickedBuilding.transform.position.y,
                        clickedBuilding.transform.position.z);
                }
                else if (tempTouch.phase == TouchPhase.Moved)
                {
                    touchedPos = Input.GetTouch(0).position;
                    clickedBuilding.transform.position = new Vector3(
                        Camera.main.ScreenToWorldPoint(touchedPos).x,
                        clickedBuilding.transform.position.y,
                        clickedBuilding.transform.position.z);
                }
            }
        }   

        void RotateBuilding() { clickedBuilding.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX ? false : true; }   // 건물 회전

        void Build(string str)  // 건물 생성 
        {
            BuildingObject.GetComponent<SpriteRenderer>().sprite = DataManager.Instance.buildings[str].image;
            clickedBuilding = Instantiate(BuildingObject, Camera.main.ScreenToWorldPoint(new Vector3(0f, -1.3f, 0f)), Quaternion.Euler(0, 0, 0));
            setupBuildings.Add(clickedBuilding,str);
            ownBuildings[str]--;
            SetOwnBuilding();
        }

        void RemoveBuilding()   // 건물 철거
        {
            ownBuildings[setupBuildings[clickedBuilding]]++;
            setupBuildings.Remove(clickedBuilding);
            clickedBuilding = null;
            SetOwnBuilding();
        }
    }
}
