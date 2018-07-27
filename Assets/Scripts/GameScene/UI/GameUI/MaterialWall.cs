using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class MaterialWall : MonoBehaviour
    {
        public enum Position { Top, Bottom, Left, Right }
        [SerializeField]
        public Position position;

        //private void Awake()
        //{
        //    if (position == Position.Top || position == Position.Bottom)
        //        GetComponent<BoxCollider2D>().size = new Vector2(Screen.width, 10);
        //}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Bubble")
            {
                Vector3 direction = new Vector3();
                switch(position)
                {
                    case Position.Top:
                        direction = new Vector3(0, transform.position.x - collision.transform.position.x);
                        break;

                    case Position.Bottom:
                        direction = new Vector3(0, -(transform.position.x - collision.transform.position.x));
                        break;

                    case Position.Left:
                        direction = new Vector3(transform.position.y - collision.transform.position.y, 0);
                        break;

                    case Position.Right:
                        direction = new Vector3(-(transform.position.y - collision.transform.position.y), 0);
                        break;
                }
                Vector3 collisionDirection = collision.GetComponent<Bubble>().direction;
                collision.GetComponent<Bubble>().direction = Rotate(-collisionDirection, 2 * GetAngle(collisionDirection, direction));
            }
        }

        private float GetAngle(Vector3 vector1, Vector3 vector2)
        {
            float angle = (Mathf.Atan2(vector2.y, vector2.x) - Mathf.Atan2(vector1.y, vector1.x)) * Mathf.Rad2Deg;
            return angle;
        }

        private Vector3 Rotate(Vector3 point, float degree)
        {
            float radius = degree * Mathf.Deg2Rad;
            float sin = Mathf.Sin(radius);
            float cos = Mathf.Cos(radius);
            float posX = point.x * cos - point.y * sin;
            float posY = point.y * cos + point.x * sin;
            return new Vector3(posX, posY);
        }
    }
}