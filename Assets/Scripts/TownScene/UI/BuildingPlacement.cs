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
        public Button buildingTap;              // 건물 탭
        public Button interiorTap;              // 인테리어 탭

        [SerializeField] private Sprite clickTap;       // 선택된 탭
        [SerializeField] private Sprite notClickTap;    // 선택되지 않은 탭

        private List<Building> ownBuildings = new List<Building>();                 // 소유중인 건물
        private List<Interior> ownInteriors = new List<Interior>();                 // 소유중인 인테리어
        private List<GameObject> setupBuildings = new List<GameObject>();       // 설치된 건물
        private GameObject clickedBuilding;                                     // 선택된 건물
        private Touch tempTouch;                                                // 터치들
        private Vector3 touchedPos;                                             // 터치위치
        private int page;                                                       // 현재 건물이미지 페이지
        private float touchTime;                                                // 터치시간
        private string tapType;                                                 // 선택된 탭


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

            buildingTap.onClick.AddListener(() => { SetBuildingImage(); page = 0; });
            interiorTap.onClick.AddListener(() => { SetInteriorImage(); page = 0; });


            TownUI.Instance.player.SetActive(false);
            page = 0;
            GetBuilding();
            clickedBuilding = null;
            SetBuildingImage();
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
            foreach (Building building in DataManager.Instance.CurrentPlayerData.buildings)
            {
                if (!building.setup)
                    ownBuildings.Add(building);
            }

            foreach (Interior interior in DataManager.Instance.CurrentPlayerData.interiors)
            {
                if (!interior.setup)
                    ownInteriors.Add(interior);
            }

            setupBuildings = DataManager.Instance.CurrentPlayerData.setupBuildilngs;
        }

        void SetBuilding()
        {
            foreach (Building building in DataManager.Instance.CurrentPlayerData.buildings)
                WebSocketManager.Instance.SendUpdateBuilding("", DataManager.Instance.CurrentPlayerData.player_id, building.id.ToString(), building.buildingName,
                    building.buildingLevel, building.position, building.setup, building.flip, building.upgrading, building.UpgradeEndTime);

            foreach (Interior interior in DataManager.Instance.CurrentPlayerData.interiors)
                WebSocketManager.Instance.SendUpdateInterior("", DataManager.Instance.CurrentPlayerData.player_id, interior.id.ToString(),
                    interior.interiorName, interior.position, interior.setup, interior.flip);

            foreach (GameObject obj in setupBuildings)
            {
                if (obj.name.StartsWith("1"))
                {
                    Building building = DataManager.Instance.CurrentPlayerData.buildings.Find(a => a.id == int.Parse(obj.name.Substring(0, obj.name.Length - 7)));
                    building.flip = obj.GetComponent<SpriteRenderer>().flipX;
                    building.position = obj.transform.position.x;
                    building.setup = true;

                    WebSocketManager.Instance.SendUpdateBuilding("", DataManager.Instance.CurrentPlayerData.player_id, building.id.ToString(), building.buildingName,
                        building.buildingLevel, building.position, building.setup, building.flip, building.upgrading, building.UpgradeEndTime);
                }
                else if (obj.name.StartsWith("2"))
                {
                    Interior interior = DataManager.Instance.CurrentPlayerData.interiors.Find(a => a.id == int.Parse(obj.name.Substring(0, obj.name.Length - 7)));
                    interior.flip = obj.GetComponent<SpriteRenderer>().flipX;
                    interior.position = obj.transform.position.x;
                    interior.setup = true;

                    WebSocketManager.Instance.SendUpdateInterior("", DataManager.Instance.CurrentPlayerData.player_id, interior.id.ToString(),
                        interior.interiorName, interior.position, interior.setup, interior.flip);
                }
            }

            DataManager.Instance.CurrentPlayerData.setupBuildilngs = setupBuildings;

            GameManager.Instance.SetFloor();
        }

        void SetBuildingImage() // 소유중인 건물이미지 출력하기
        {
            for (int i = 0; i < 5; i++)
            {
                if (i < ownBuildings.Count - page * 5)
                {
                    if (!buildingImages[i].activeSelf)
                        buildingImages[i].SetActive(true);
                    buildingImages[i].GetComponent<Image>().sprite = ownBuildings[i + page * 5].image;
                    buildingImages[i].name = ownBuildings[i + page * 5].buildingName;
                }
                else
                {
                    buildingImages[i].gameObject.SetActive(false);
                }
            }
            tapType = "Building";
            buildingTap.image.sprite = clickTap;
            interiorTap.image.sprite = notClickTap;
        }

        void SetInteriorImage() // 소유중인 건물이미지 출력하기
        {
            for (int i = 0; i < 5; i++)
            {
                if (i < ownInteriors.Count - page * 5)
                {
                    if (!buildingImages[i].activeSelf)
                        buildingImages[i].SetActive(true);
                    buildingImages[i].GetComponent<Image>().sprite = ownInteriors[i + page * 5].image;
                    buildingImages[i].name = ownInteriors[i + page * 5].interiorName;
                }
                else
                {
                    buildingImages[i].gameObject.SetActive(false);
                }
            }
            tapType = "Interior";
            interiorTap.image.sprite = clickTap;
            buildingTap.image.sprite = notClickTap;
        }

        void DetectTouch()    // 클릭감지
        {
            if (Input.touchCount > 0)
            {
                tempTouch = Input.GetTouch(0);
                touchedPos = Camera.main.ScreenToWorldPoint(tempTouch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);
                if (hit.collider != null && hit.collider.tag == "Building" 
                    && !EventSystem.current.IsPointerOverGameObject(tempTouch.fingerId))
                    touchTime += tempTouch.deltaTime;
                if (tempTouch.phase == TouchPhase.Ended || hit.collider == null
                    || (hit.collider != null && hit.collider.tag != "Building"))
                    touchTime = 0;

                // 건물 선택하기 0.7초 동안 눌러야 선택됨
                if (touchTime > 0.7f && clickedBuilding == null)
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
            foreach (GameObject obj in TownUI.Instance.npc)
            {
                obj.SetActive(true);
            }
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
            if (clickedBuilding != null)    // 클릭된 건물이 있으면 제거
            {
                clickedBuilding.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                if (clickedBuilding.GetComponent<StructureCollision>().CheckCollision())
                    RemoveBuilding();
                clickedBuilding = null;
            }

            if (tapType == "Building")
            {
                Building building = ownBuildings.Find(a => a.buildingName == str);
                building.Build();
                clickedBuilding = Instantiate(building.buildingObject, new Vector2(TownUI.Instance.mainCamera.transform.position.x, building.buildingObject.transform.position.y), Quaternion.Euler(0, 0, 0));

                ownBuildings.Remove(building);
                setupBuildings.Add(clickedBuilding);
                SetBuildingImage();
            }

            if (tapType == "Interior")
            {
                Interior interior = ownInteriors.Find(a => a.interiorName == str);
                interior.Build();
                clickedBuilding = Instantiate(interior.interiorObject, new Vector2(TownUI.Instance.mainCamera.transform.position.x, interior.interiorObject.transform.position.y), Quaternion.Euler(0, 0, 0));

                ownInteriors.Remove(interior);
                setupBuildings.Add(clickedBuilding);
                SetInteriorImage();
            }

        }

        void RemoveBuilding()   // 건물 철거
        {
            if (clickedBuilding.name.StartsWith("1"))
            {
                ownBuildings.Add(DataManager.Instance.CurrentPlayerData.buildings.Find(a => a.buildingName == clickedBuilding.name.Substring(0, clickedBuilding.name.Length - 7)));
                setupBuildings.Remove(clickedBuilding);
                Destroy(clickedBuilding);
            }
            if (clickedBuilding.name.StartsWith("2"))
            {
                ownInteriors.Add(DataManager.Instance.CurrentPlayerData.interiors.Find(a => a.interiorName == clickedBuilding.name.Substring(0, clickedBuilding.name.Length - 7)));
                setupBuildings.Remove(clickedBuilding);
                Destroy(clickedBuilding);
            }
            if (tapType == "Building")
                SetBuildingImage();
            else if (tapType == "Interior")
                SetInteriorImage();
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
            if (tapType == "Building")
            {
                if (over)
                    page += ownBuildings.Count > (page + 1) * 5 ? 1 : 0;
                else
                    page -= page > 0 ? 1 : 0;
                SetBuildingImage();
            }
            else if (tapType == "Interior")
            {
                if (over)
                    page += ownInteriors.Count > (page + 1) * 5 ? 1 : 0;
                else
                    page -= page > 0 ? 1 : 0;
                SetInteriorImage();
            }
        }
    }
}
