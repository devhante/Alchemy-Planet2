using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.TownScene
{
    public class Player : MonoBehaviour {

        public float speed;         // 속도

        private Touch tempTouchs;   // 터치들
        private Vector3 touchedPos; // 터치위치
        private bool touchOn;       // 터치유무


        // Use this for initialization
        void Start() {
            touchOn = false;
        }

        // Update is called once per frame
        void Update() {
            Click();
        }

        void Touch()    // 터치감지
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    tempTouchs = Input.GetTouch(i);
                    if (tempTouchs.phase == TouchPhase.Began)
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
                if (hit)
                {
                    if (hit.collider.tag == "Road" || hit.collider.tag == "NPC")
                    {
                        StopCoroutine("Move");
                        StartCoroutine("Move", hit.collider.gameObject);
                    }
                }
            }
        }

        IEnumerator Move(GameObject obj)    //캐릭터 움직이기
        {
            Debug.Log(transform.position.x - obj.transform.position.x);
            if (obj.tag == "NPC")
            {
                UIManager.Instance.OpenMenu<DialogUI>();
            }
            while (transform.position.x - obj.transform.position.x > 0.1f || transform.position.x - obj.transform.position.x < -0.1f)
            {
                if (transform.position.x - obj.transform.position.x < 0)
                {
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                    yield return new WaitForFixedUpdate();
                }
                else if (transform.position.x - obj.transform.position.x > 0)
                {
                    transform.Translate(Vector2.left * speed * Time.deltaTime);
                    yield return new WaitForFixedUpdate();
                }
            }
            yield return null;
        }
    }
}