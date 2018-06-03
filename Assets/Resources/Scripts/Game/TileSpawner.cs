using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public int speed = 1;

    Queue<GameObject> tileInstances = new Queue<GameObject>();
    int count = 0;

    private void Start()
    {
        tileInstances.Enqueue(Instantiate(tilePrefab, new Vector3(0.0f, transform.position.y), Quaternion.identity, transform));
        tileInstances.Enqueue(Instantiate(tilePrefab, new Vector3(6.0f, transform.position.y), Quaternion.identity, transform));
        tileInstances.Enqueue(Instantiate(tilePrefab, new Vector3(12.0f, transform.position.y), Quaternion.identity, transform));
    }

    private void Update()
    {
        foreach(GameObject item in tileInstances)
        {
            item.transform.position += new Vector3(-1 * speed * Time.deltaTime, 0.0f, 0.0f);
        }

        AddTile();
    }

    void AddTile()
    {
        if (tileInstances.Peek().transform.position.x <= -6.0f)
        {
            Destroy(tileInstances.Dequeue());
            tileInstances.Enqueue(Instantiate(tilePrefab, new Vector3(12.0f, transform.position.y), Quaternion.identity, transform));
        }
    }
}
