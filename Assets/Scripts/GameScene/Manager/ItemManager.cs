using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager Instance { get; private set; }

        public List<GameObject> Objects { get; private set; }
        public int MaxItemNumber { get; private set; }
        public bool IsSprinting { get; private set; }

        public GameObject sprintEffect;

        private int increasePurifyCount = 0;
        private int noReducedOxygenCount = 0;
        private int slowReducedOxygenCount = 0;
        private int sprintCount = 0;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;

            Objects = new List<GameObject>();
            MaxItemNumber = 3;
            IsSprinting = false;
        }

        public void CreateItem()
        {
            if (Objects.Count < MaxItemNumber)
            {
                Vector3 position = MaterialManager.Instance.GetNewMaterialPosition();
                int itemIndex = Random.Range(0, PrefabManager.Instance.itemPrefabs.Length);
                GameObject instance = Instantiate(PrefabManager.Instance.itemPrefabs[itemIndex], position, Quaternion.identity, transform);
                Objects.Add(instance);
            }
        }

        public void IncreasePurify()
        {
            increasePurifyCount++;

            if(increasePurifyCount == 1)
                StartCoroutine("IncreasePurifyCoroutine");
        }

        IEnumerator IncreasePurifyCoroutine()
        {
            float duration = 5;
            float increaseRatio = 7;

            GameUI.Instance.IsIncreasingPurify = true;

            while (increasePurifyCount > 0)
            {
                for (int i = 0; i < duration; i++)
                {
                    GameUI.Instance.UpdateGage(Gages.PURIFY, increaseRatio);
                    yield return new WaitForSeconds(1);
                }

                increasePurifyCount--;
            }

            GameUI.Instance.IsIncreasingPurify = false;
        }

        public void NoReducedOxygen()
        {
            noReducedOxygenCount++;

            if(noReducedOxygenCount == 1)
                StartCoroutine("NoReducedOxygenCoroutine");
        }

        IEnumerator NoReducedOxygenCoroutine()
        {
            GameUI.Instance.IsNotReducingOxygen = true;

            while (noReducedOxygenCount > 0)
            {
                yield return new WaitForSeconds(7);
                noReducedOxygenCount--;
            }

            GameUI.Instance.IsNotReducingOxygen = false;
        }

        public void RainbowColorBall()
        {
            int index = Random.Range(0, StageManager.Instance.stageInfos[Data.DataManager.Instance.selected_stage].materials.Length);
            MaterialName materialName = StageManager.Instance.stageInfos[Data.DataManager.Instance.selected_stage].materials[index].GetComponent<Material>().materialName;
            List<Material> materials = new List<Material>();

            foreach (var item in MaterialManager.Instance.Objects)
            {
                Material material = item.GetComponent<Material>();
                if (material.materialName == materialName) materials.Add(material);
            }

            foreach (var item in materials)
            {
                MaterialManager.Instance.RespawnMaterial(item);
                GameManager.Instance.GainScore(ScoreType.TouchRightRecipe);
                GameUI.Instance.UpdateGage(Gages.PURIFY, 2.5f);
            }
        }

        public void SlowReducedOxygen()
        {
            slowReducedOxygenCount++;

            if(slowReducedOxygenCount == 1)
                StartCoroutine("SlowReducedOxygenCoroutine");
        }

        IEnumerator SlowReducedOxygenCoroutine()
        {
            GameUI.Instance.OxygenReduceSpeed *= 0.5f;

            while (slowReducedOxygenCount > 0)
            {
                yield return new WaitForSeconds(10);
                slowReducedOxygenCount--;
            }

            GameUI.Instance.OxygenReduceSpeed /= 0.5f;
        }

        public void Sprint()
        {
            sprintCount++;

            if (sprintCount == 1)
                StartCoroutine("SprintCoroutine");
        }

        IEnumerator SprintCoroutine()
        {
            float duration = 2;

            IsSprinting = true;
            GameManager.Instance.UpdateSpeed();
            GameObject sprintEffectObject = Instantiate(sprintEffect, GameObject.FindWithTag("Player").transform.position, Quaternion.identity);
            GameUI.Instance.TopWind.SetActive(true);
            GameUI.Instance.BottomWind.SetActive(true);

            while (sprintCount > 0)
            {
                yield return new WaitForSeconds(duration);
                sprintCount--;
            }

            IsSprinting = false;
            GameManager.Instance.UpdateSpeed();
            Destroy(sprintEffectObject);
            GameUI.Instance.TopWind.SetActive(false);
            GameUI.Instance.BottomWind.SetActive(false);
        }
    }
}