using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class DropManager : MonoBehaviour
    {
        public static DropManager Instance { get; private set; }
        public Vector3 DropDestination { get; private set; }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            DropDestination = Player.Instance.transform.position;
        }

        public void GenerateCoin(Vector3 spawnPoint)
        {
            Instantiate(PrefabManager.Instance.coin, spawnPoint, Quaternion.identity);
        }

        public void GenerateDropMaterial(Vector3 spawnPoint)
        {
            GameObject instance = PrefabManager.Instance.dropMaterialPrefabs[Random.Range(0, PrefabManager.Instance.dropMaterialPrefabs.Length)];
            Instantiate(instance, spawnPoint, Quaternion.identity);
        }

        public void GainCoin(int value)
        {
            GameManager.Instance.Coin += value;
            UpdateCoin();
        }

        public void GainDropMaterial(MaterialName materialName)
        {
            if (GameManager.Instance.DropMaterialList.ContainsKey(materialName))
                GameManager.Instance.DropMaterialList[materialName]++;

            else GameManager.Instance.DropMaterialList.Add(materialName, 1);

            UpdateDropMaterial();
        }
        
        private void UpdateCoin()
        {
            GameUI.Instance.Unicoin.text = GameManager.Instance.Coin.ToString("#,##0");
        }

        private void UpdateDropMaterial()
        {
            foreach (var item in GameManager.Instance.DropMaterialList)
            {
                Debug.Log(string.Format("{0}을/를 {1}개 얻음.", item.Key, item.Value));
            }
        }
    }
}