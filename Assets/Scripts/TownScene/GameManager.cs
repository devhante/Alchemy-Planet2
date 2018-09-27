using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.Data
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public GameObject pollutedForest1;  // 타운의 경계 오염된숲1
        public GameObject pollutedForest2;  // 타운의 경계 오염된숲2
        public GameObject floor;            // 타운 바닥

        private int boundary;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
        }

        // Use this for initialization
        void Start()
        {
            SetFloor();
        }

        private void Update()
        {
            CheckUpgradeTime();
        }

        public void SetFloor()
        {
            SetBoundary();
            float floorX =-boundary;
            while (floorX <= boundary)
            {
                Instantiate(floor, new Vector2(floorX, -1), Quaternion.Euler(0, 0, 0));
                floorX += 0.9f;
            }
            
        }

        void SetBoundary()
        {
            boundary = DataManager.Instance.CurrentPlayerData.boundary;
            Building strc;
            if (DataManager.Instance.CurrentPlayerData.buildings.Find(a => a.buildingName == "House") != null)
            {
                strc = DataManager.Instance.CurrentPlayerData.buildings.Find(a => a.buildingName == "House");
                if (strc != null && strc.setup)
                {
                    boundary += (strc as Building).buildingLevel * 5;
                }
            }
            pollutedForest1.transform.position = new Vector3(-boundary, pollutedForest1.transform.position.y);
            pollutedForest2.transform.position = new Vector3(boundary, pollutedForest1.transform.position.y);
        }

        void CheckUpgradeTime()
        {
            if (DataManager.Instance.CurrentPlayerData != null)
            {
                foreach (Building strc in DataManager.Instance.CurrentPlayerData.buildings)
                {
                    if (strc.upgrading)
                    {
                        if (strc.UpgradeEndTime - System.DateTime.Now.ToBinary() < 0)
                        {
                            strc.UpgradeEnd();
                        }
                    }
                }
            }
        }
    }
}
