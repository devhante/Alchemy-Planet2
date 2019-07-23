using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.AlchemyScene
{
    public class AlchemyManager : MonoBehaviour
    {
        public static AlchemyManager Instance;
        public List<Data.FormulaData> formulas;
        public List<Data.Request> requests;
        public Dictionary<string, string> itemEnglishName;
         

        private void Awake()
        {
            formulas = Data.DataManager.Instance.LoadFormulas();
            requests = Data.DataManager.Instance.LoadRequests();

            Instance = this;
        }

        private void Start()
        {
            itemEnglishName = new Dictionary<string, string>();

            foreach (var item in Data.DataManager.Instance.itemInfo)
            {
                itemEnglishName.Add(item.Value.item_name, item.Key);
            }
        }

        public string GetEnglishName(string koreanName)
        {
            if (itemEnglishName.ContainsKey(koreanName))
                return itemEnglishName[koreanName];

            return "찾을 수 없습니다.";
        }
    }
}