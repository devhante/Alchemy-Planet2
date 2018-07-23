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
            if (Instance == null) Instance = this;
            else Destroy(this);

            Objects = new List<GameObject>();
        }

        public void CreateItem()
        {
            Debug.Log("CreateItem");
            Vector3 position = MaterialManager.Instance.GetNewMaterialPosition();
            int itemIndex = Random.Range(0, PrefabManager.Instance.itemPrefabs.Length);
            GameObject instance = Instantiate(PrefabManager.Instance.itemPrefabs[itemIndex], position, Quaternion.identity, transform);
            Objects.Add(instance);
        }
    }
}