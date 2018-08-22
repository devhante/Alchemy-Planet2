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

        private Dictionary<string, int> ownBuildings = new Dictionary<string, int>();                   // 소유중인 건물
        private Dictionary<GameObject, string> setupBuildings = new Dictionary<GameObject, string>();          // 설치된 건물
        private int page;                                               // 현재 건물이미지 페이지

        private void OnEnable()
        {
            leftButton.onClick.AddListener(() => { page -= page > 0 ? 1 : 0; });
            rightButton.onClick.AddListener(() => { page += ownBuildings.Count > (page + 1) * 6 ? 1 : 0; });
            
            for(int i=0; i<6; i++)
                buildingImages[i].GetComponent<Button>().onClick.AddListener(() => OnBuildingInfo(buildingImages[i].name));

            GetOwnBuilding();
            SetImage();
        }

        void OnBuildingInfo(string str)
        {

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

            foreach (string str in ownBuildings.Keys)
            {
                ownBuildingsImages.Add(str);
            }
            for (int i = 0; i < 6; i++)
            {
                if (i < ownBuildings.Count - page * 6)
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
    }
}
