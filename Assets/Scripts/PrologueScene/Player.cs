using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.PrologueScene
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Vector3 touchedPos;
        private Animator animator;

        private bool TouchLock = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            animator.SetBool("Run", false);
        }
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                TouchLock = false;
            }
            if(!TouchLock)
                DetectTouch();
        }
        void DetectTouch()
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                touchedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            if (Input.GetMouseButtonUp(0) || EventSystem.current.IsPointerOverGameObject())
            {
                animator.SetBool("Run", false);
            }
        }
        /*
        public IEnumerator WaitForMouseUp()
        {
            while (! Input.GetMouseButtonUp(0))
            {
                yield return new WaitForEndOfFrame();
            }
            TouchLock = false;
        }
        */
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("NPC"))
            {
                TouchLock = true;
                //StopCoroutine("WaitForMouseUp");
                //StartCoroutine("WaitForMouseUp");
                transform.rotation = Quaternion.Euler(0, 180, 0);
                animator.SetBool("Run", false);
            }
            if (collision.CompareTag("Guide"))
            {
                TouchLock = true;
                animator.SetBool("Run", false);
            }
        }
    }
}