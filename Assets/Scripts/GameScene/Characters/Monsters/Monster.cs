using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public class Monster : MonoBehaviour
    {
        [HideInInspector]
        public int index;
        [HideInInspector]
        public bool isMoving = true;

        protected float attackCoolTime;
        protected float attackRange;
        protected float attackPower;
        protected float moveSpeed;
        protected float maxHealth;
        protected SpriteRenderer bodySpriteRenderer;
        protected SpriteRenderer faceSpriteRenderer;

        protected bool isAttackCoroutinePlaying;
        protected bool isDraggedForwardCoroutinePlaying;

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
            animator = transform.GetComponent<Animator>();
            // monster > canvas > healthbar_background > healthbar
            healthBar = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>();
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
                if(GameManager.Instance.MoveSpeed >= 3 && isDraggedForwardCoroutinePlaying == false)
                {
                    StopCoroutine("MoveBackwardCoroutine");
                    StartCoroutine("DraggedForwardCoroutine");
                }

                // 피격받아서 공중에 떠있지 않을 때
                if (rigidbody2d.velocity.y == 0)
                {
                    // 사정거리 밖이라 이동
                    if (GetDistanceBetweenPlayer() > attackRange + (index * 0.5f))
                    {
                        isMoving = true;
                        StopCoroutine("AttackCoroutine");
                        isAttackCoroutinePlaying = false;
                        MoveToward();
                    }
                    // 사정거리 안이라 멈춰서 공격
                    else
                    {
                        isMoving = false;
                        if (isAttackCoroutinePlaying == false)
                            StartCoroutine("AttackCoroutine");
                    }
                }

                else isMoving = false;

                yield return new WaitForSeconds(0.02f);
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

        public virtual void ChangeSortingLayer(int bodyLayerNumber, int faceLayerNumber)
        {
            bodySpriteRenderer.sortingOrder = bodyLayerNumber;
            faceSpriteRenderer.sortingOrder = faceLayerNumber;
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
            int result;
            GameManager.Instance.GainScore(ScoreType.KillMonster, out result);
            PlayScoreAnimation(result);

            StopCoroutine("MoveTowardCoroutine");
            StopCoroutine("MoveBackwardCoroutine");
            StopCoroutine("DraggedForwardCoroutine");
            StartCoroutine("DieCoroutine");
        }

        private IEnumerator DieCoroutine()
        {
            PlayDieAnimation();
            yield return new WaitForSeconds(0.2f);
            DropManager.Instance.GenerateCoin(transform.position);
            yield return new WaitForSeconds(0.1f);
            DropManager.Instance.GenerateDropMaterial(transform.position);

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(GetNameOfDieAnimation()))
                yield return new WaitForEndOfFrame();
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                yield return new WaitForEndOfFrame();

            MonsterManager.Instance.KillMonster(MonsterManager.Instance.GetKeyByValue(this));
        }

        private void PlayScoreAnimation(int score)
        {
            GameObject instance = Instantiate(PrefabManager.Instance.scoreText, Camera.main.WorldToScreenPoint(transform.position), Quaternion.identity, GameUI.Instance.transform);
            instance.GetComponent<Text>().text = string.Format("+ {0}", score);
        }

        IEnumerator DraggedForwardCoroutine()
        {
            isDraggedForwardCoroutinePlaying = true;
            int speed = (int)GameManager.Instance.MoveSpeed;

            while(ItemManager.Instance.IsSprinting == true)
            {
                speed = (int)GameManager.Instance.MoveSpeed;

                if (rigidbody2d.velocity.y == 0)
                {
                    transform.position += new Vector3(-moveSpeed * 3 * Time.deltaTime, 0, 0);

                    if (GetDistanceBetweenPlayer() < 1.4f)
                    {
                        Health -= 100;
                        rigidbody2d.AddForce(new Vector2(130, 170));
                        PlayHitAnimation();
                    }
                }

                yield return new WaitForSeconds(0.02f);
            }

            StartCoroutine("MoveBackwardCoroutine");
            isDraggedForwardCoroutinePlaying = false;
        }

        IEnumerator MoveBackwardCoroutine()
        {
            while (GetDistanceBetweenPlayer() < attackRange + (index * 0.5f))
            {
                transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                yield return new WaitForSeconds(0.02f);
            }
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