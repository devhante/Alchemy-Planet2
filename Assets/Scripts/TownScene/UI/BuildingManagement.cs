using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.TownScene
{
    public class BuildingManagement : Common.UI<BuildingManagement>
    {
        public Button leftButton;
        public Button rightButton;
        public Button closeButton;
        public List<GameObject> buildingImages;

        private List<Building> ownBuildings;                   // 소유중인 건물
        private int page;                                                         // 현재 건물이미지 페이지

        private void OnEnable()
        {
            GetComponent<CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            leftButton.onClick.AddListener(() => { ChangePage(false); });
            rightButton.onClick.AddListener(() => { ChangePage(true); });
            closeButton.onClick.AddListener(() => { UIManager.Instance.CloseMenu(); });

            buildingImages[0].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(0));
            buildingImages[1].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(1));
            buildingImages[2].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(2));
            buildingImages[3].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(3));
            buildingImages[4].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(4));
            buildingImages[5].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(5));

            GetOwnBuilding();
            SetImage();
        }

        void OpenBuildingInfo(int n)
        {
            buildingImages[n].GetComponent<BuildingInfo>().SetInfo(ownBuildings[n]);
            buildingImages[n].GetComponent<BuildingInfo>().OpenInfo();
        }

        public void GetOwnBuilding()   // 소유중인 건물 받아오기
        {
            ownBuildings = new List<Building>();
            List<string> buildingList = new List<string>();
            foreach (Structure strc in DataManager.Instance.structureInfo.Values)
            {
                if (strc is Building)
                {
                    ownBuildings.Add(strc as Building);
                    buildingList.Add(strc.structureName);
                }
            }
            foreach (Structure strc in DataManager.Instance.CurrentPlayerData.structures)
            {
                if (strc is Building)
                {
                    int a = ownBuildings.FindIndex(x => x.structureName.Contains(strc.structureName));
                    ownBuildings[a] = strc as Building;
                }
            }
        }

        void SetImage() // 소유중인 건물이미지 출력하기
        {
            for (int i = 0; i < 6; i++)
            {
                if (i < ownBuildings.Count - page * 6)
                {
                    if (!buildingImages[i].activeSelf)
                        buildingImages[i].SetActive(true);
                    buildingImages[i].name = ownBuildings[i + page * 6].structureName;
                    buildingImages[i].GetComponent<Image>().sprite = ownBuildings[i + page * 6].image;
                }
                else
                {
                    buildingImages[i].gameObject.SetActive(false);
                }
            }
        }

        void ChangePage(bool over)
        {
            if (over)
                page += ownBuildings.Count > (page + 1) * 6 ? 1 : 0;
            else
                page -= page > 0 ? 1 : 0;
            SetImage();
            Debug.Log(page);
        }
    }
}
