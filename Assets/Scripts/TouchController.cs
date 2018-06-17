using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
	Touch touch;
	Vector3 touchPos;
	int touchCount;

	RaycastHit2D hit;

	public GameObject testPrefab;

	private void Start()
	{
		//if (Application.platform == RuntimePlatform.WindowsEditor)
		//{
		//	UpdateMouse();
		//}

		//else if (Application.platform == RuntimePlatform.Android)
		//{
		//	UpdateTouch();
		//}

		StartCoroutine("TouchCoroutine");
	}

	private void UpdateMouse()
	{

	}

	IEnumerator TouchCoroutine()
	{
		while (true)
		{
			touchCount = Input.touchCount;
			
			if(touchCount > 0)
			{
				for(int i = 0; i < touchCount; i++)
				{
					touch = Input.GetTouch(i);

					if(touch.phase == TouchPhase.Began)
					{
						touchPos = Camera.main.ScreenToWorldPoint(touch.position);
						hit = Physics2D.Raycast(touchPos, Vector2.zero);

						if(hit)
						{
							Destroy(hit.collider.gameObject);
						}
					}
				}
			}

			yield return null;
		}
	}
}