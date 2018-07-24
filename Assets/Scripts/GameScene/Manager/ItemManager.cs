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

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;

            Objects = new List<GameObject>();
            MaxItemNumber = 3;
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
            StopCoroutine("IncreasePurifyCoroutine");
            StartCoroutine("IncreasePurifyCoroutine");
        }

        IEnumerator IncreasePurifyCoroutine()
        {
            float duration = 5;
            float increaseRatio = 7;

            GameUI.Instance.IsIncreasingPurify = true;

            for (int i = 0; i < duration; i++)
            {
                GameUI.Instance.IncreasePurifyForItem(increaseRatio);
                yield return new WaitForSeconds(1);
            }

            GameUI.Instance.IsIncreasingPurify = false;
        }

        public void NoReducedOxygen()
        {
            StopCoroutine("NoReducedOxygenCoroutine");
            StartCoroutine("NoReducedOxygenCoroutine");
        }

        IEnumerator NoReducedOxygenCoroutine()
        {
            GameUI.Instance.IsNotReducingOxygen = true;
            yield return new WaitForSeconds(7);
            GameUI.Instance.IsNotReducingOxygen = false;
        }

        public void RainbowColorBall()
        {
            StopCoroutine("RainbowColorBallCoroutine");
            StartCoroutine("RainbowColorBallCoroutine");
        }

        IEnumerator RainbowColorBallCoroutine()
        {
            int index = Random.Range(0, PrefabManager.Instance.materialPrefabs.Length);
            string materialName = PrefabManager.Instance.materialPrefabs[index].GetComponent<Material>().materialName;
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
            yield return null;   
        }

        public void SlowReducedOxygen()
        {
            StopCoroutine("SlowReducedOxygenCoroutine");
            StartCoroutine("SlowReducedOxygenCoroutine");
        }

        IEnumerator SlowReducedOxygenCoroutine()
        {
            GameUI.Instance.OxygenReduceSpeed *= 0.5f;
            yield return new WaitForSeconds(10);
            GameUI.Instance.OxygenReduceSpeed /= 0.5f;
        }

        public void Sprint()
        {
            StopCoroutine("SprintCoroutine");
            StartCoroutine("SprintCoroutine");
        }

        IEnumerator SprintCoroutine()
        {
            float duration = 2;
            float speed = 3;

            StartCoroutine(SprintScoreCoroutine(duration, speed));

            TileManager.Instance.TileSpeed *= speed;
            BackgroundManager.Instance.BackgroundSpeed *= speed;
            yield return new WaitForSeconds(duration);
            TileManager.Instance.TileSpeed /= speed;
            BackgroundManager.Instance.BackgroundSpeed /= speed;
        }

        IEnumerator SprintScoreCoroutine(float duration, float speed)
        {
            while (duration > 0)
            {
                for(int i = 0; i < (int)speed - 1; i++)
                    GameManager.Instance.GainScore(ScoreType.TimePass);

                duration -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void ChickenBox()
        {
            Debug.Log("ChickenBox");
        }
    }
}