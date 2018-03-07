using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetarialSpawner : MonoBehaviour
{
    public GameObject[] prefabs;

    int count = 13;
    float x_max = 6.0f;
    float y_max = 5.0f;
    float minDistance = 1.0f;

    Vector3[] positions;
    GameObject[] objects;

    private void Awake()
    {
        Vector3 temp = Vector3.zero;
        bool isNotTooClose = true;

        positions = new Vector3[count];
        objects = new GameObject[count];

        // positions 초기화
        for(int i = 0; i < count; i++)
        {
            isNotTooClose = true;
            temp.x = Random.Range(0.0f, x_max);
            temp.y = Random.Range(0.0f, y_max);

            for(int j = 0; j < i; j++)
            {
                if ((positions[j] - temp).sqrMagnitude < 1) isNotTooClose = false;
            }

            if (isNotTooClose == true) positions[i] = temp;
            else i--;
        }

        // 인스턴스화
        for(int i = 0; i < count; i++)
        {
            Debug.Log("count = " + count + ", i = " + i +  " // positions[" + i + "] = " + positions[i]);
            objects[i] = Instantiate(prefabs[0], transform.position + positions[i], Quaternion.identity, transform);
        }
    }
}