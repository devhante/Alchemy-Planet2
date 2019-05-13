using AlchemyPlanet.Common;
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
            if(TouchManager.Instance != null)
            {
                if(TouchManager.Instance.IsMoving == true)
                {
                    if (TouchLock == false)
                    {
                        Run();
                    }
                }
                else
                {
                    animator.SetBool("Run", false);
                    TouchLock = false;
                }
            }
        }

        private void Run()
        {
            if(TouchManager.Instance != null && TouchManager.Instance.Joystick != null)
            {
                animator.SetBool("Run", true);
                if (TouchManager.Instance.Joystick.Normal.x > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    RaycastHit2D wall = Physics2D.Raycast(transform.position, Vector2.right, 1.5f, LayerMask.GetMask("Wall"));
                    if (wall.collider == null)
                        transform.Translate(Vector2.right * speed * Time.deltaTime);
                }
                else if (TouchManager.Instance.Joystick.Normal.x < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    RaycastHit2D wall = Physics2D.Raycast(transform.position, Vector2.left, 1.5f, LayerMask.GetMask("Wall"));
                    if (wall.collider == null)
                        transform.Translate(Vector2.right * speed * Time.deltaTime);
                }
            }
        }

        private void Interact(RaycastHit2D hit)
        {
            PrologueObject obj = hit.transform.GetComponent<PrologueObject>();
            obj.ActiveObject(gameObject);
            StartCoroutine(DeactiveObject_WithDelay(obj));
        }

        public IEnumerator DeactiveObject_WithDelay(PrologueObject obj)
        {
            yield return new WaitForSeconds(4);
            obj.DeactiveObject();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Guide"))
            {
                TouchLock = true;
                animator.SetBool("Run", false);
                Destroy(TouchManager.Instance.Joystick.transform.parent.gameObject);
                Destroy(collision.gameObject);
            }
        }
    }
}