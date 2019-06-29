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

        public Dictionary<PopinPotionColor, int> UsedSkillNumber { get; set; }

        private int skillGage;
        public int SkillGage
        {
            get { return skillGage; }
            set
            {
                if (isSkillOn == false)
                {
                    skillGage = Mathf.Clamp(value, 0, 100);

                    if (skillGage == 100)
                    {
                        color = GetPopinPotionColor();
                        isSkillOn = true;
                        skillGage = 0;
                    }
                }
                Debug.Log("Skill Gage : " + skillGage);
            }
        }

        public GameObject itemPopinPotionBlack;
        
        [HideInInspector]
        public bool isSkillOn = false;

        [HideInInspector]
        public PopinPotionColor color;

        private GameObject bulletSpawnPoint;
        private PopinPotionColor[] popinPotionColorList = { PopinPotionColor.Blue, PopinPotionColor.Black };
        
        private void OnDestroy()
        {
            Instance = null;
        }

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
            bulletSpawnPoint = transform.GetChild(5).gameObject;

            UsedSkillNumber = new Dictionary<PopinPotionColor, int>();
            UsedSkillNumber.Add(PopinPotionColor.Blue, 0);
            UsedSkillNumber.Add(PopinPotionColor.Black, 0);
        }

        public override void Attack(int chainNumber, float purifyGage)
        {
            base.Attack(chainNumber, purifyGage);
            float damage = GetDamage(chainNumber, purifyGage);
            SpawnBullet(damage);
        }

        private void SpawnBullet(float damage)
        {
            StartCoroutine("SpawnBulletCoroutine", damage);
        }

        IEnumerator SpawnBulletCoroutine(float damage)
        {
            while (animator.GetCurrentAnimatorStateInfo(0).IsName("PopinAttack") == false)
                yield return null;

            yield return new WaitForSeconds(0.1f);
            GameObject instance = Instantiate(PrefabManager.Instance.popinBullet, bulletSpawnPoint.transform.position, Quaternion.identity);
            instance.GetComponent<PopinBullet>().damage = damage;
        }

        private PopinPotionColor GetPopinPotionColor()
        {
            int result = 0;
            float random = Random.Range(0f, 1f);
            int length = GameSettings_Popin.Instance.popinPotionChanges_Value.Length;

            for (int i = 0; i < length; i++)
            {
                Debug.Log(random);
                random -= GameSettings_Popin.Instance.popinPotionChanges_Value[i];
                Debug.Log(random);
                if (random <= 0)
                {
                    result = i;
                    break;
                }

                if (i == length - 1)
                    result = length - 1;
            }

            return GameSettings_Popin.Instance.popinPotionChances_Key[result];
        }

        public void Skill()
        {
            Skill(color);
            isSkillOn = false;
            UsedSkillNumber[color]++;
        }

        private void Skill(PopinPotionColor color)
        {
            switch (color)
            {
                case PopinPotionColor.Blue:
                    StartCoroutine("PotionBlueCoroutine");
                    break;

                case PopinPotionColor.Black:
                    StartCoroutine("PotionBlackCoroutine");
                    break;
            }
        }

        IEnumerator PotionBlueCoroutine()
        {
            Debug.Log("PotionBlueCoroutine");
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
            Debug.Log("PotionBlackCoroutine");
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
    }
}
