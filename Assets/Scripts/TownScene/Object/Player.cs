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
            DetectTouch();
        }

        void DetectTouch()    // 클릭감지
        {
            if (Input.touchCount == 1 && !talking)
            {
                tempTouch = Input.GetTouch(0);
                if (tempTouch.phase != TouchPhase.Ended && !EventSystem.current.IsPointerOverGameObject(tempTouch.fingerId))
                {
                    touchedPos = Camera.main.ScreenToWorldPoint(tempTouch.position);
                    RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero, 0, LayerMask.GetMask("NPC"));

                    if (tempTouch.phase == TouchPhase.Began && hit.collider != null && hit.collider.tag == "NPC")
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
                        if (transform.position.x - touchedPos.x < 0)
                        {
                            RaycastHit2D wall = Physics2D.Raycast(transform.position, Vector2.right, 1, LayerMask.GetMask("Wall"));
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            if (wall.collider == null)
                                transform.Translate(Vector2.right * speed * Time.deltaTime);
                        }
                        else if (transform.position.x - touchedPos.x > 0)
                        {
                            RaycastHit2D wall = Physics2D.Raycast(transform.position, Vector2.left, 1, LayerMask.GetMask("Wall"));
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                            if (wall.collider == null)
                                transform.Translate(Vector2.right * speed * Time.deltaTime);
                        }
                        animator.SetBool("Run", true);
                    }
                }
                if (tempTouch.phase == TouchPhase.Ended || EventSystem.current.IsPointerOverGameObject(tempTouch.fingerId))
                {
                    animator.SetBool("Run", false);
                }
            }
            else if (Input.GetMouseButton(0) && !talking && !EventSystem.current.IsPointerOverGameObject())
            {
                touchedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero, 0, LayerMask.GetMask("NPC"));

                if (hit.collider != null)
                    Debug.Log(hit.collider.name);
                if (Input.GetMouseButtonDown(0) && hit.collider != null && hit.collider.tag == "NPC")
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
                        RaycastHit2D wall = Physics2D.Raycast(transform.position, Vector2.right, 1.5f, LayerMask.GetMask("Wall"));
                        if (wall.collider == null)
                            transform.Translate(Vector2.right * speed * Time.deltaTime);
                    }
                    else if (transform.position.x - touchedPos.x > 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        RaycastHit2D wall = Physics2D.Raycast(transform.position, Vector2.left, 1.5f, LayerMask.GetMask("Wall"));
                        if (wall.collider == null)
                            transform.Translate(Vector2.right * speed * Time.deltaTime);
                    }
                }
            }
            if (Input.GetMouseButtonUp(0) || EventSystem.current.IsPointerOverGameObject())
            {
                animator.SetBool("Run", false);
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