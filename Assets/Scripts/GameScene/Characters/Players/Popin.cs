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

        private int skillGage;
        public int SkillGage
        {
            get { return skillGage; }
            set
            {
                skillGage = Mathf.Clamp(value, 0, 100);
            }
        }

        public bool PotionGreen { get; set; }
        public bool PotionRed { get; set; }

        public GameObject itemPopinPotionBlack;
        public Image skillBar;

        private GameObject bulletSpawnPoint;
        private PopinPotionColor[] popinPotionColorList = { PopinPotionColor.Red, PopinPotionColor.Green, PopinPotionColor.Blue, PopinPotionColor.Black, PopinPotionColor.Rainbow };
        
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

        private void Start()
        {
            StartCoroutine("UpdateSkillGageCoroutine");
        }

        private void UpdateSkillBar()
        {
            skillBar.fillAmount = SkillGage / 100.0f;
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

        IEnumerator UpdateSkillGageCoroutine()
        {
            while (true)
            {
                float skillGageValue = skillBar.fillAmount * 100;

                while (Mathf.Abs(SkillGage - skillGageValue) > 0.01f)
                {
                    skillGageValue = Mathf.Lerp(skillGageValue, SkillGage, 0.1f);
                    skillBar.fillAmount = skillGageValue / 100.0f;
                    yield return new WaitForSeconds(0.01f);
                }

                if (1 - skillBar.fillAmount < 0.01f)
                {
                    int index = Random.Range(0, popinPotionColorList.Length);
                    Skill(PopinPotionColor.Black);
                    //Skill(popinPotionColorList[index]);
                    SkillGage = 0;
                }

                yield return new WaitForEndOfFrame();
            }
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
            GameObject effect = Instantiate(PrefabManager.Instance.potionEffectRed, transform.position + new Vector3(0.05f, -0.7f, 0), Quaternion.identity, transform);

            PotionRed = true;
            GameManager.Instance.UpdateSpeed();

            yield return new WaitForSeconds(5);

            PotionRed = false;
            GameManager.Instance.UpdateSpeed();

            Destroy(effect);
        }

        IEnumerator PotionGreenCoroutine()
        {
            animator.SetTrigger("PotionGreen");

            while (animator.GetNextAnimatorStateInfo(0).IsName("PopinPotionGreen") == false)
                yield return new WaitForEndOfFrame();

            yield return new WaitForSeconds(1.5f);
            GameObject effect = Instantiate(PrefabManager.Instance.potionEffectGreen, transform.position + new Vector3(0.05f, -0.7f, 0), Quaternion.identity, transform);

            PotionGreen = true;
            float purify = GameUI.Instance.GetGage(Gages.PURIFY);
            GameUI.Instance.UpdateGage(Gages.PURIFY, 100 - purify);

            yield return new WaitForSeconds(5);

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
            GameObject effect = Instantiate(PrefabManager.Instance.potionEffectBlue, transform.position + new Vector3(0.05f, -0.7f, 0), Quaternion.identity, transform);

            for(int i = 0; i < 5; i++)
            {
                GameUI.Instance.UpdateGage(Gages.OXYGEN, 4);
                yield return new WaitForSeconds(1);
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
            GameObject effect = Instantiate(PrefabManager.Instance.potionEffectRainbow, transform.position + new Vector3(0.05f, -0.7f, 0), Quaternion.identity, transform);

            PotionRed = true;
            GameManager.Instance.UpdateSpeed();
            PotionGreen = true;
            float purify = GameUI.Instance.GetGage(Gages.PURIFY);
            GameUI.Instance.UpdateGage(Gages.PURIFY, 100 - purify);

            for (int i = 0; i < 5; i++)
            {
                GameUI.Instance.UpdateGage(Gages.OXYGEN, 4);
                yield return new WaitForSeconds(1);
            }

            PotionRed = false;
            GameManager.Instance.UpdateSpeed();
            PotionGreen = false;
            GameUI.Instance.UpdateGage(Gages.PURIFY, purify - 100);

            Destroy(effect);
        }
    }
}
