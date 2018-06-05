using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeController : MonoBehaviour
{
	public GameObject[] prefabs;

	GameObject[] objects;
	Queue<Recipe> queue = new Queue<Recipe>();

	static RecipeController instance = null;

	public static RecipeController Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType(typeof(RecipeController)) as RecipeController;

				if (instance == null)
				{
					Debug.Log("RecipeController doesn't exist.");
				}
			}

			return instance;
		}
	}

	private void Start()
	{
        StartCoroutine("CreateRecipe");
	}

	public string GetQueuePeekName()
	{
		return queue.Peek().name.Substring(6, queue.Peek().name.Length - 13);
	}

	public void DestroyQueuePeek()
	{
        GameMenu.Instance.UpdateGage(Gages.PURIFY, 5);
        Destroy(queue.Dequeue().gameObject);

        int index = 0;
        foreach(var item in queue)
        {
            item.SetDestination(index);
            index++;
        }
	}

    private void AddRecipe()
    {
        int index = Random.Range(0, prefabs.Length);
        Recipe temp = Instantiate(prefabs[index], transform).GetComponent<Recipe>();
        queue.Enqueue(temp);
        temp.SetDestination(queue.Count - 1);
    }

    IEnumerator CreateRecipe()
    {
        while (true)
        {
            if (queue.Count < 14) AddRecipe();
            yield return new WaitForSeconds(0.1f);
        }
    }

	//IEnumerator CreateRecipe()
	//{
	//	int index;
	//	GameObject temp;

	//	while(true)
	//	{
	//		index = Random.Range(0, prefabs.Length);
	//		temp = Instantiate(prefabs[index], transform);

	//		queue.Enqueue(temp);
	//		temp.transform.position = new Vector2(730.0f, 0.0f + 732.0f);

	//		yield return new WaitForSeconds(1.0f);
	//	}
	//}

	//IEnumerator MoveRecipe()
	//{
	//	Vector3 speed = new Vector3(60.0f, 0.0f);
	//	Vector3 target;

	//	while (true)
	//	{
	//		foreach (var item in queue)
	//		{
	//			target = item.transform.position - speed;
	//			item.transform.position = Vector3.Lerp(item.transform.position, target, Time.deltaTime);
	//		}

	//		if (queue.Peek().transform.position.x <= -30.0f) Destroy(queue.Dequeue());

	//		yield return null;
	//	}
	//}
}
