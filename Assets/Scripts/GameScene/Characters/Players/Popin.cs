using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public enum PopinPotionColor { Red, Green, Blue, Black, Rainbow }
    
    public class Popin : Player
    {
        public static new Popin Instance { get; private set; }

        public bool PotionGreen { get; set; }
        public bool PotionRed { get; set; }

        public GameObject itemPopinPotionBlack;

        private GameObject bulletSpawnPoint;

        private void OnDestroy()
        {
            Instance = null;
        }

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
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

        public void Skill(PopinPotionColor color)
        {
            switch (color)
            {
                case PopinPotionColor.Red:
                    StartCoroutine("PotionRedCoroutine");
                    break;

                case PopinPotionColor.Green:
                    StartCoroutine("PotionGreenCoroutine");
                    break;

                case PopinPotionColor.Blue:
                    StartCoroutine("PotionBlueCoroutine");
                    break;

                case PopinPotionColor.Black:
                    StartCoroutine("PotionBlackCoroutine");
                    break;

                case PopinPotionColor.Rainbow:
                    StartCoroutine("PotionRainbowCoroutine");
                    break;
            }
        }

        IEnumerator PotionRedCoroutine()
        {
            animator.SetTrigger("PotionRed");

            while (animator.GetNextAnimatorStateInfo(0).IsName("PopinPotionRed") == false)
                yield return new WaitForEndOfFrame();

            yield return new WaitForSeconds(2);
            GameObject effect = Instantiate(PrefabManager.Instance.potionEffectRed, transform.position + new Vector3(0, -0.6f, 0), Quaternion.identity, transform);

            PotionRed = true;
            GameManager.Instance.UpdateSpeed();

            yield return new WaitForSeconds(2);

            PotionRed = false;
            GameManager.Instance.UpdateSpeed();

            Destroy(effect);
        }

        IEnumerator PotionGreenCoroutine()
        {
            animator.SetTrigger("PotionGreen");

            while (animator.GetNextAnimatorStateInfo(0).IsName("PopinPotionGreen") == false)
                yield return new WaitForEndOfFrame();

            yield return new WaitForSeconds(2);
            GameObject effect = Instantiate(PrefabManager.Instance.potionEffectGreen, transform.position + new Vector3(0, -0.6f, 0), Quaternion.identity, transform);

            PotionGreen = true;
            float purify = GameUI.Instance.GetGage(Gages.PURIFY);
            GameUI.Instance.UpdateGage(Gages.PURIFY, 100 - purify);

            yield return new WaitForSeconds(2);

            PotionGreen = false;
            GameUI.Instance.UpdateGage(Gages.PURIFY, purify - 100);

            Destroy(effect);
        }

        IEnumerator PotionBlueCoroutine()
        {
            animator.SetTrigger("PotionBlue");

            while (animator.GetNextAnimatorStateInfo(0).IsName("PopinPotionBlue") == false)
                yield return new WaitForEndOfFrame();

            yield return new WaitForSeconds(2);
            GameObject effect = Instantiate(PrefabManager.Instance.potionEffectBlue, transform.position + new Vector3(0, -0.6f, 0), Quaternion.identity, transform);

            for(int i = 0; i < 4; i++)
            {
                GameUI.Instance.UpdateGage(Gages.OXYGEN, 5);
                yield return new WaitForSeconds(0.5f);
            }

            Destroy(effect);
        }

        IEnumerator PotionBlackCoroutine()
        {
            animator.SetTrigger("PotionBlack");

            while (animator.GetNextAnimatorStateInfo(0).IsName("PopinPotionBlack") == false)
                yield return new WaitForEndOfFrame();

            yield return new WaitForSeconds(2);
            SpawnPopinPotionBlack();

            yield return null;
        }

        public void SpawnPopinPotionBlack()
        {
            Vector3 position = MaterialManager.Instance.GetNewMaterialPosition();
            GameObject instance = Instantiate(itemPopinPotionBlack, position, Quaternion.identity, ItemManager.Instance.transform);
            ItemManager.Instance.Objects.Add(instance);
        }

        IEnumerator PotionRainbowCoroutine()
        {
            animator.SetTrigger("PotionRainbow");

            while (animator.GetNextAnimatorStateInfo(0).IsName("PopinPotionRainbow") == false)
                yield return new WaitForEndOfFrame();

            yield return new WaitForSeconds(2);
            GameObject effect = Instantiate(PrefabManager.Instance.potionEffectRainbow, transform.position + new Vector3(0, -0.6f, 0), Quaternion.identity, transform);

            PotionRed = true;
            GameManager.Instance.UpdateSpeed();
            PotionGreen = true;
            float purify = GameUI.Instance.GetGage(Gages.PURIFY);
            GameUI.Instance.UpdateGage(Gages.PURIFY, 100 - purify);

            for (int i = 0; i < 4; i++)
            {
                GameUI.Instance.UpdateGage(Gages.OXYGEN, 5);
                yield return new WaitForSeconds(0.5f);
            }

            PotionRed = false;
            GameManager.Instance.UpdateSpeed();
            PotionGreen = false;
            GameUI.Instance.UpdateGage(Gages.PURIFY, purify - 100);

            Destroy(effect);
        }
    }
}
