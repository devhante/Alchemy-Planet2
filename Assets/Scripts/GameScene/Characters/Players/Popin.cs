using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public class Popin : Player
    {
        public GameObject itemChickenBox;
        public Transform chickenCountSpawnPosition;

        private GameObject bulletSpawnPoint;
        private int chickenNumber;
        private Text chickenCount;

        protected override void Awake()
        {
            base.Awake();
            bulletSpawnPoint = transform.GetChild(5).gameObject;
            chickenNumber = 9;
        }

        private void Start()
        {
            chickenCount = Instantiate(PrefabManager.Instance.chickenCount, GameUI.Instance.transform).GetComponent<Text>();
            StartCoroutine("SetChickenCountPositionCoroutine");
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

        public override void GetMaterialMessage(MaterialName materialName)
        {
            if (materialName == MaterialName.Chicken)
            {
                chickenNumber--;
                if (chickenNumber == 0)
                {
                    chickenNumber = 9;
                    SpawnChickenBox();
                }

                chickenCount.text = chickenNumber.ToString();
            }
        }

        IEnumerator SetChickenCountPositionCoroutine()
        {
            while (true)
            {
                chickenCount.rectTransform.position = Camera.main.WorldToScreenPoint(chickenCountSpawnPosition.position);
                yield return new WaitForEndOfFrame();
            }

        }

        private void SpawnChickenBox()
        {
            Vector3 position = MaterialManager.Instance.GetNewMaterialPosition();
            GameObject instance = Instantiate(itemChickenBox, position, Quaternion.identity, ItemManager.Instance.transform);
            ItemManager.Instance.Objects.Add(instance);
        }
    }
}
