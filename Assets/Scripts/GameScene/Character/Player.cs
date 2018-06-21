using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace AlchemyPlanet.GameScene
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        public int AttackPower { get; private set; }
        public GameObject BulletSpawnPoint { get; private set; }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            AttackPower = 30;
            BulletSpawnPoint = transform.GetChild(5).gameObject;
        }

        public void Attack(int chainNumber)
        {
            int damage = (int)(AttackPower * chainNumber * (1 + chainNumber * 0.1));

            PlayAttackAnimation();
            SpawnBullet(damage);

            
        }

        private void PlayAttackAnimation()
        {
            GetComponent<Animator>().SetTrigger("Attack");
        }

        private void SpawnBullet(int damage)
        {
            StartCoroutine("SpawnBulletCoroutine", damage);
        }

        IEnumerator SpawnBulletCoroutine(int damage)
        {
            yield return new WaitForSeconds(0.3f);
            Instantiate(PrefabManager.Instance.bullet, BulletSpawnPoint.transform.position, Quaternion.identity).GetComponent<Bullet>().Damage = damage;
        }
    }
}