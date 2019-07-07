using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace AlchemyPlanet.GameScene
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }

        public int PlayerHitNumber { get; set; }

        protected float health;
        public float Health
        {
            get { return health; }
            set
            {
                health = Mathf.Clamp(value, 0, maxHealth);
                UpdateHealthBar();
                if(health == 0)
                {
                    UIManager.Instance.OpenMenu<EndUI>();
                    if (StoryLobbyScene.StoryManager.Instance != null)
                    {
                        SceneManager.sceneLoaded -= StoryLobbyScene.StoryManager.Instance.OnSceneLoaded;
                        Destroy(StoryLobbyScene.StoryManager.Instance.gameObject);
                    }
                }
            }
        }

        [SerializeField]
        protected int maxHealth;

        [SerializeField]
        protected Image healthBar;

        
        protected float attackPower;
        protected Animator animator;

        private void OnDestroy()
        {
            Instance = null;
        }

        protected virtual void Awake()
        {
            Instance = this;
            PlayerHitNumber = 0;
            health = maxHealth;
            attackPower = 30;
            animator = GetComponent<Animator>();
        }

        public virtual void Attack(int chainNumber, float purifyGage)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("PopinAttack"))
                animator.SetTrigger("StopAttack");

            animator.SetTrigger("StartAttack");
        }

        public virtual void Hit(float damage)
        {
            Health = Health - damage;
            PlayHitAnimation();
            PlayerHitNumber++;
        }

        public float GetDamage(int chainNumber, float purifyGage)
        {
            return attackPower * chainNumber * (1 + chainNumber * 0.1f) * (1 + purifyGage * 0.04f);
        }

        private void UpdateHealthBar()
        {
            healthBar.fillAmount = Health / maxHealth;
        }

        public virtual void GetMaterialMessage(MaterialName materialName)
        {
            //Debug.Log("GetMaterialMessage: " + materialName);
        }

        private void PlayHitAnimation()
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("PopinHit"))
                animator.SetTrigger("StopHit");

            animator.SetTrigger("StartHit");
        }

        public void Run()
        {
            animator.SetTrigger("StartRun");
        }

        public void Idle()
        {
            animator.SetTrigger("StopRun");
        }
    }
}