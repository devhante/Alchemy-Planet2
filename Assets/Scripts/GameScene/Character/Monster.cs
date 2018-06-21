using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public class Monster : MonoBehaviour
    {
        public float attackCoolTime;
        public float attackRange;
        public float attackPower;
        public float moveSpeed;
        public int maxHealth;

        private Animator animator;
        private Image healthBar;
        private int health;
        public int Health
        {
            get { return health; }
            set
            {
                health = Mathf.Clamp(value, 0, maxHealth);
                UpdateHealthBar();
                if (health == 0) MonsterManager.Instance.KillMonster(MonsterManager.Instance.GetKeyByValue(this));
            }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            healthBar = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            health = maxHealth;
        }

        private void Start()
        {
            StartCoroutine("MoveTowardCoroutine");
        }

        IEnumerator MoveTowardCoroutine()
        {
            while(GetDistanceBetweenPlayer() > attackRange)
            {
                MoveToward();
                yield return null;
            }

            StartCoroutine("AttackCoroutine");
        }

        IEnumerator AttackCoroutine()
        {
            while(true)
            {
                Attack();
                yield return new WaitForSeconds(attackCoolTime);
            }
        }

        private float GetDistanceBetweenPlayer()
        {
            return transform.position.x - Player.Instance.transform.position.x;
        }

        private void MoveToward()
        {
            transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
        }

        public void Attack()
        {
            PlayAttackAnimation();
            GameUI.Instance.UpdateGage(Gages.PURIFY, -attackPower);
        }
        
        private void UpdateHealthBar()
        {
            healthBar.fillAmount = (float)health / maxHealth;
        }

        public void Hit(int damage)
        {
            health -= damage;
            PlayHitAnimation();
        }

        public void Die()
        {
            PlayDieAnimation();
        }

        private void PlayAttackAnimation()
        {
            animator.SetTrigger("Attack");
        }

        private void PlayHitAnimation()
        {
            animator.SetTrigger("Hit");
        }

        private void PlayDieAnimation()
        {
            animator.SetTrigger("Die");
        }

    }
}