using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class BackgroundManager : MonoBehaviour
    {
        public static BackgroundManager Instance { get; private set; }

        public float BackgroundSpeed;
        public int BackgroundNumber;
        public float BackgroundWidth;

        private Queue<GameObject> backgroundQueue;
        private GameObject backgroundParent;
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
            BackgroundSpeed = 0.5f;
            BackgroundNumber = 2;
            BackgroundWidth = 7.2f;
            backgroundQueue = new Queue<GameObject>();
            backgroundParent = new GameObject("Backgrounds");
            startPoint = new Vector3(7.2f, 3.84f, 0);
            endPoint = new Vector3(-7.2f, 3.84f, 0);

            InitBackgrounds();
        }

        private void Update()
        {
            foreach (var item in backgroundQueue)
                item.transform.position += Vector3.left * BackgroundSpeed * Time.deltaTime;

            if(backgroundQueue.Peek().transform.position.x <= endPoint.x)
            {
                backgroundQueue.Peek().transform.position = startPoint;
                backgroundQueue.Enqueue(backgroundQueue.Dequeue());
            }
        }

        private void InitBackgrounds()
        {
            for (int i = BackgroundNumber; i > 0; i--)
                AddBackground(startPoint + Vector3.left * BackgroundWidth * i);
        }

        private void AddBackground(Vector3 offset)
        {
            backgroundQueue.Enqueue(Instantiate(PrefabManager.Instance.backgroundPrefab, offset, Quaternion.identity, backgroundParent.transform)); 
        }
    }
}