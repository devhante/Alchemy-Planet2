using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public class Monster : MonoBehaviour
    {
        [HideInInspector]
        public int index;

        protected float attackCoolTime;
        protected float attackRange;
        protected float attackPower;
        protected float moveSpeed;
        protected float maxHealth;

        protected bool isAttackCoroutinePlaying;

        protected Animator animator;
        protected Image healthBar;
        protected Rigidbody2D rigidbody2d;

        protected float health;
        public float Health
        {
            get { return health; }
            set
            {
                health = Mathf.Clamp(value, 0, maxHealth);
                UpdateHealthBar();

                if (health == 0)
                    Die();
            }
        }
        private void UpdateHealthBar()
        {
            healthBar.fillAmount = Health / maxHealth;
        }

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            // monster > canvas > healthbar_background > healthbar
            healthBar = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            StartCoroutine("MoveTowardCoroutine");
        }

        private IEnumerator MoveTowardCoroutine()
        {
            while (true)
            {
                // 피격받아서 공중에 떠있지 않을 때
                if (rigidbody2d.velocity.y == 0)
                {
                    // 사정거리 밖이라 이동
                    if (GetDistanceBetweenPlayer() > attackRange + (index * 0.5f))
                    {
                        StopCoroutine("AttackCoroutine");
                        isAttackCoroutinePlaying = false;
                        MoveToward();
                    }
                    // 사정거리 안이라 멈춰서 공격
                    else
                    {
                        if (isAttackCoroutinePlaying == false)
                            StartCoroutine("AttackCoroutine");
                    }
                }

                yield return null;
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

        private IEnumerator AttackCoroutine()
        {
            isAttackCoroutinePlaying = true;

            while (true)
            {
                yield return new WaitForSeconds(attackCoolTime);
                Attack();
            }
        }

        public virtual void Attack()
        {
            PlayAttackAnimation();
        }

        public virtual void Hit(float damage)
        {
            Health -= damage;
            rigidbody2d.AddForce(new Vector2(100, 100));
            PlayHitAnimation();
        }

        public virtual void Die()
        {
            GameManager.Instance.GainScore(ScoreType.KillMonster);
            StopCoroutine("MoveTowardCoroutine");
            StartCoroutine("DieCoroutine");
        }

        private IEnumerator DieCoroutine()
        {
            PlayDieAnimation();
            yield return new WaitForSeconds(0.2f);
            CoinManager.Instance.GenerateCoin(transform.position);

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(GetNameOfDieAnimation()))
                yield return new WaitForEndOfFrame();
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                yield return new WaitForEndOfFrame();

            MonsterManager.Instance.KillMonster(MonsterManager.Instance.GetKeyByValue(this));
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

        protected virtual string GetNameOfDieAnimation()
        {
            return "MonsterDie";
        }
    }
}