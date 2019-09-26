using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.AlchemyScene
{
    public class SynthesizeResult : MonoBehaviour
    {
        [SerializeField]
        private Image resultItemImage;
        [SerializeField]
        private Text resultItemName;
        [SerializeField]
        private Text plusEXP;
        [SerializeField]
        private Image EXPImage;
        [SerializeField]
        private Text EXPText;
        [SerializeField]
        private Text levelText;
        [SerializeField]
        private Button OpenSynthesizeSelectUIButton;

        private int greatProbability;
        private int itemCount;
        private FormulaData formula;

        void Start()
        {
            formula = AlchemyManager.Instance.formulaDictionary[SynthesizeManager.Instance.itemName];
            greatProbability = 5;
            OpenSynthesizeSelectUIButton.onClick.AddListener(() =>
            {
                SynthesizeManager.Instance.OpenSynthesizeSelectUI();
                Destroy(gameObject);
            });
            SetResult();
        }

        void SetResult()
        {
            if(SynthesizeManager.Instance.result == SynthesizeManager.Result.fail){
                resultItemName.text = "실패";
                return;
            }

            itemCount = SynthesizeManager.Instance.itemCount;
            resultItemName.text = SynthesizeManager.Instance.itemName + " " + itemCount + " 개";
            resultItemImage.sprite = DataManager.Instance.itemInfo[AlchemyManager.Instance.GetEnglishName(SynthesizeManager.Instance.itemName)].image;

            foreach (var material in formula.formula)
            {
                DataManager.Instance.CurrentPlayerData.inventory[AlchemyManager.Instance.GetEnglishName(material.Key)] -= material.Value * SynthesizeManager.Instance.itemCount;

                if (DataManager.Instance.CurrentPlayerData.inventory[AlchemyManager.Instance.GetEnglishName(material.Key)] > 0)
                    BackendManager.Instance.UpdateItemNumber(BackendManager.Instance.GetInDate("item"), AlchemyManager.Instance.GetEnglishName(material.Key),
                        DataManager.Instance.CurrentPlayerData.inventory[AlchemyManager.Instance.GetEnglishName(material.Key)]);
                else
                    BackendManager.Instance.DeleteItem(BackendManager.Instance.GetInDate("item"), AlchemyManager.Instance.GetEnglishName(material.Key));
            }

            if (DataManager.Instance.CurrentPlayerData.inventory.ContainsKey(AlchemyManager.Instance.GetEnglishName(SynthesizeManager.Instance.itemName)))
            {
                DataManager.Instance.CurrentPlayerData.inventory[AlchemyManager.Instance.GetEnglishName(SynthesizeManager.Instance.itemName)] += itemCount;
                BackendManager.Instance.UpdateItemNumber(BackendManager.Instance.GetInDate("item"), AlchemyManager.Instance.GetEnglishName(SynthesizeManager.Instance.itemName),
                    DataManager.Instance.CurrentPlayerData.inventory[AlchemyManager.Instance.GetEnglishName(SynthesizeManager.Instance.itemName)]);
            }
            else
            {
                DataManager.Instance.CurrentPlayerData.inventory.Add(AlchemyManager.Instance.GetEnglishName(SynthesizeManager.Instance.itemName), itemCount);
                BackendManager.Instance.AddItem(BackendManager.Instance.GetInDate("item"), AlchemyManager.Instance.GetEnglishName(SynthesizeManager.Instance.itemName),
                    DataManager.Instance.CurrentPlayerData.inventory[AlchemyManager.Instance.GetEnglishName(SynthesizeManager.Instance.itemName)]);
            }
        }
    }
}