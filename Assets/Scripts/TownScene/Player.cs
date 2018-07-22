using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{

    public float speed;         // 속도

    private Touch tempTouch;   // 터치들
    private Vector3 touchedPos; // 터치위치
    private Animator animator;  // 애니메이터

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectTouch();
        DetectClick();
    }

    void DetectTouch()    // 터치감지
    {
        if (Input.touchCount > 0)
        {
            tempTouch = Input.GetTouch(0);
            if (tempTouch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject() == false)
            {
                touchedPos = Camera.main.ScreenToWorldPoint(tempTouch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);
                if (hit.collider.tag == "Building" || hit.collider.tag == "Road" || hit.collider.tag == "NPC")
                {
                    StopCoroutine("Move");
                    StartCoroutine("Move", hit.collider.gameObject);
                }
            }
        }
    }

    void DetectClick()    // 클릭감지
    {
        if (Input.GetMouseButtonDown(0))
        {
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
        if (transform.position.x - obj.transform.position.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (transform.position.x - obj.transform.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (obj.tag == "NPC")
        {
            obj.SendMessage("stop");
            while (transform.position.x - obj.transform.position.x > 1.5f || transform.position.x - obj.transform.position.x < -1.5f)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            obj.SendMessage("talk");
        }
        else
        {
            while (transform.position.x - obj.transform.position.x > 0.1f || transform.position.x - obj.transform.position.x < -0.1f)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
        }
        animator.SetBool("Run", false);
        yield return null;
    }
}