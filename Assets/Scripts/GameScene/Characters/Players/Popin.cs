using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public class Popin : Player
    {
        private GameObject bulletSpawnPoint;

        protected override void Awake()
        {
            base.Awake();
            bulletSpawnPoint = transform.GetChild(5).gameObject;
        }

        public override void Attack(int chainNumber)
        {
            base.Attack(chainNumber);

            float damage = GetDamage(chainNumber);
            SpawnBullet(damage);
        }

        private void SpawnBullet(float damage)
        {
            StartCoroutine("SpawnBulletCoroutine", damage);
        }

        IEnumerator SpawnBulletCoroutine(float damage)
        {
            yield return new WaitForSeconds(0.3f);
            GameObject instance = Instantiate(PrefabManager.Instance.popinBullet, bulletSpawnPoint.transform.position, Quaternion.identity);
            instance.GetComponent<PopinBullet>().damage = damage;
        }
    }
}
