using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager Instance { get; set; }

        public Transform grid;
        public StageInfo[] stageInfos;

        public float BackgroundSpeed { get; set; }
        public float TileSpeed { get; set; }

        private readonly float frame = 0.02f;
        private int stageLength = 2;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            //CreateJsonFile();
        }

        private void Start()
        {
            StartCoroutine("MoveBackgroundCoroutine");
            StartCoroutine("MoveTileCoroutine");
        }

        private void CreateJsonFile()
        {
            using (StreamWriter file = File.CreateText(string.Format("{0}/{1}.json", "Assets/Resources/Datas/", "StageInfo")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, stageInfos);
            }
        }

        IEnumerator MoveBackgroundCoroutine()
        {
            BackgroundSpeed = 0.5f;
            int backgroundStageIndex = 0;
            int backgroundCount = 0;
            float length = 21.6f;
            Vector3 startPoint = new Vector3(0.45f, 0.25f, 0);
            Queue<GameObject> backgroundQueue = new Queue<GameObject>();

            backgroundQueue.Enqueue(Instantiate(stageInfos[backgroundStageIndex].backgroundPrefab, startPoint, Quaternion.identity, grid));
            backgroundQueue.Enqueue(Instantiate(stageInfos[backgroundStageIndex].backgroundPrefab, startPoint + new Vector3(length, 0, 0), Quaternion.identity, grid));
            backgroundCount += 2;

            while(true)
            {
                float speed = BackgroundSpeed * GameManager.Instance.MoveSpeed;

                foreach (var item in backgroundQueue)
                    item.transform.position += Vector3.left * speed * frame;

                if (backgroundQueue.Peek().transform.position.x <= startPoint.x - length)
                {
                    if(backgroundCount == stageLength && backgroundStageIndex < stageInfos.Length - 1)
                    {
                        backgroundStageIndex++;
                        backgroundCount = 0;
                    }

                    Destroy(backgroundQueue.Dequeue());
                    backgroundQueue.Peek().transform.position = startPoint;
                    backgroundQueue.Enqueue(Instantiate(stageInfos[backgroundStageIndex].backgroundPrefab, startPoint + new Vector3(length, 0, 0), Quaternion.identity, grid));
                    backgroundCount++;
                }

                yield return new WaitForSeconds(frame);
            }
        }

        IEnumerator MoveTileCoroutine()
        {
            TileSpeed = 2;
            int tileStageIndex = 0;
            int tileCount = 0;
            float length = 21.6f;
            Vector3 startPoint = new Vector3(0, -0.1f, 0);
            Queue<GameObject> tileQueue = new Queue<GameObject>();

            tileQueue.Enqueue(Instantiate(stageInfos[tileStageIndex].tilePrefab, startPoint, Quaternion.identity, grid));
            tileQueue.Enqueue(Instantiate(stageInfos[tileStageIndex].tilePrefab, startPoint + new Vector3(length, 0, 0), Quaternion.identity, grid));
            tileCount += 2;

            while(true)
            {
                float speed = TileSpeed * GameManager.Instance.MoveSpeed;

                foreach (var item in tileQueue)
                    item.transform.position += Vector3.left * speed * frame;

                if(tileQueue.Peek().transform.position.x <= startPoint.x - length)
                {
                    if (tileCount == (int)(stageLength * TileSpeed / BackgroundSpeed) && tileStageIndex < stageInfos.Length - 1)
                    {
                        tileStageIndex++;
                        tileCount = 0;
                    }

                    Destroy(tileQueue.Dequeue());
                    tileQueue.Peek().transform.position = startPoint;
                    tileQueue.Enqueue(Instantiate(stageInfos[tileStageIndex].tilePrefab, startPoint + new Vector3(length, 0, 0), Quaternion.identity, grid));
                    tileCount++;
                }

                yield return new WaitForSeconds(frame);
            }
        }
    }
}
