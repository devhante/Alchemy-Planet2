using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public class HarpRadisheal : Monster
    {
        private GameObject bulletSpawnPoint;

        private SpriteRenderer bodySpriteRenderer;
        private SpriteRenderer faceSpriteRenderer;

        protected override void Awake()
        {
            base.Awake();

            bodySpriteRenderer = GetComponent<SpriteRenderer>();
            faceSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
            bulletSpawnPoint = transform.GetChild(2).gameObject;
            isAttackCoroutinePlaying = false;

            attackCoolTime = 5;
            attackRange = 4;
            attackPower = 2;
            moveSpeed = 2;
            maxHealth = 300;
            health = maxHealth;
        }

        public override void Attack()
        {
            base.Attack();
            SpawnBullet();
        }

        private void SpawnBullet()
        {
            StartCoroutine("SpawnBulletCoroutine");
        }

        private IEnumerator SpawnBulletCoroutine()
        {
            yield return new WaitForSeconds(0.8f);
            Instantiate(PrefabManager.Instance.harpRadishealBullet, bulletSpawnPoint.transform.position, Quaternion.identity);
        }

        public void ChangeSortingLayer(int bodyLayerNumber, int faceLayerNumber)
        {
            bodySpriteRenderer.sortingOrder = bodyLayerNumber;
            faceSpriteRenderer.sortingOrder = faceLayerNumber;
        }

        protected override string GetNameOfDieAnimation()
        {
            return "HarpRadishealDie";
        }
    }
}
