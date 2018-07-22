using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.TownScene
{
    public class NPC : MonoBehaviour
    {

        public float speed;
        public float moveTime;

        private int moveChoice;         // 움직임
        private bool moving;            // 움직이는 중
        private bool talking = false;           // 말하는 중

        // Use this for initialization
        void Start()
        {
            moving = false;
            moveChoice = Random.Range(0, 3);
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private void Move()
        {
            if (!moving)
            {
                switch (moveChoice)
                {
                    case 0:
                        StartCoroutine("LeftMove");
                        break;
                    case 1:
                        StartCoroutine("RightMove");
                        break;
                    case 2:
                        StartCoroutine("StopMove");
                        break;
                }
            }
        }

        void stop()
        {
            if (moving)
            {
                switch (moveChoice)
                {
                    case 0:
                        StopCoroutine("LeftMove");
                        break;
                    case 1:
                        StopCoroutine("RightMove");
                        break;
                }
            }
        }

        void talk()
        {
            if (!talking)
            {
                talking = true;
                UIManager.Instance.OpenMenu<DialogUI>();
                talking = false;
            }

            moving = false;
        }

        private IEnumerator LeftMove()
        {
            moving = true;
            for (int i = 0; i < moveTime; i++)
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            moving = false;
            moveChoice = Random.Range(1, 3);
            yield return null;
        }

        private IEnumerator RightMove()
        {
            moving = true;
            for (int i = 0; i < moveTime; i++)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            moving = false;
            moveChoice = Random.Range(0, 2) == 1 ? 2 : 0;
            yield return null;
        }

        private IEnumerator StopMove()
        {
            moving = true;
            yield return new WaitForSeconds(2f);
            moving = false;
            moveChoice = Random.Range(0, 2);
            yield return null;
        }
    }
}