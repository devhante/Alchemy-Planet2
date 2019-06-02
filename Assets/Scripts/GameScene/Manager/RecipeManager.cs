using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public class RecipeManager : MonoBehaviour
    {
        public static RecipeManager Instance { get; private set; }

        public List<MaterialName> RecipeNameList { get; private set; }
        public Queue<Recipe> recipeQueue;
        public int recipeNumber = 14;

        public RectTransform startPoint;
        public RectTransform endPoint;

        public int HighlightedRecipeCount { get; set; }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            RecipeNameList = new List<MaterialName>();
            recipeQueue = new Queue<Recipe>();
            HighlightedRecipeCount = 0;
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
            UpdateRecipeNameList();
        }

        public MaterialName GetQueuePeekName()
        {
            return recipeQueue.Peek().recipeName;
        }

        public void DestroyQueuePeek()
        {
            Destroy(recipeQueue.Dequeue().gameObject);

            int index = 0;
            foreach (var item in recipeQueue)
            {
                item.SetDestination(index);
                index++;
            }
            UpdateRecipeNameList();
        }

        public void UpdateRecipeNameList()
        {
            RecipeNameList.Clear();

            foreach (var item in recipeQueue)
                RecipeNameList.Add(item.recipeName);
        }

        public void HighlightRecipe()
        {
            Recipe recipe = recipeQueue.ElementAt(HighlightedRecipeCount);
            HighlightedRecipeCount++;
            recipe.GetComponent<Image>().sprite = SpriteManager.Instance.GetHighlightedMaterialSprite(recipe.recipeName);
        }
    }
}