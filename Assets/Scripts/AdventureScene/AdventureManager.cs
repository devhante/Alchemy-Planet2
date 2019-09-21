using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.AdvectureScene
{
    public class AdventureManager : MonoBehaviour
    {
        public static AdventureManager Instance { get; private set; }

        public int planetCount;
        public float animationTime;

        [HideInInspector] public int planetNumber;
        [HideInInspector] public bool isMoving;

        private void OnDestroy() { Instance = null; }

        private void Awake()
        {
            Instance = this;
            planetNumber = 0;
            isMoving = false;
        }


        private void Update()
        {
            if (isMoving == false)
            {
                if(Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    isMoving = true;
                    planetNumber--;
                    UIManager.Instance.SendMessage("MoveLeft");
                }
                else if(Input.GetKeyDown(KeyCode.RightArrow))
                {
                    isMoving = true;
                    planetNumber++;
                    UIManager.Instance.SendMessage("MoveRight");
                }
            }
        }
    }
}