using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.TownScene
{
    public class TownUpgrade : Common.UI<TownUpgrade>
    {
        public Button leftButton;
        public Button rightButton;
        public Button closeButton;
        public List<GameObject> buildingImages;

        private List<string> ownBuildings = new List<string>();                   // 소유중인 건물
        private int page;                                                         // 현재 건물이미지 페이지

        private void OnEnable()
        {
            GetComponent<CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            leftButton.onClick.AddListener(() => { page -= page > 0 ? 1 : 0; });
            rightButton.onClick.AddListener(() => { page += ownBuildings.Count > (page + 1) * 6 ? 1 : 0; });
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
        }

        void GetOwnBuilding()   // 소유중인 건물 받아오기
        {
            foreach (string str in DataManager.Instance.CurrentPlayerData.setupBuildings.Values)
            {
                Building b = DataManager.Instance.structures[str] as Building;
                if(b != null)
                    ownBuildings.Add(str);
            }
            foreach (string str in DataManager.Instance.CurrentPlayerData.ownBuildings.Keys)
            {
                Building b = DataManager.Instance.structures[str] as Building;
                if (b != null)
                    ownBuildings.Add(str);
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
                    buildingImages[i].GetComponent<Image>().sprite = DataManager.Instance.structures[ownBuildings[i]].image;
                    buildingImages[i].name = DataManager.Instance.structures[ownBuildings[i]].structureName;
                }
                else
                {
                    buildingImages[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
