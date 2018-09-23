using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public class MaterialManager : MonoBehaviour
    {
        public static MaterialManager Instance { get; private set; }

        public int ChainedNumber { get; set; }

        public Dictionary<MaterialName, int> MaterialNumbers { get; private set; }
        public List<GameObject> Objects { get; private set; }
        public List<Material> MaterialChain { get; private set; }
        public List<Line> Lines { get; private set; }

        public int MaxChainNumber { get; private set; }
        public int Count { get; private set; }
        public int MinMaterialNumber { get; private set; }
        public float MinDistance { get; private set; }
        public bool IsClickedRightMaterial { get; set; }
        public MaterialName HighlightedMaterialName { get; private set; }

        public RectTransform minPoint;
        public RectTransform maxPoint;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            ChainedNumber = 0;

            Objects = new List<GameObject>();
            MaterialNumbers = new Dictionary<MaterialName, int>();
            MaterialChain = new List<Material>();
            Lines = new List<Line>();

            MaxChainNumber = 5;
            Count = 17;
            MinMaterialNumber = 2;
            MinDistance = Screen.width / 7.2f;
            IsClickedRightMaterial = false;
        }

        private void Start()
        {
            // materialNumbers 초기화
            foreach (var item in StageManager.Instance.stageInfos[Data.DataManager.Instance.selected_stage].materials)
                MaterialNumbers.Add(item.GetComponent<Material>().materialName, 0);

            for (int i = 0; i < Count; i++)
                StartCoroutine("CreateMaterialCoroutine");

            StartCoroutine("UpdateHighlightedMaterialNameCoroutine");
        }

        IEnumerator UpdateHighlightedMaterialNameCoroutine()
        {
            while (true)
            {
                if (RecipeManager.Instance.RecipeNameList.Count != 0)
                {
                    if (!IsClickedRightMaterial) HighlightedMaterialName = RecipeManager.Instance.RecipeNameList[0];
                    else HighlightedMaterialName = RecipeManager.Instance.RecipeNameList[MaterialChain.Count + 1];
                }
                yield return new WaitForEndOfFrame();
            }
        }

        public IEnumerator CreateMaterialCoroutine()
        {
            while (RecipeManager.Instance.recipeQueue.Count <= MaxChainNumber)
                yield return new WaitForEndOfFrame();

            CreateMaterial();
        }

        public void CreateMaterial()
        {
            Vector3 position = GetNewMaterialPosition();
            GameObject instance;

            int materialIndex;
            Material material;

            if (FindFewMaterial(out materialIndex) == false)
                materialIndex = Random.Range(0, StageManager.Instance.stageInfos[Data.DataManager.Instance.selected_stage].materials.Length);

            instance = Instantiate(StageManager.Instance.stageInfos[Data.DataManager.Instance.selected_stage].materials[materialIndex], position, Quaternion.identity, transform);
            Objects.Add(instance);

            material = Objects[Objects.Count - 1].GetComponent<Material>();
            MaterialNumbers[material.materialName]++;
        }

        public Vector3 GetNewMaterialPosition()
        {
            Vector3 position = new Vector3();
            bool isTooClose = false;
            do
            {
                isTooClose = false;
                position.x = Random.Range(minPoint.position.x, maxPoint.position.x);
                position.y = Random.Range(minPoint.position.y, maxPoint.position.y);

                foreach (var item in Objects)
                    if ((item.transform.position - position).sqrMagnitude < (MinDistance * MinDistance)) isTooClose = true;
                    foreach (var item in ItemManager.Instance.Objects)
                        if ((item.transform.position - position).sqrMagnitude < (MinDistance * MinDistance)) isTooClose = true;
            } while (isTooClose);

            return position;
        }

        public void RespawnMaterial(Material material)
        {
            StartCoroutine("RespawnMaterialCoroutine", material);
        }

        private IEnumerator RespawnMaterialCoroutine(Material material)
        {
            material.ExpandAndDestroy();

            MaterialNumbers[material.materialName]--;
            Objects.Remove(material.gameObject);

            yield return new WaitForSeconds(0.5f);
            CreateMaterial();
        }

        private bool FindFewMaterial(out int index)
        {
            index = 0;

            foreach (var item in MaterialNumbers)
            {
                int minNumber = 0;
                Recipe[] recipeArray = RecipeManager.Instance.recipeQueue.ToArray();

                for(int i = 0; i < 5; i++)
                {
                    if (item.Key == recipeArray[i].recipeName)
                        minNumber++;
                }

                if (minNumber < MinMaterialNumber) minNumber = MinMaterialNumber;

                if (item.Value < minNumber) return true;
                index++;
            }

            return false;
        }
    }
}