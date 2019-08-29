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

        // Use this for initialization
        void Start()
        {
            greatProbability = 5;
            OpenSynthesizeSelectUIButton.onClick.AddListener(() => SynthesizeUI.Instance.OpenSynthesizeSelectUI());
        }

        void SetResult(int greatProbability)
        {
            this.greatProbability = greatProbability;
            itemCount = SynthesizeUI.Instance.itemCount;
            MeasureGreatProbability();
            resultItemName.text = SynthesizeUI.Instance.itemName + " " + itemCount + " 개";
            resultItemImage.sprite = DataManager.Instance.itemInfo[AlchemyManager.Instance.GetEnglishName(SynthesizeUI.Instance.itemName)].image;

            if (DataManager.Instance.CurrentPlayerData.inventory.ContainsKey(AlchemyManager.Instance.GetEnglishName(SynthesizeUI.Instance.itemName)))
            {
                DataManager.Instance.CurrentPlayerData.inventory[AlchemyManager.Instance.GetEnglishName(SynthesizeUI.Instance.itemName)] += itemCount;
                BackendManager.Instance.UpdateItemNumber(BackendManager.Instance.GetInDate("item"), AlchemyManager.Instance.GetEnglishName(SynthesizeUI.Instance.itemName),
                    DataManager.Instance.CurrentPlayerData.inventory[AlchemyManager.Instance.GetEnglishName(SynthesizeUI.Instance.itemName)]);
            }
            else
            {
                DataManager.Instance.CurrentPlayerData.inventory.Add(AlchemyManager.Instance.GetEnglishName(SynthesizeUI.Instance.itemName), itemCount);
                BackendManager.Instance.AddItem(BackendManager.Instance.GetInDate("item"), AlchemyManager.Instance.GetEnglishName(SynthesizeUI.Instance.itemName),
                    DataManager.Instance.CurrentPlayerData.inventory[AlchemyManager.Instance.GetEnglishName(SynthesizeUI.Instance.itemName)]);
            }
        }

        void MeasureGreatProbability()
        {
            int random = Random.Range(0, 100);
            if (random < greatProbability)
            {
                if (SynthesizeUI.Instance.itemCount < 5)
                {
                    itemCount++;
                }
                else
                {
                    itemCount += SynthesizeUI.Instance.itemCount / 5;
                }
            }
        }
    }
}