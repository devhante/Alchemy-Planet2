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
        }

        // Update is called once per frame
        void Update()
        {
            DetectTouch();
        }

        void DetectTouch()    // 클릭감지
        {
            if (Input.touchCount > 0 && !talking)
            {
                tempTouch = Input.GetTouch(0);
                if (tempTouch.phase != TouchPhase.Ended && !EventSystem.current.IsPointerOverGameObject(tempTouch.fingerId))
                {
                    touchedPos = Camera.main.ScreenToWorldPoint(tempTouch.position);
                    RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);
                    if (hit.collider != null && hit.collider.tag == "NPC")
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
                    else
                    {
                        animator.SetBool("Run", true);
                        if (transform.position.x - touchedPos.x < 0)
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if (transform.position.x - touchedPos.x > 0)
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                        transform.Translate(Vector2.right * speed * Time.deltaTime);
                    }
                }
                if (tempTouch.phase == TouchPhase.Ended)
                {
                    animator.SetBool("Run", false);
                }
            }
        }

        IEnumerator TalkEnd()
        {
            yield return new WaitForSeconds(0.2f);
            talking = false;
            yield return null;
        }
    }
}