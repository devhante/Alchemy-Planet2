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

        public GameObject BulletSpawnPoint { get; private set; }
        public bool IsAttackCoroutinePlaying { get; private set; }
        public int Index { get; set; }

        private Animator animator;
        private Image healthBar;
        private Rigidbody2D rigidbody2d;
        private SpriteRenderer bodySpriteRenderer;
        private SpriteRenderer faceSpriteRenderer;
        private int health;
        public int Health
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
            healthBar.fillAmount = (float)Health / maxHealth;
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            healthBar = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            rigidbody2d = GetComponent<Rigidbody2D>();
            bodySpriteRenderer = GetComponent<SpriteRenderer>();
            faceSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
            BulletSpawnPoint = transform.GetChild(2).gameObject;
            IsAttackCoroutinePlaying = false;
            Health = maxHealth;
        }

        private void Start()
        {
            StartCoroutine("MoveTowardCoroutine");
        }

        private IEnumerator MoveTowardCoroutine()
        {
            while(true)
            {
                if (rigidbody2d.velocity.y == 0)
                {
                    if (GetDistanceBetweenPlayer() > attackRange + (Index * 0.5f))
                    {
                        StopCoroutine("AttackCoroutine");
                        IsAttackCoroutinePlaying = false;
                        MoveToward();
                    }
                    else
                    {
                        if(!IsAttackCoroutinePlaying)
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
            IsAttackCoroutinePlaying = true;

            while(true)
            {
                yield return new WaitForSeconds(attackCoolTime);
                Attack();
            }
        }

        public void Attack()
        {
            PlayAttackAnimation();
            SpawnBullet();
        }

        private void SpawnBullet()
        {
            StartCoroutine("SpawnBulletCoroutine");
        }

        private IEnumerator SpawnBulletCoroutine()
        {
            yield return new WaitForSeconds(0.8f);
            Instantiate(PrefabManager.Instance.harpRadishealBullet, BulletSpawnPoint.transform.position, Quaternion.identity);
        }

        public void Hit(int damage)
        {
            Health -= damage;
            rigidbody2d.AddForce(new Vector2(100, 100));
            PlayHitAnimation();
        }

        public void Die()
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

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("HarpRadishealDie"))
                yield return new WaitForEndOfFrame();

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                yield return new WaitForEndOfFrame();

            MonsterManager.Instance.KillMonster(MonsterManager.Instance.GetKeyByValue(this));
        }

        public void ChangeSortingLayer(int bodyLayerNumber, int faceLayerNumber)
        {
            bodySpriteRenderer.sortingOrder = bodyLayerNumber;
            faceSpriteRenderer.sortingOrder = faceLayerNumber;
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