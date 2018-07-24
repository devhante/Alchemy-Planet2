using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager Instance { get; private set; }

        public List<GameObject> Objects { get; private set; }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;

            Objects = new List<GameObject>();
        }

        public void CreateItem()
        {
            if (Objects.Count < 3)
            {
                Vector3 position = MaterialManager.Instance.GetNewMaterialPosition();
                int itemIndex = Random.Range(0, PrefabManager.Instance.itemPrefabs.Length);
                GameObject instance = Instantiate(PrefabManager.Instance.itemPrefabs[itemIndex], position, Quaternion.identity, transform);
                Objects.Add(instance);
            }
        }

        public void IncreasePurify()
        {

        }

        public void NoReducedOxygen()
        {

        }

        public void RainbowColorBall()
        {

        }

        public void SlowReducedOxygen()
        {

        }

        public void Sprint()
        {
            StartCoroutine("SprintCoroutine");
        }

        IEnumerator SprintCoroutine()
        {
            TileManager.Instance.TileSpeed *= 3f;
            BackgroundManager.Instance.BackgroundSpeed *= 3f;
            yield return new WaitForSeconds(2);
            TileManager.Instance.TileSpeed /= 3f;
            BackgroundManager.Instance.BackgroundSpeed /= 3f;
        }
    }
}