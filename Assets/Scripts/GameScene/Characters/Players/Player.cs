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
        protected Animator animator;

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

        public virtual void Hit(float damage)
        {
            GameUI.Instance.UpdateGage(Gages.PURIFY, -damage);
            PlayHitAnimation();
        }

        protected float GetDamage(int chainNumber)
        {
            return attackPower * chainNumber * (1 + chainNumber * 0.1f);
        }

        public virtual void GetMaterialMessage(MaterialName materialName)
        {
            Debug.Log("GetMaterialMessage: " + materialName);
        }

        private void PlayHitAnimation()
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("PopinHit"))
                animator.SetTrigger("StopHit");

            animator.SetTrigger("StartHit");
        }
    }
}