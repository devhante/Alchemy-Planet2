using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.AlchemyScene
{
    public class AlchemyManager : MonoBehaviour
    {
        public static AlchemyManager Instance;
        public List<Data.Formula> formulas;
        public List<Data.Request> requests;

        private void Awake()
        {
            formulas = Data.DataManager.Instance.LoadFormulas();
            //requests = Data.DataManager.Instance.LoadRequests();

            Instance = this;
        }
    }
}