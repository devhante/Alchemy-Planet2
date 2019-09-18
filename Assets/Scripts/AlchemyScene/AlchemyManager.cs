using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.AlchemyScene
{
    public class AlchemyManager : MonoBehaviour
    {
        public static AlchemyManager Instance;
        public Dictionary<string, FormulaData> formulaDictionary { get; private set; }
        public List<Request> requests;
        public Dictionary<string, string> itemEnglishName;
         

        private void Awake()
        {
            formulaDictionary = new Dictionary<string, FormulaData>();
            DataManager.Instance.LoadFormulas().ForEach((formula) => { formulaDictionary.Add(formula.result, formula); });
            requests = DataManager.Instance.LoadRequests();

            Instance = this;
        }

        private void Start()
        {
            itemEnglishName = new Dictionary<string, string>();

            foreach (var item in DataManager.Instance.itemInfo)
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