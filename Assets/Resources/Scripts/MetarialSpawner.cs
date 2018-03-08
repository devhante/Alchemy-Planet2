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
    Dictionary<string, int> metarialNumbers;

    static MetarialSpawner instance = null;

    public static MetarialSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(MetarialSpawner)) as MetarialSpawner;

                if(instance == null)
                {
                    Debug.Log("MetarialSpawner doesn't exist.");
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        // 변수 초기화
        Vector3 temp = Vector3.zero;
        bool isNotTooClose = true;

        // 배열 할당
        metarialNumbers = new Dictionary<string, int>();
        positions = new Vector3[count];
        objects = new GameObject[count];
        
        // metarialNumbers 초기화
        for(int i = 0; i < prefabs.Length; i++)
        {
            metarialNumbers.Add(prefabs[i].name, 0);
        }

        // positions 초기화
        for(int i = 0; i < count; i++)
        {
            isNotTooClose = true;
            temp.x = Random.Range(0.0f, x_max);
            temp.y = Random.Range(0.0f, y_max);

            for(int j = 0; j < i; j++)
            {
                if ((positions[j] - temp).sqrMagnitude < (minDistance * minDistance)) isNotTooClose = false;
            }

            if (isNotTooClose == true) positions[i] = temp;
            else i--;
        }

        // 인스턴스화
        for(int i = 0; i < count; i++)
        {
            int index = Random.Range(0, prefabs.Length);

            //Debug.Log(string.Format("count = {0}, i = {1}, positions[{1}] = {2}", count, i, positions[i]));
            objects[i] = Instantiate(prefabs[index], transform.position + positions[i], Quaternion.identity, transform);

            metarialNumbers[prefabs[index].name]++;
        }
    }

    public int GetMetarialNumber(string name)
    {
        return metarialNumbers[name];
    }
}