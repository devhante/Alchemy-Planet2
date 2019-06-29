using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager Instance { get; private set; }

        public Dictionary<ItemName, int> CreatedItemNumber { get; set; }
        public Dictionary<ItemName, int> UsedItemNumber { get; set; }

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

            CreatedItemNumber = new Dictionary<ItemName, int>();
            UsedItemNumber = new Dictionary<ItemName, int>();

            CreatedItemNumber.Add(ItemName.PopinPotionBlack, 0);
            UsedItemNumber.Add(ItemName.PopinPotionBlack, 0);
        }

        public void CreateItem(ItemName itemName)
        {
            if (Objects.Count < MaxItemNumber)
            {
                Vector3 position = MaterialManager.Instance.GetNewMaterialPosition();
                GameObject prefab = new GameObject();

                GameObject instance = Instantiate(prefab, position, Quaternion.identity, transform);
                Objects.Add(instance);
                CreatedItemNumber[itemName]++;
                Debug.Log(Instance.gameObject);
            }
        }

        public ItemName GetItemName()
        {
            int result = 0;
            float random = Random.Range(0f, 1f);
            int length = GameSettings.Instance.itemChanges_Value.Length;

            for (int i = 0; i < length; i++)
            {
                random -= GameSettings.Instance.itemChanges_Value[i];
                if (random <= 0)
                {
                    result = i;
                    break;
                }

                if (i == length - 1)
                    result = length - 1;
            }

            Debug.Log(GameSettings.Instance.itemChanges_Key[result] + " " + "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            return GameSettings.Instance.itemChanges_Key[result];
        }
    }
}