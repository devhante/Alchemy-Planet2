using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public static Queue<Monster> monsters = new Queue<Monster>();

    public GameObject[] prefabs;
    public List<GameObject> combo;

    private void Awake()
    {
        combo = GetRandomCombo(1);

        foreach(var item in combo)
            Instantiate(item, transform, false).transform.position += new Vector3(0, 1, 0);

        monsters.Enqueue(this);
    }

    private List<GameObject> GetRandomCombo(int length)
    {
        var list = new List<GameObject>();

        for (int i = 0; i < length; i++)
        {
            int index = Random.Range(0, prefabs.Length);
            list.Add(prefabs[index]);
        }

        return list;
    }
}
