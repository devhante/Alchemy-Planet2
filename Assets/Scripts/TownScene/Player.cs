using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.TownScene
{
    public class Player : MonoBehaviour {

        public float speed;

        private Touch tempTouchs;
        private Vector3 touchedPos;
        private bool touchOn;


        // Use this for initialization
        void Start() {
            touchOn = false;
        }

        // Update is called once per frame
        void Update() {
            Click();
        }

        void Touch()
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

        void Click()
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

        IEnumerator Move(GameObject obj)
        {
            Debug.Log(transform.position.x - obj.transform.position.x);
            if (obj.tag == "NPC")
            {
                obj.SendMessage("Talk");
                // NPC 이동멈추기

                UIManager.Instance.OpenMenu<DialogUI>();
            }
            Debug.Log(transform.position.x - obj.transform.position.x);
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