using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace AlchemyPlanet.GameScene
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        public int AttackPower { get; private set; }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            AttackPower = 30;
        }

        public void Attack(int chainNumber)
        {
            PlayAttackAnimation();

            if (MonsterManager.Instance.Monsters.Count > 0)
            {
                MonsterManager.Instance.Monsters.Values.First().Health -= (int)(AttackPower * chainNumber * (1 + chainNumber * 0.1));
            }
        }

        public void PlayAttackAnimation()
        {
            GetComponent<Animator>().SetTrigger("isAttacking");
        }
    }
}