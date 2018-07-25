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

        private Coroutine increasePurifyCoroutine = null;
        private Coroutine noReducedOxygenCoroutine = null;
        private Coroutine rainbowColorBallCoroutine = null;
        private Coroutine slowReduceOxygenCoroutine = null;
        private Coroutine sprintCoroutine = null;

        private bool isIncreasePurifyPlaying = false;
        private bool isNoReducedOxygenPlaying = false;
        private bool isRainbowColorBallPlaying = false;
        private bool isSlowReduceOxygenPlaying = false;
        private bool isSprintPlaying = false;

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
            if (isIncreasePurifyPlaying)
            {
                StopCoroutine("IncreasePurifyCoroutine");
                StartCoroutine("increasePurifyEndCoroutine");
            }
            StartCoroutine("IncreasePurifyCoroutine");
        }

        IEnumerator IncreasePurifyCoroutine()
        {
            float duration = 5;
            float increaseRatio = 7;

            GameUI.Instance.IsIncreasingPurify = true;

            isIncreasePurifyPlaying = true;
            for (int i = 0; i < duration; i++)
            {
                GameUI.Instance.IncreasePurifyForItem(increaseRatio);
                yield return new WaitForSeconds(1);
            }
            isIncreasePurifyPlaying = false;

            StartCoroutine("IncreasePurifyEndCoroutine");
        }

        IEnumerator IncreasePurifyEndCoroutine()
        {
            GameUI.Instance.IsIncreasingPurify = false;
            yield return null;
        }

        public void NoReducedOxygen()
        {
            if (isNoReducedOxygenPlaying)
            {
                StopCoroutine("NoReducedOxygenCoroutine");
                StartCoroutine("NoReducedOxygenEndCoroutine");
            }
            StartCoroutine("NoReducedOxygenCoroutine");
        }

        IEnumerator NoReducedOxygenCoroutine()
        {
            GameUI.Instance.IsNotReducingOxygen = true;

            isNoReducedOxygenPlaying = true;
            yield return new WaitForSeconds(7);
            isNoReducedOxygenPlaying = false;
            StartCoroutine("NoReducedOxygenEndCoroutine");
        }

        IEnumerator NoReducedOxygenEndCoroutine()
        {
            GameUI.Instance.IsNotReducingOxygen = false;
            yield return null;
        }

        public void RainbowColorBall()
        {
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
            if(isSlowReduceOxygenPlaying)
            {
                StopCoroutine("SlowReducedOxygenCoroutine");
                StartCoroutine("SlowReducedOxygenEndCoroutine");
            }
            StartCoroutine("SlowReducedOxygenCoroutine");
        }

        IEnumerator SlowReducedOxygenCoroutine()
        {
            GameUI.Instance.OxygenReduceSpeed *= 0.5f;
            isSlowReduceOxygenPlaying = true;
            yield return new WaitForSeconds(10);
            isSlowReduceOxygenPlaying = false;
            StartCoroutine("SlowReducedOxygenEndCoroutine");
        }

        IEnumerator SlowReducedOxygenEndCoroutine()
        {
            GameUI.Instance.OxygenReduceSpeed /= 0.5f;
            yield return null;
        }

        public void Sprint()
        {
            float duration = 2;
            float speed = 3;

            if (isSprintPlaying)
            {
                StopCoroutine(sprintCoroutine);
                StopCoroutine("SprintScoreCoroutine");
                StartCoroutine("SprintEndCoroutine", speed);
            }
            sprintCoroutine = StartCoroutine(SprintCoroutine(duration, speed));
        }

        IEnumerator SprintCoroutine(float duration, float speed)
        {
            StartCoroutine(SprintScoreCoroutine(duration, speed));

            TileManager.Instance.TileSpeed *= speed;
            BackgroundManager.Instance.BackgroundSpeed *= speed;
            isSprintPlaying = true;
            yield return new WaitForSeconds(duration);
            isSprintPlaying = false;
            StartCoroutine("SprintEndCoroutine", speed);
        }

        IEnumerator SprintEndCoroutine(float speed)
        {
            Debug.Log(TileManager.Instance.TileSpeed);
            TileManager.Instance.TileSpeed /= speed;
            BackgroundManager.Instance.BackgroundSpeed /= speed;
            yield return null;
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