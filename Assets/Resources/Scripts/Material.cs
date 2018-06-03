using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material : MonoBehaviour
{
	public int index;
	public string prefabName;

	public void Onclick()
	{
		string recipe = RecipeController.Instance.GetQueuePeekName();

		Debug.Log(recipe + " " + prefabName);
        if (recipe == prefabName)
        {
            RecipeController.Instance.DestroyQueuePeek();
        }

        foreach (var item in Monster.monsters)
        {
            if (item.combo[0].name.Substring(5) == prefabName)
            {
                Destroy(item.gameObject);
            }
        }

        MaterialSpawner.Instance.DecreasematerialNumber(prefabName);
		MaterialSpawner.Instance.StartCoroutine("ReInstantiatematerial", index);
		Destroy(gameObject);
	}
}
