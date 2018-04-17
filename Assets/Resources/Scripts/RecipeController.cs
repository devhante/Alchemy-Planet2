using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeController : MonoBehaviour
{
	public GameObject[] prefabs;

	GameObject[] objects;
	Queue<GameObject> queue = new Queue<GameObject>();

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
		StartCoroutine("MoveRecipe");
	}

	public string GetQueuePeekName()
	{
		return queue.Peek().name.Substring(5, queue.Peek().name.Length - 12);
	}

	public void DestroyQueuePeek()
	{
		Destroy(queue.Dequeue());
	}

	IEnumerator CreateRecipe()
	{
		int index;
		GameObject temp;

		while(true)
		{
			index = Random.Range(0, prefabs.Length);
			temp = Instantiate(prefabs[index], transform);

			queue.Enqueue(temp);
			temp.transform.position = new Vector2(730.0f, 0.0f + 732.0f);

			yield return new WaitForSeconds(1.0f);
		}
	}

	IEnumerator MoveRecipe()
	{
		Vector3 speed = new Vector3(60.0f, 0.0f);
		Vector3 target;

		while (true)
		{
			foreach (var item in queue)
			{
				target = item.transform.position - speed;
				item.transform.position = Vector3.Lerp(item.transform.position, target, Time.deltaTime);
			}

			if (queue.Peek().transform.position.x <= -30.0f) Destroy(queue.Dequeue());

			yield return null;
		}
	}
}
