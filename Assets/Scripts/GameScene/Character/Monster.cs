using System.Collections;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Monster : MonoBehaviour
    {
        public float attackCoolTime;
        public float attackRange;
        public float attackPower;
        public float moveSpeed;
        public int maxHealth;

        private int health;
        public int Health
        {
            get { return health; }
            set
            {
                health = Mathf.Clamp(value, 0, maxHealth);
                if (health == 0) MonsterManager.Instance.KillMonster(MonsterManager.Instance.GetKeyByValue(this));
            }
        }

        private void Awake()
        {
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

        float GetDistanceBetweenPlayer()
        {
            return transform.position.x - Player.Instance.transform.position.x;
        }

        void MoveToward()
        {
            transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
        }

        public void Attack()
        {
            GameUI.Instance.UpdateGage(Gages.PURIFY, -attackPower);
        }
    }
}