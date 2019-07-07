using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class MonsterManager : MonoBehaviour
    {
        public static MonsterManager Instance { get; private set; } 

        public int DeadMonsterNumber { get; set; }

        public Dictionary<int, Monster> Monsters { get; private set; }
        public Vector3 SpawnPoint { get; private set; }
        public int Key { get; private set; }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            DeadMonsterNumber = 0;
            Monsters = new Dictionary<int, Monster>();
            SpawnPoint = new Vector3(6, 2, 0);
            Key = 0;
        }

        public void SpawnMonster()
        {
            HarpRadisheal harpRadisheal = Instantiate(PrefabManager.Instance.monster, SpawnPoint, Quaternion.identity).GetComponent<HarpRadisheal>();
            Monsters.Add(Key, harpRadisheal);
            harpRadisheal.index = Monsters.Count - 1;
            harpRadisheal.ChangeSortingLayer(harpRadisheal.index * -2, harpRadisheal.index * -2 + 1);
            Key++;
        }

        public void SpawnBossMonster()
        {
            HarpRadisheal harpRadisheal = Instantiate(PrefabManager.Instance.bossMonster, SpawnPoint, Quaternion.identity).GetComponent<HarpRadisheal>();
            Monsters.Add(Key, harpRadisheal);
            harpRadisheal.index = Monsters.Count - 1;
            harpRadisheal.ChangeSortingLayer(harpRadisheal.index * -2, harpRadisheal.index * -2 + 1);
            Key++;
        }

        IEnumerator SpawnBossMonsterCoroutine()
        {
            // Zoom Out
            while (Camera.main.orthographicSize <= 9.4f)
            {
                Camera.main.orthographicSize += Time.deltaTime * 2;
                Camera.main.transform.position += new Vector3(Time.deltaTime * 0.8f, -Time.deltaTime * 0.2f);
                yield return null; 
            }
            Camera.main.orthographicSize = 9.4f;

            // Spawn
            SpawnBossMonster();

            // Shake
            Vector3 originPos = Camera.main.transform.position;
            while (Monsters[Key - 1].isMoving)
            {
                Camera.main.transform.localPosition = (Vector3)Random.insideUnitCircle * 0.05f + originPos;
                yield return null;
            }
            Camera.main.transform.localPosition = originPos;

            while (Monsters.Count > 0)
                yield return null;
            StartCoroutine("SpawnMonsterCoroutine");
        }

        public void KillMonster(int key)
        {
            DestroyMonster(key);
            Monsters.Remove(key);
            ReallocateMonsterIndex();
        }

        private void DestroyMonster(int key)
        {
            Monster monster;
            Monsters.TryGetValue(key, out monster);
            Destroy(monster.gameObject);
        }

        private void ReallocateMonsterIndex()
        {
            int index = 0;
            foreach(var item in Monsters)
            {
                item.Value.index = index;
                item.Value.ChangeSortingLayer(item.Value.index * -2, item.Value.index * -2 + 1);
                index++;
            }
        }

        public int GetKeyByValue(Monster monster)
        {
            foreach (var item in Monsters)
                if (item.Value.Equals(monster))
                    return item.Key;

            return -1;
        }
    }
}
