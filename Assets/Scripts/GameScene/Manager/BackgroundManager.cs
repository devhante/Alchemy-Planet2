using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class BackgroundManager : MonoBehaviour
    {
        public static BackgroundManager Instance { get; private set; }

        public float BackgroundSpeed { get; set; }

        public GameObject tileMap;
        private Vector3 startPoint;
        private Vector3 endPoint;
        private float speed;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        private void Start()
        {
            BackgroundSpeed = 0.5f;
            startPoint = new Vector3(0.45f, 0, 0);
            endPoint = new Vector3(-13.95f, 0, 0);
        }

        private void Update()
        {
            speed = BackgroundSpeed * GameManager.Instance.moveSpeed; 
            tileMap.transform.position += Vector3.left * speed * Time.deltaTime;

            if (tileMap.transform.position.x <= endPoint.x)
                tileMap.transform.position = startPoint;
        }
    }
}