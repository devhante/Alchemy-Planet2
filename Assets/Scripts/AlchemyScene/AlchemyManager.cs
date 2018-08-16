using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.AlchemyScene
{
    public class AlchemyManager : MonoBehaviour
    {
        List<Formula> formulas;

        private void Awake()
        {
            formulas = DataManager.Instance.LoadFormulas();
        }

        private void Start()
        {
            Data.Material material;
            DataManager.Instance.materials.TryGetValue("Red", out material);
        }
    }
}