using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Bullet : MonoBehaviour
    {
        public int Damage { get; set; }
        public int Speed { get; private set; }

        private void Awake()
        {
            Speed = 5;
        }

        private void Update()
        {
            transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Monster")
            {
                collision.GetComponent<Monster>().Hit(Damage);
                Destroy(gameObject);
            }
        }
    }
}