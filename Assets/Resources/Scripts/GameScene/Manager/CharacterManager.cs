using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance { get; private set; }

        public GameObject player;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
        }

        public void Attack()
        {
            PlayPlayerAttackAnimation();
        }

        public void PlayPlayerAttackAnimation()
        {
            player.GetComponent<Animator>().SetTrigger("isAttacking");
        }
    }
}