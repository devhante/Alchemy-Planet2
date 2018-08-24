using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.TownScene
{
    public class TownUpgrade : Common.UI
    {
        public Button leftButton;
        public Button rightButton;
        public List<GameObject> buildingImages;
        public GameObject backgroundImage;

        private List<string> ownBuildings = new List<string>();                   // 소유중인 건물
        private int page;                                               // 현재 건물이미지 페이지

        private void OnEnable()
        {
            GetComponent<CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            leftButton.onClick.AddListener(() => { page -= page > 0 ? 1 : 0; });
            rightButton.onClick.AddListener(() => { page += ownBuildings.Count > (page + 1) * 6 ? 1 : 0; });
            
            for(int i=0; i<6; i++)
                buildingImages[i].GetComponent<Button>().onClick.AddListener(() => OnBuildingInfo(buildingImages[i].name));

            GetOwnBuilding();
            SetImage();
        }

        void OnBuildingInfo(string str)
        {
            backgroundImage.SetActive(true);
            
        }

        void GetOwnBuilding()   // 소유중인 건물 받아오기
        {
            foreach (GameObject obj in DataManager.Instance.CurrentPlayerData.setupBuildings.Keys)
            {
                if (DataManager.Instance.structures[DataManager.Instance.CurrentPlayerData.setupBuildings[obj]].GetType() == typeof(Building))
                    ownBuildings.Add(DataManager.Instance.CurrentPlayerData.setupBuildings[obj]);
            }
            foreach (string str in DataManager.Instance.CurrentPlayerData.ownBuildings.Keys)
            {
                if (DataManager.Instance.structures[str].GetType() == typeof(Building))
                    ownBuildings.Add(str);
            }
        }

        void SetImage() // 소유중인 건물이미지 출력하기
        {
            List<string> ownBuildingsImages = new List<string>();

            foreach (string str in ownBuildings)
            {
                ownBuildingsImages.Add(str);
            }
            for (int i = 0; i < 6; i++)
            {
                if (i < ownBuildings.Count - page * 6)
                {
                    if (!buildingImages[i].activeSelf)
                        buildingImages[i].SetActive(true);
                    buildingImages[i].GetComponent<Image>().sprite = DataManager.Instance.structures[ownBuildingsImages[i]].image;
                    buildingImages[i].name = DataManager.Instance.structures[ownBuildingsImages[i]].structureName;
                }
                else
                {
                    buildingImages[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
