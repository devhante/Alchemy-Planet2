using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class RecipeManager : MonoBehaviour
    {
        public List<string> RecipeNameList { get; private set; }
        public Queue<Recipe> recipeQueue;
        public int recipeNumber = 14;

        public static RecipeManager Instance { get; private set; }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);

            RecipeNameList = new List<string>();
            recipeQueue = new Queue<Recipe>();
        }

        private void Start()
        {
            StartCoroutine("CreateRecipe");
        }

        IEnumerator CreateRecipe()
        {
            while (true)
            {
                if (recipeQueue.Count < recipeNumber) AddRecipe();
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void AddRecipe()
        {
            int index = Random.Range(0, PrefabManager.Instance.recipePrefabs.Length);
            Recipe temp = Instantiate(PrefabManager.Instance.recipePrefabs[index], transform).GetComponent<Recipe>();
            recipeQueue.Enqueue(temp);
            temp.SetDestination(recipeQueue.Count - 1);
        }

        public string GetQueuePeekName()
        {
            return recipeQueue.Peek().recipeName;
        }

        public void DestroyQueuePeek()
        {
            GameUI.Instance.UpdateGage(Gages.PURIFY, 5);
            Destroy(recipeQueue.Dequeue().gameObject);

            int index = 0;
            foreach (var item in recipeQueue)
            {
                item.SetDestination(index);
                index++;
            }
        }

        public void UpdateRecipeNameList()
        {
            RecipeNameList.Clear();

            foreach (var item in recipeQueue)
                RecipeNameList.Add(item.recipeName);
        }
    }
}