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
        private Animator animator;

        private void OnDestroy()
        {
            Instance = null;
        }

        protected virtual void Awake()
        {
            Instance = this;
            attackPower = 30;
            animator = GetComponent<Animator>();
        }

        public virtual void Attack(int chainNumber)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("PopinAttack"))
                animator.SetTrigger("StopAttack");

            animator.SetTrigger("StartAttack");
        }

        protected float GetDamage(int chainNumber)
        {
            return attackPower * chainNumber * (1 + chainNumber * 0.1f);
        }

        public virtual void GetMaterialMessage(string materialName)
        {
            Debug.Log("GetMaterialMessage: " + materialName);
        }
    }
}