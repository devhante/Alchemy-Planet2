using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    static int speed = 3;
    Vector3 destination;

    private void Awake()
    {
        transform.position = new Vector2(760.0f, 0.0f + 732.0f);
        destination = new Vector3(36.0f, 0.0f + 732.0f);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * speed);
    }

    public void SetDestination(int index)
    {
        destination = new Vector3(36.0f + index * 50.0f, destination.y);
    }
}
