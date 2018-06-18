using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public float speed;
    public float moveTime;

    private int moveChoice;         // 움직임
    private bool moving;            // 움직이는중
    
	// Use this for initialization
	void Start () {
        moving = false;
        moveChoice = Random.Range(0, 3);
    }
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    private void Move()
    {
        if (!moving)
        {
            switch (moveChoice)
            {
                case 0:
                    StartCoroutine("LeftMove");
                    break;
                case 1:
                    StartCoroutine("RightMove");
                    break;
                case 2:
                    StartCoroutine("StopMove");
                    break;
            }
        }
    }

    void Talk()
    {
        switch (moveChoice)
        {
            case 0:
                StopCoroutine("LeftMove");
                break;
            case 1:
                StopCoroutine("RightMove");
                break;
            case 2:
                StopCoroutine("StopMove");
                break;
        }
        moving = true;
    }

    IEnumerator LeftMove()
    {
        moving = true;
        for (int i=0; i<moveTime; i++)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        moving = false;
        moveChoice = Random.Range(1, 3);
        yield return null;
    }
    
    IEnumerator RightMove()
    {
        moving = true;
        for (int i = 0; i < moveTime; i++)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        moving = false;
        moveChoice = Random.Range(0, 2)==1?2:0;
        yield return null;
    }

    IEnumerator StopMove()
    {
        moving = true;
        yield return new WaitForSeconds(2f);
        moving = false;
        moveChoice = Random.Range(0, 2);
        yield return null;
    }
}
