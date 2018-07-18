using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class BackgroundManager : MonoBehaviour
    {
        private int tileSpeed;
        private int tileNumber;
        private float tileWidth;
        private Vector3 tileOffset;

        Queue<GameObject> tileQueue = new Queue<GameObject>();
        GameObject tileParent;

        private void Start()
        {
            tileSpeed = 2;
            tileNumber = 12;
            tileWidth = 1.04f;
            tileOffset = new Vector3(6.24f, 0.96f);
            tileParent = new GameObject("Tiles");

            InitTiles();
        }

        private void FixedUpdate()
        {
            foreach (GameObject item in tileQueue)
            {
                item.transform.position += new Vector3(-1 * tileSpeed * Time.deltaTime, 0.0f, 0.0f);
            }

            if (tileQueue.Peek().transform.position.x <= -6.24f)
            {
                Destroy(tileQueue.Dequeue());
                AddTile(tileOffset);
            }
        }

        void InitTiles()
        {
            for(int i = tileNumber; i > 0; i--)
            {
                AddTile(new Vector3(tileOffset.x - i * tileWidth, tileOffset.y));
            }
        }

        void AddTile(Vector3 offset)
        {
            tileQueue.Enqueue(Instantiate(PrefabManager.Instance.tilePrefab, offset, Quaternion.identity, tileParent.transform));
        }
    }
}
