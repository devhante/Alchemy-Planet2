using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.TownScene
{
    public class TownManager : Common.UI
    {
        public List<GameObject> buildingImages;  // 건물 미리보기
        public Button leftButton;           // 건물이미지 페이지 왼쪽으로 넘기기
        public Button rightButton;          // 건물이미지 페이지 오른쪽으로 넘기기
        public Button rotateButton;         // 건물 회전 버튼
        public Button removeButton;         // 건물 보관 버튼
        public Button exitButton;           // 타운관리 나가기 버튼


        private Dictionary<string, int> ownBuildings = new Dictionary<string, int>();                   // 소유중인 건물
        private Dictionary<GameObject, string> setupBuildings = new Dictionary<GameObject, string>();          // 설치된 건물
        private GameObject clickedBuilding;                             // 선택된 건물
        private Touch tempTouch;                                        // 터치들
        private Vector3 touchedPos;                                     // 터치위치
        private int page;                                               // 현재 건물이미지 페이지

        private void OnEnable()
        {
            // 버튼 기능 적용
            leftButton.onClick.AddListener(() =>{ page-=page>0?1:0; });
            rightButton.onClick.AddListener(() =>{ page+=ownBuildings.Count>(page+1)*5?1:0; });
            rotateButton.onClick.AddListener(() => { RotateBuilding(); });
            removeButton.onClick.AddListener(() => { RemoveBuilding(); });
            exitButton.onClick.AddListener(() => { Exit(); });
            
            buildingImages[0].GetComponent<Button>().onClick.AddListener(() => { Build(buildingImages[0].name); });
            buildingImages[1].GetComponent<Button>().onClick.AddListener(() => { Build(buildingImages[1].name); });
            buildingImages[2].GetComponent<Button>().onClick.AddListener(() => { Build(buildingImages[2].name); });
            buildingImages[3].GetComponent<Button>().onClick.AddListener(() => { Build(buildingImages[3].name); });
            buildingImages[4].GetComponent<Button>().onClick.AddListener(() => { Build(buildingImages[4].name); });

            TownUI.Instance.player.SetActive(false);
            page = 0;
            GetOwnBuilding();
            clickedBuilding = null;
            SetImage();
        }

        void Update()
        {
            DetectTouch();
            MoveCamera();
        }

        void GetOwnBuilding()   // 소유중인 건물 받아오기
        {
            setupBuildings = DataManager.Instance.CurrentPlayerData.setupBuildings;
            ownBuildings = DataManager.Instance.CurrentPlayerData.ownBuildings;
        }

        void SetOwnBuilding()   // 소유중인 건물 적용하기
        {
            DataManager.Instance.CurrentPlayerData.setupBuildings = setupBuildings;
            DataManager.Instance.CurrentPlayerData.ownBuildings = ownBuildings;
        }

        void SetImage() // 소유중인 건물이미지 출력하기
        {
            List<string> ownBuildingsImages = new List<string>();

            foreach(string str in ownBuildings.Keys)
            {
                ownBuildingsImages.Add(str);
                Debug.Log(str);
            }
            for (int i = 0; i < 5; i++)
            {
                if(i<ownBuildings.Count - page * 5)
                {
                    if (!buildingImages[i].activeSelf)
                        buildingImages[i].SetActive(true);
                    buildingImages[i].GetComponent<Image>().sprite = DataManager.Instance.buildings[ownBuildingsImages[i]].image;
                    buildingImages[i].name = DataManager.Instance.buildings[ownBuildingsImages[i]].buildingName;
                }
                else
                {
                    buildingImages[i].gameObject.SetActive(false);
                }
            }
        }

        void DetectTouch()    // 클릭감지
        {
            if (Input.touchCount > 0)
            {
                tempTouch = Input.GetTouch(0);
                if (tempTouch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(0))
                {
                    touchedPos = Camera.main.ScreenToWorldPoint(tempTouch.position);
                    RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);
                    if (hit.collider != null && hit.collider.tag == "Building")
                    {
                        if (clickedBuilding != null)
                        {
                            clickedBuilding.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                            clickedBuilding = null;
                        }
                        clickedBuilding = hit.collider.gameObject;
                        clickedBuilding.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
                        StartCoroutine("MoveBuilding");
                    }
                    else if (hit.collider == null && clickedBuilding != null)
                    {
                        clickedBuilding.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                        clickedBuilding = null;
                    }
                }
            }
            
        }

        public void Exit() // 타운관리모드 나가기
        {
            if (clickedBuilding != null)
            {
                clickedBuilding.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                clickedBuilding = null;
            }
            TownUI.Instance.player.SetActive(true);
            UIManager.Instance.CloseMenu();
            UIManager.Instance.menuStack.Peek().gameObject.SetActive(true);
        }   

        IEnumerator MoveBuilding()     //건물 위치 변경
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
                yield return null;
            }
        }   

        void MoveCamera()
        {
            if(clickedBuilding == null && tempTouch.phase == TouchPhase.Moved)
            {
                TownUI.Instance.mainCamera.transform.position += Vector3.left * tempTouch.deltaPosition.x/2 * tempTouch.deltaTime;
            }
        }

        void RotateBuilding()   // 건물 회전
        {
            if(clickedBuilding != null)
                clickedBuilding.GetComponent<SpriteRenderer>().flipX = clickedBuilding.GetComponent<SpriteRenderer>().flipX ? false : true;
        }

        void Build(string str)  // 건물 생성 
        {
            if (clickedBuilding != null)
            {
                clickedBuilding.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                clickedBuilding = null;
            }
            clickedBuilding = Instantiate(DataManager.Instance.buildings[str].buildingObject);
            clickedBuilding.transform.position = new Vector3(TownUI.Instance.mainCamera.transform.position.x, clickedBuilding.transform.position.y);
            setupBuildings.Add(clickedBuilding,str);
            ownBuildings[str]--;
            SetOwnBuilding();
            SetImage();
        }

        void RemoveBuilding()   // 건물 철거
        {
            if (ownBuildings.ContainsKey(setupBuildings[clickedBuilding]))
            {
                ownBuildings[setupBuildings[clickedBuilding]]++;
            }
            else
            {
                ownBuildings.Add(setupBuildings[clickedBuilding], 1);
            }
            setupBuildings.Remove(clickedBuilding);
            Destroy(clickedBuilding);
            clickedBuilding = null;
            SetImage();
            SetOwnBuilding();
        }
    }
}
