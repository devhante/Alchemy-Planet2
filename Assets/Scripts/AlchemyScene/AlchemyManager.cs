using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.AlchemyScene
{
    public class AlchemyManager : MonoBehaviour
    {
        List<Data.Formula> formulas;

        private void Awake()
        {
            formulas = Data.DataManager.Instance.LoadFormulas();
        }

        private void Start()
        {
            Data.Material material;
            Data.DataManager.Instance.materials.TryGetValue("Red", out material);
        }
    }
}