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
            SetBoundary();
            SetFloor();
        }

        void SetFloor()
        {
            float floorX=-boundary;
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
    }
}
