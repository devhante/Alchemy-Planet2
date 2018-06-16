using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class MonsterManager : MonoBehaviour
    {
        public static Queue<Monster> monsters = new Queue<Monster>();

        public Vector3 SpawnPoint { get; private set; }

        private void Awake()
        {
            SpawnPoint = new Vector3(6, 2.6f, 0);
        }

        public void SpawnMonster()
        {
            monsters.Enqueue(Instantiate(PrefabManager.Instance.monster, SpawnPoint, Quaternion.identity).GetComponent<Monster>());
        }

        IEnumerator SpawnMonsterCoroutine()
        {
            SpawnMonster();
            yield return new WaitForSeconds(10.0f);
        }
    }
}
