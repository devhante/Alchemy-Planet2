using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureCollision : MonoBehaviour {

    private bool isCollide;
    
    public bool CheckCollision()
    {
        return isCollide;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building" && collision.gameObject.layer == gameObject.layer)
        {
            isCollide = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building" && collision.gameObject.layer == gameObject.layer)
        {
            isCollide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building" && collision.gameObject.layer == gameObject.layer)
        {
            isCollide = false;
        }
    }
}
