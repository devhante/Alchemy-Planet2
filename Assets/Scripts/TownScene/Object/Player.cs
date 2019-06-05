using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.TownScene
{
    public class Player : MonoBehaviour
    {
        public float speed;         // 속도

        private Touch tempTouch;    // 터치들
        private Vector3 touchedPos; // 터치위치
        private Animator animator;  // 애니메이터
        private bool talking;       // 대화중

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            animator.SetBool("Run", false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Interact(RaycastHit2D hit)
        {
            if (hit.collider.tag == "NPC")
            {
                if (TownUI.Instance.turnOnBuildBar)
                {
                    TownUI.Instance.StartCoroutine("MoveBar");
                }
                animator.SetBool("Run", false);
                talking = true;
                hit.collider.gameObject.SendMessage("Stop");
                hit.collider.gameObject.SendMessage("TalkStart", gameObject);
            }
        }

        void Move(Vector2 direction)
        {
            if (direction.x < 0) {
                direction.x *= -1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
                transform.rotation = Quaternion.Euler(0, 0, 0);
            if (((Vector2)transform.position + direction * speed * Time.deltaTime).y > Data.GameManager.Instance.floorY ||
                ((Vector2)transform.position + direction * speed * Time.deltaTime).y < -Data.GameManager.Instance.floorY)
            {
                direction.y = 0;
            }
            transform.Translate(direction * speed * Time.deltaTime);
            animator.SetBool("Run", true);
        }

        void Stop()
        {
            animator.SetBool("Run", false);
        }

        IEnumerator TalkEnd()
        {
            yield return new WaitForSeconds(0.2f);
            talking = false;
            yield return null;
        }
    }
}