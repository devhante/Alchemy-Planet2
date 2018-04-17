using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metarial : MonoBehaviour
{
	public int index;
	public string prefabName;

	public void Onclick()
	{
		string recipe = RecipeController.Instance.GetQueuePeekName();

		Debug.Log(recipe + " " + prefabName);
		if (recipe == prefabName) RecipeController.Instance.DestroyQueuePeek();

		MetarialSpawner.Instance.DecreaseMetarialNumber(prefabName);
		MetarialSpawner.Instance.StartCoroutine("ReInstantiateMetarial", index);
		Destroy(gameObject);
	}
}
