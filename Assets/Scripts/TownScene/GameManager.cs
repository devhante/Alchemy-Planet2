using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.Data
{
    public class GameManager : MonoBehaviour
    {
        public GameObject pollutedForest1;  // 타운의 경계 오염된숲1
        public GameObject pollutedForest2;  // 타운의 경계 오염된숲2
        public GameObject floor;            // 타운 바닥

        private int boundary;

        // Use this for initialization
        void Start()
        {
            SetFloor();
        }

        private void Update()
        {
            CheckUpgradeTime();
        }

        void SetFloor()
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
            pollutedForest1.transform.position = new Vector3(-boundary, pollutedForest1.transform.position.y);
            pollutedForest2.transform.position = new Vector3(boundary, pollutedForest1.transform.position.y);
        }

        void CheckUpgradeTime()
        {
            System.TimeSpan timeDiff;
            foreach (Structure strc in DataManager.Instance.CurrentPlayerData.structures)
            {
                if (strc is Building && (strc as Building).upgrading)
                {
                    timeDiff = (strc as Building).UpgradeEndTime - System.DateTime.Now;

                    if (timeDiff.TotalSeconds < 0)
                    {
                        (strc as Building).UpgradeEnd();
                    }
                }
            }
        }
    }
}
