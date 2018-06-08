using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class RecipeManager : MonoBehaviour
    {
        GameObject[] objects;
        Queue<Recipe> queue = new Queue<Recipe>();

        public static RecipeManager Instance { get; private set; }

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
            StartCoroutine("CreateRecipe");
        }

        public string GetQueuePeekName()
        {
            return queue.Peek().recipeName;
        }

        public void DestroyQueuePeek()
        {
            GameUI.Instance.UpdateGage(Gages.PURIFY, 5);
            Destroy(queue.Dequeue().gameObject);

            int index = 0;
            foreach (var item in queue)
            {
                item.SetDestination(index);
                index++;
            }
        }

        private void AddRecipe()
        {
            int index = Random.Range(0, PrefabManager.Instance.recipePrefabs.Length);
            Recipe temp = Instantiate(PrefabManager.Instance.recipePrefabs[index], transform).GetComponent<Recipe>();
            queue.Enqueue(temp);
            temp.SetDestination(queue.Count - 1);
        }

        IEnumerator CreateRecipe()
        {
            while (true)
            {
                if (queue.Count < 14) AddRecipe();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}