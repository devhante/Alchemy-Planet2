using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.TownScene
{
    public class BuildingPlacement : Common.UI<BuildingPlacement>
    {
        public List<GameObject> buildingImages; // 건물 미리보기
        public Button leftButton;               // 건물이미지 페이지 왼쪽으로 넘기기
        public Button rightButton;              // 건물이미지 페이지 오른쪽으로 넘기기
        public Button rotateButton;             // 건물 회전 버튼
        public Button removeButton;             // 건물 보관 버튼
        public Button exitButton;               // 타운관리 나가기 버튼


        private List<Structure> ownBuildings = new List<Structure>();                 // 소유중인 건물
        private List<GameObject> setupBuildings = new List<GameObject>();       // 설치된 건물
        private GameObject clickedBuilding;                                     // 선택된 건물
        private Touch tempTouch;                                                // 터치들
        private Vector3 touchedPos;                                             // 터치위치
        private int page;                                                       // 현재 건물이미지 페이지
        private float touchTime;                                                // 터치시간


        private void OnEnable()
        {
            // 버튼 기능 적용
            leftButton.onClick.AddListener(() => { ChangePage(false); });
            rightButton.onClick.AddListener(() => { ChangePage(true); });
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
            GetBuilding();
            clickedBuilding = null;
            SetImage();
            touchTime = 0;
        }

        void Update()
        {
            DetectTouch();
            MoveCamera();
            CheckCollision();
        }

        void GetBuilding()   // 소유중인 건물 받아오기
        {
            foreach (Structure strc in DataManager.Instance.CurrentPlayerData.structures)
            {
                if (!strc.setup)
                    ownBuildings.Add(strc);
            }
            setupBuildings = DataManager.Instance.CurrentPlayerData.setupBuildilngs;
        }

        void SetBuilding()
        {
            foreach (Structure strc in DataManager.Instance.CurrentPlayerData.structures)
                strc.setup = false;

            foreach (GameObject obj in setupBuildings)
            {
                foreach (Structure strc in DataManager.Instance.CurrentPlayerData.structures)
                {
                    if (strc.id == int.Parse(obj.name.Substring(0, obj.name.Length - 7)))
                    {
                        strc.flip = obj.GetComponent<SpriteRenderer>().flipX;
                        strc.position = obj.transform.position;
                        strc.setup = true;
                        break;
                    }
                }
            }

            DataManager.Instance.CurrentPlayerData.setupBuildilngs = setupBuildings;
        }

        void SetImage() // 소유중인 건물이미지 출력하기
        {
            for (int i = 0; i < 5; i++)
            {
                if (i < ownBuildings.Count - page * 5)
                {
                    if (!buildingImages[i].activeSelf)
                        buildingImages[i].SetActive(true);
                    buildingImages[i].GetComponent<Image>().sprite = ownBuildings[i + page * 5].image;
                    buildingImages[i].name = ownBuildings[i + page * 5].structureName;
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
                touchedPos = Camera.main.ScreenToWorldPoint(tempTouch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);
                if (tempTouch.phase == TouchPhase.Stationary)
                    touchTime += tempTouch.deltaTime;
                else if (tempTouch.phase != TouchPhase.Stationary)
                    touchTime = 0;

                // 건물 선택하기 0.7초 동안 눌러야 선택됨
                if (hit.collider != null && hit.collider.tag == "Building" &&
                    !EventSystem.current.IsPointerOverGameObject(tempTouch.fingerId) && touchTime > 0.7f && clickedBuilding == null)
                {
                    if (clickedBuilding != null)
                    {
                        clickedBuilding.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                        if (clickedBuilding.GetComponent<StructureCollision>().CheckCollision())
                            RemoveBuilding();
                        clickedBuilding = null;
                    }
                    clickedBuilding = hit.collider.gameObject;
                    StartCoroutine("MoveBuilding");
                }
                else if ((hit.collider == null || hit.collider != null && hit.collider.gameObject != clickedBuilding) && clickedBuilding != null &&
                    tempTouch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(tempTouch.fingerId))
                {
                    clickedBuilding.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                    if (clickedBuilding.GetComponent<StructureCollision>().CheckCollision())
                        RemoveBuilding();
                    clickedBuilding = null;
                }
                if (clickedBuilding != null && hit.collider != null && hit.collider.gameObject == clickedBuilding)
                    StartCoroutine("MoveBuilding");
            }
        }

        public void Exit() // 타운관리모드 나가기
        {
            if (clickedBuilding != null)
            {
                clickedBuilding.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                if (clickedBuilding.GetComponent<StructureCollision>().CheckCollision())
                    RemoveBuilding();
                clickedBuilding = null;
            }

            SetBuilding();
            TownUI.Instance.player.SetActive(true);
            UIManager.Instance.CloseMenu();
            UIManager.Instance.TownUIOn();
        }

        IEnumerator MoveBuilding()     //건물 위치 변경
        {
            while (TouchPhase.Ended != tempTouch.phase)
            {
                touchedPos = tempTouch.position;

                clickedBuilding.transform.position = new Vector3(
                    Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(touchedPos).x),
                    clickedBuilding.transform.position.y,
                    clickedBuilding.transform.position.z);

                yield return null;
            }
        }

        void MoveCamera()
        {
            if (clickedBuilding == null && tempTouch.phase == TouchPhase.Moved)
            {
                TownUI.Instance.mainCamera.transform.position += Vector3.left * tempTouch.deltaPosition.x / 2 * tempTouch.deltaTime;
                if (TownUI.Instance.mainCamera.transform.position.x > DataManager.Instance.CurrentPlayerData.boundary)
                    TownUI.Instance.mainCamera.transform.position = new Vector3(DataManager.Instance.CurrentPlayerData.boundary,
                        TownUI.Instance.mainCamera.transform.position.y, TownUI.Instance.mainCamera.transform.position.z);
                else if (TownUI.Instance.mainCamera.transform.position.x < -DataManager.Instance.CurrentPlayerData.boundary)
                    TownUI.Instance.mainCamera.transform.position = new Vector3(-DataManager.Instance.CurrentPlayerData.boundary,
                        TownUI.Instance.mainCamera.transform.position.y, TownUI.Instance.mainCamera.transform.position.z);
            }
        }

        void RotateBuilding()   // 건물 회전
        {
            if (clickedBuilding != null)
                clickedBuilding.GetComponent<SpriteRenderer>().flipX = clickedBuilding.GetComponent<SpriteRenderer>().flipX ? false : true;
        }

        void Build(string str)  // 건물 생성 
        {
            if (clickedBuilding != null)
            {
                clickedBuilding.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                if (clickedBuilding.GetComponent<StructureCollision>().CheckCollision())
                    RemoveBuilding();
                clickedBuilding = null;
            }

            foreach (Structure strc in DataManager.Instance.CurrentPlayerData.structures)
            {
                if (strc.structureName == str && !strc.setup)
                {
                    strc.Build();
                    clickedBuilding = Instantiate(strc.StructureObject, strc.position, Quaternion.Euler(0, 0, 0));
                    ownBuildings.Remove(strc);
                    break;
                }
            }
            clickedBuilding.transform.position = new Vector2(TownUI.Instance.mainCamera.transform.position.x, clickedBuilding.transform.position.y);

            setupBuildings.Add(clickedBuilding);
            SetImage();
            SetBuilding();
        }

        void RemoveBuilding()   // 건물 철거
        {
            foreach (Structure strc in DataManager.Instance.CurrentPlayerData.structures)
            {
                if (strc.id == int.Parse(clickedBuilding.name.Substring(0, clickedBuilding.name.Length - 7)))
                    ownBuildings.Add(strc);
            }
            setupBuildings.Remove(clickedBuilding);
            Destroy(clickedBuilding);
            clickedBuilding = null;
            SetImage();
            SetBuilding();
        }

        void CheckCollision()
        {
            if (clickedBuilding != null)
            {
                if (clickedBuilding.GetComponent<StructureCollision>().CheckCollision())
                    clickedBuilding.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                else
                    clickedBuilding.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
            }
        }

        void ChangePage(bool over)
        {
            if (over)
                page += ownBuildings.Count > (page + 1) * 5 ? 1 : 0;
            else
                page -= page > 0 ? 1 : 0;
            SetImage();
            Debug.Log(page);
        }
    }
}
