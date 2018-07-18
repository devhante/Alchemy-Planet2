using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class CoinManager : MonoBehaviour
    {
        public static CoinManager Instance { get; private set; }
        public Vector3 CoinDestination { get; private set; }

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
            CoinDestination = Player.Instance.transform.position;
        }

        public void GenerateCoin(Vector3 spawnPoint)
        {
            Instantiate(PrefabManager.Instance.coin, spawnPoint, Quaternion.identity);
        }

        public void GainCoin(int value)
        {
            GameManager.Instance.coin += value;
            UpdateCoin();
        }
        
        private void UpdateCoin()
        {
            GameUI.Instance.Unicoin.text = GameManager.Instance.coin.ToString("#,##0");
        }
    }
}