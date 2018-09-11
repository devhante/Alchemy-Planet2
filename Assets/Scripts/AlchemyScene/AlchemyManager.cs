using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.AlchemyScene
{
    public class AlchemyManager : MonoBehaviour
    {
        public static AlchemyManager Instance;
        public List<Data.Formula> formulas;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            formulas = Data.DataManager.Instance.LoadFormulas();
        }

        private void Start()
        {
            Data.Material material;
            Data.DataManager.Instance.materials.TryGetValue("Red", out material);
        }
    }
}