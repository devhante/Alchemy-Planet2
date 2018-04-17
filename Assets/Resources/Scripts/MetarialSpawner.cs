using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetarialSpawner : MonoBehaviour
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
		RectTransform rt;

        // 배열 할당
        metarialNumbers = new Dictionary<string, int>();
        positions = new Vector3[count];
        objects = new Button[count];
        
        // metarialNumbers 초기화
        for(int i = 0; i < prefabs.Length; i++)
        {
            metarialNumbers.Add(prefabs[i].name.Substring(6, prefabs[i].name.Length - 6), 0);
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
			InstantiateMetarial(i);
        }
    }

    public int GetMetarialNumber(string name)
    {
        return metarialNumbers[name];
    }

	public void  DecreaseMetarialNumber(string name)
	{
		metarialNumbers[name]--;
	}

	private void InstantiateMetarial(int i)
	{
		int index = Random.Range(0, prefabs.Length);
		Metarial m;

		objects[i] = Instantiate(prefabs[index], transform);
		objects[i].transform.position = new Vector2(positions[i].x, positions[i].y + 130);
		m = objects[i].GetComponent<Metarial>();

		m.index = i;
		m.prefabName = prefabs[index].name.Substring(6, prefabs[index].name.Length - 6);
		objects[i].onClick.AddListener(m.Onclick);

		metarialNumbers[m.prefabName]++;
	}

	public IEnumerator ReInstantiateMetarial(int i)
	{
		Debug.Log("RE");
		yield return new WaitForSeconds(1.0f);
		InstantiateMetarial(i);
	}
}