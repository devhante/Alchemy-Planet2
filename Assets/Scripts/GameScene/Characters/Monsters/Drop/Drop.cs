using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Drop : MonoBehaviour
    {
        private float speed;
        private Rigidbody2D rigidbody2d;

        private void Awake()
        {
            speed = 3;
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Draw();
        }

        private void Draw()
        {
            StartCoroutine(DrawCoroutine());
        }

        private IEnumerator DrawCoroutine()
        {
            yield return new WaitForSeconds(0.5f);

            while (rigidbody2d.velocity.y != 0)
                yield return null;

            rigidbody2d.isKinematic = true;
            GetComponent<CircleCollider2D>().enabled = false;
            while (Vector3.Distance(transform.position, DropManager.Instance.DropDestination) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, DropManager.Instance.DropDestination, Time.deltaTime * speed);
                yield return null;
            }

            Gain();

            Destroy(gameObject);
        }

        protected virtual void Gain()
        {

        }
    }
}