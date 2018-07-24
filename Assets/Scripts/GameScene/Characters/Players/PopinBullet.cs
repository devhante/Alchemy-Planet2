using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class PopinBullet : MonoBehaviour
    {
        [HideInInspector]
        public float damage;
        private int speed;

        private void Awake()
        {
            speed = 5;
        }

        private void Update()
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }

        // 카메라 밖으로 나가면 파괴
        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Monster")
            {
                collision.GetComponent<Monster>().Hit(damage);
                Destroy(gameObject);
            }
        }
    }
}