using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class TileManager : MonoBehaviour
    {
        public static TileManager Instance { get; private set; }

        public float TileSpeed { get; set; }

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
            TileSpeed = 2;
            startPoint = new Vector3(0, -0.1f, 0);
            endPoint = new Vector3(-0.9f, -0.1f, 0);
        }

        private void Update()
        {
            speed = TileSpeed * GameManager.Instance.MoveSpeed;
            tileMap.transform.position += Vector3.left * speed * Time.deltaTime;

            if (tileMap.transform.position.x <= endPoint.x)
            {
                GameManager.Instance.Position++;
                tileMap.transform.position = startPoint;
            }
        }
    }
}
