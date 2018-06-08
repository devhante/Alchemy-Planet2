using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class BackgroundManager : MonoBehaviour
    {
        public int TileSpeed { get; set; }
        public int TileNumber { get; set; }
        public float TileWidth { get; set; }
        public Vector3 TileOffset { get; set; }

        Queue<GameObject> tileQueue = new Queue<GameObject>();
        GameObject tileParent;

        private void Start()
        {
            TileSpeed = 1;
            TileNumber = 12;
            TileWidth = 1;
            TileOffset = new Vector3(6, 1.8f);
            tileParent = new GameObject("Tiles");

            InitTiles();
        }

        private void Update()
        {
            foreach (GameObject item in tileQueue)
            {
                item.transform.position += new Vector3(-1 * TileSpeed * Time.deltaTime, 0.0f, 0.0f);
            }

            if (tileQueue.Peek().transform.position.x <= -6)
            {
                Destroy(tileQueue.Dequeue());
                AddTile(TileOffset);
            }
        }

        void InitTiles()
        {
            for(int i = TileNumber; i > 0; i--)
            {
                AddTile(new Vector3(TileOffset.x - i * TileWidth, TileOffset.y));
            }
        }

        void AddTile(Vector3 offset)
        {
            tileQueue.Enqueue(Instantiate(PrefabManager.Instance.tilePrefab, offset, Quaternion.identity, tileParent.transform));
        }
    }
}
