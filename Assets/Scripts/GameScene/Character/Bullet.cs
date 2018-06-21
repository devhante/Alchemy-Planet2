using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Bullet : MonoBehaviour
    {
        public int speed = 3;

        public int Damage { get; set; }

        private void Update()
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Monster")
                collision.GetComponent<Monster>().Hit(Damage);

            Destroy(gameObject);
        }
    }
}