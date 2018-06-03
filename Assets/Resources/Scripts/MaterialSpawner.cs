using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSpawner : MonoBehaviour
{
    public Button[] prefabs;

    int count = 13;
	float x_min = 70.0f;
    float x_max = 640.0f;
	float y_min = 80.0f;
    float y_max = 510.0f;
    float minDistance = 100.0f;

    Vector3[] positions;
    Button[] objects;
    Dictionary<string, int> materialNumbers;

    static MaterialSpawner instance = null;

    public static MaterialSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(MaterialSpawner)) as MaterialSpawner;

                if(instance == null)
                {
                    Debug.Log("materialSpawner doesn't exist.");
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
        materialNumbers = new Dictionary<string, int>();
        positions = new Vector3[count];
        objects = new Button[count];
        
        // materialNumbers 초기화
        for(int i = 0; i < prefabs.Length; i++)
        {
            materialNumbers.Add(prefabs[i].name.Substring(6, prefabs[i].name.Length - 6), 0);
        }

        // positions 초기화
        for(int i = 0; i < count; i++)
        {
            isNotTooClose = true;
            temp.x = Random.Range(x_min, x_max);
            temp.y = Random.Range(y_min, y_max);

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
			Instantiatematerial(i);
        }
    }

    public int GetmaterialNumber(string name)
    {
        return materialNumbers[name];
    }

	public void  DecreasematerialNumber(string name)
	{
		materialNumbers[name]--;
	}

	private void Instantiatematerial(int i)
	{
        int index;

        if (FindNoneMaterial(out index) == false)
            index = Random.Range(0, prefabs.Length);

		Material m;

		objects[i] = Instantiate(prefabs[index], transform);
		objects[i].transform.position = new Vector2(positions[i].x, positions[i].y + 130);
		m = objects[i].GetComponent<Material>();

		m.index = i;
		m.prefabName = prefabs[index].name.Substring(6, prefabs[index].name.Length - 6);
		objects[i].onClick.AddListener(m.Onclick);

		materialNumbers[m.prefabName]++;
	}

	public IEnumerator ReInstantiatematerial(int i)
	{
		Debug.Log("RE");
		yield return new WaitForSeconds(1.0f);
		Instantiatematerial(i);
	}

    private bool FindNoneMaterial(out int index)
    {
        index = 0;

        foreach(var item in materialNumbers)
        {
            if(item.Value <= 1)
            {
                return true;
            }

            index++;
        }

        return false;
    }
}