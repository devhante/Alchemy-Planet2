using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.TownScene
{
    public class Player : MonoBehaviour {

        public float speed;         // 속도

        private Touch tempTouchs;   // 터치들
        private Vector3 touchedPos; // 터치위치
        private bool touchOn;       // 터치유무
        private Animator animator;  // 애니메이터

        // Use this for initialization
        void Start() {
            touchOn = false;
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update() {
            Touch();
        }

        void Touch()    // 터치감지
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    tempTouchs = Input.GetTouch(i);
                    if (tempTouchs.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject() == false)
                    {
                        touchOn = true;
                        touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);
                        RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);
                        if (hit.collider.tag == "Road" || hit.collider.tag == "NPC")
                        {
                            StopCoroutine("Move");
                            StartCoroutine("Move", hit.collider.gameObject);
                        }
                        break;
                    }
                }
            }
        }

        void Click()    // 클릭감지
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchOn = true;
                touchedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);
                if (hit && EventSystem.current.IsPointerOverGameObject() == false)
                {
                    if (hit.collider.tag == "Road" || hit.collider.tag == "NPC" || hit.collider.tag == "Building")
                    {
                        StopCoroutine("Move");
                        StartCoroutine("Move", hit.collider.gameObject);
                    }
                }
            }
        }

        IEnumerator Move(GameObject obj)    //캐릭터 움직이기
        {
            animator.SetBool("Run", true);
            Debug.Log(transform.position.x - obj.transform.position.x);
            if (obj.tag == "NPC")
            {
                UIManager.Instance.OpenMenu<DialogUI>();
            }
            if (transform.position.x - obj.transform.position.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (transform.position.x - obj.transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            while (transform.position.x - obj.transform.position.x > 0.1f || transform.position.x - obj.transform.position.x < -0.1f)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            animator.SetBool("Run", false);
            yield return null;
        }
    }
}