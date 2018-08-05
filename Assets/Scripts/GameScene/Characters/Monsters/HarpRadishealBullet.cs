using System.Collections;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class HarpRadishealBullet : MonoBehaviour
    {
        private int speed;
        private int damage;

        private Animator animator;

        private void Awake()
        {
            speed = 3;
            damage = 5;

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
                transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
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
                Player.Instance.Hit(damage);
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