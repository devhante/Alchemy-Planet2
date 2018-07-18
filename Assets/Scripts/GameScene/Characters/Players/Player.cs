using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace AlchemyPlanet.GameScene
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        protected float attackPower;

        private void OnDestroy()
        {
            Instance = null;
        }

        protected virtual void Awake()
        {
            Instance = this;
            attackPower = 30;
        }

        public virtual void Attack(int chainNumber)
        {
            PlayAttackAnimation();
        }

        protected float GetDamage(int chainNumber)
        {
            return attackPower * chainNumber * (1 + chainNumber * 0.1f);
        }

        private void PlayAttackAnimation()
        {
            GetComponent<Animator>().SetTrigger("Attack");
        }
    }
}