using System.Collections;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Monster : MonoBehaviour
    {
        public float attackCoolTime;
        public float attackRange;
        public float moveSpeed;

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
            
        }
    }
}