using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class HarpRadishealBullet : MonoBehaviour
    {
        public int Speed { get; private set; }
        public int Damage { get; private set; }

        private Animator animator;

        private void Awake()
        {
            Speed = 3;
            Damage = 5;

            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            StartCoroutine("MoveCoroutine");
        }

        private IEnumerator MoveCoroutine()
        {
            while (true)
            {
                transform.position += new Vector3(-Speed * Time.deltaTime, 0, 0);
                yield return null;
            }
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                StopCoroutine("MoveCoroutine");
                GameUI.Instance.UpdateGage(Gages.PURIFY, -Damage);
                Explode();
            }
        }

        private void Explode()
        {
            StartCoroutine("ExplodeCoroutine");
        }

        private IEnumerator ExplodeCoroutine()
        {
            PlayExplodeAnimation();

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                yield return null;

            Destroy(gameObject);
        }

        private void PlayExplodeAnimation()
        {
            animator.SetTrigger("Explode");
        }
    }
}