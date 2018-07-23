using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class TileManager : MonoBehaviour
    {
        public static TileManager Instance { get; private set; }

        public float TileSpeed { get; set; }
        public int TileNumber { get; private set; }
        public float TileWidth { get; private set; }

        private Queue<GameObject> tileQueue;
        private GameObject tileParent;
        private Vector3 startPoint;
        private Vector3 endPoint;

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
            TileNumber = 11;
            TileWidth = 1f;
            tileQueue = new Queue<GameObject>();
            tileParent = new GameObject("Tiles");
            startPoint = new Vector3(6.5f, 0.96f, 0);
            endPoint = new Vector3(-4.5f, 0.96f, 0);

            InitTiles();
        }

        private void Update()
        {
            foreach (var item in tileQueue)
                item.transform.position += Vector3.left * TileSpeed * Time.deltaTime;

            if (tileQueue.Peek().transform.position.x <= endPoint.x)
            {
                tileQueue.Peek().transform.position = startPoint;
                tileQueue.Enqueue(tileQueue.Dequeue());
            }
        }

        private void InitTiles()
        {
            for (int i = TileNumber; i > 0; i--)
                AddTile(startPoint + Vector3.left * TileWidth * i);
        }

        private void AddTile(Vector3 offset)
        {
            tileQueue.Enqueue(Instantiate(PrefabManager.Instance.tilePrefab, offset, Quaternion.identity, tileParent.transform));
        }
    }
}
