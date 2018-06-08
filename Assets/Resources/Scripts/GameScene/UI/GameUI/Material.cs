using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Material : MonoBehaviour
    {
        public string materialName;

        public void Onclick()
        {
            string recipe = RecipeManager.Instance.GetQueuePeekName();

            if (recipe == materialName)
                RecipeManager.Instance.DestroyQueuePeek();

            MaterialManager.Instance.DecreaseMaterialNumber(materialName);
            MaterialManager.Instance.StartCoroutine("ReInstantiatematerial", transform.position);
            Destroy(gameObject);
        }
    }
}