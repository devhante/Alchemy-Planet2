using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.TownScene
{
    public class NPC : MonoBehaviour
    {
        public NPCDAta data;
        public float speed;
        public float moveDistance;

        private int moveChoice;         // 움직임
        private bool moving;            // 움직이는 중
        private bool talking = false;   // 말하는 중
        private Animator animator;      // 애니메이터
        private GameObject player;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        // Use this for initialization
        void Start()
        {
            data = new NPCDAta(this.gameObject.name);

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
                moving = true;
                switch (moveChoice)
                {
                    case 0:
                        StartCoroutine("RightMove");
                        break;
                    case 1:
                        StartCoroutine("LeftMove");
                        break;
                    case 2:
                        StartCoroutine("StopMove");
                        break;
                }
            }
        }

        void Stop()
        {
            animator.SetBool("Run", false);
            StopAllCoroutines();
            moving = true;
        }

        void TalkStart(GameObject a)
        {
            player = a;
            if (!talking)
            {
                talking = true;
                UIManager.Instance.OpenMenu<DialogUI>();
                //클릭된 NPC의 이름으로 수정해야 함
                DialogUI.Instance.SetDialog(this);
            }
        }

        void TalkEnd()
        {
            talking = false;
            moveChoice = 2;
            player.SendMessage("TalkEnd");
            StartCoroutine("StopMove");
        }
        
        private IEnumerator RightMove()
        {
            float firstPosX = transform.position.x;
            animator.SetBool("Run", true);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            while (firstPosX + moveDistance > transform.position.x)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                RaycastHit2D wall = Physics2D.Raycast(transform.position, Vector2.right, 1.5f, LayerMask.GetMask("Wall"));
                if(wall.collider != null)
                {
                    moveChoice = 2;
                    moving = true;
                    animator.SetBool("Run", false);
                    StartCoroutine("StopMove");
                    yield break;
                }
                yield return null;
            }
            animator.SetBool("Run", false);
            moving = false;
            moveChoice = 2;
            yield return null;
        }

        private IEnumerator LeftMove()
        {
            float firstPosX = transform.position.x;
            animator.SetBool("Run", true);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            while (firstPosX - moveDistance < transform.position.x)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                yield return null;
                RaycastHit2D wall = Physics2D.Raycast(transform.position, Vector2.left, 1.5f, LayerMask.GetMask("Wall"));
                if (wall.collider != null)
                {
                    moveChoice = 2;
                    moving = true;
                    animator.SetBool("Run", false);
                    StartCoroutine("StopMove");
                    yield break;
                }
            }
            animator.SetBool("Run", false);
            moving = false;
            moveChoice = 2;
            yield return null;
        }

        private IEnumerator StopMove()
        {
            animator.SetBool("Run", false);
            yield return new WaitForSeconds(2f);
            moving = false;
            moveChoice = Random.Range(0, 2);
            yield return null;
        }
    }
}