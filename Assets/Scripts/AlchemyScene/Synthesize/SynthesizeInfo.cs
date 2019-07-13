using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.AlchemyScene
{
    public class SynthesizeInfo : MonoBehaviour
    {
        [SerializeField]
        private List<Image> requiredItemImageList;
        [SerializeField]
        private List<Image> requiredItemBackgroundImageList;
        [SerializeField]
        private List<Text> requiredItemCountList;
        [SerializeField]
        private Image resultItemImage;
        [SerializeField]
        private Button countUpButton;
        [SerializeField]
        private Button countDownButton;
        [SerializeField]
        private Text countNumber;
        [SerializeField]
        private Text bigHitProbability;
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private Button OKButton;

        private FormulaData formula;
        private int count;

        // Use this for initialization
        void Start()
        {
            AddOnClick();
        }

        void GetFomula(string itemName)
        {
            foreach (var f in AlchemyManager.Instance.formulas)
            {
                if (f.result == itemName)
                {
                    formula = f;
                    break;
                }
            }

            SetFomula();
        }

        void AddOnClick()
        {
            countUpButton.onClick.AddListener(() => AddCount());
            countDownButton.onClick.AddListener(() => SubtractCount());
            cancelButton.onClick.AddListener(() => Cancel());
            OKButton.onClick.AddListener(() => Synthesize());
        }

        void SetFomula()
        {
            var materials = formula.formula;
            int materialCount = 0;

            count = 1;
            resultItemImage.sprite = DataManager.Instance.itemInfo[GetEnglishName(formula.result)].image;

            foreach (var material in materials)
            {
                requiredItemBackgroundImageList[materialCount].gameObject.SetActive(true);
                requiredItemImageList[materialCount].gameObject.SetActive(true);
                requiredItemCountList[materialCount].gameObject.SetActive(true);

                requiredItemCountList[materialCount].text = material.Value.ToString();
                requiredItemImageList[materialCount].sprite = DataManager.Instance.itemInfo[GetEnglishName(material.Key)].image;
                materialCount++;
            }

            for (int i = materialCount; i < 5; i++)
            {
                requiredItemBackgroundImageList[i].gameObject.SetActive(false);
                requiredItemImageList[i].gameObject.SetActive(false);
                requiredItemCountList[i].gameObject.SetActive(false);
            }
        }

        void Synthesize()
        {
            if (!CanSynthesize())
                return;

            foreach (var material in formula.formula)
            {
                DataManager.Instance.CurrentPlayerData.inventory[GetEnglishName(material.Key)] -= material.Value * count;

                if (DataManager.Instance.CurrentPlayerData.inventory[GetEnglishName(material.Key)] > 0)
                    BackendManager.Instance.UpdateItemNumber(BackendManager.Instance.GetInDate("item"), GetEnglishName(material.Key), 
                        DataManager.Instance.CurrentPlayerData.inventory[GetEnglishName(material.Key)]);
                else
                    BackendManager.Instance.DeleteItem(BackendManager.Instance.GetInDate("item"), GetEnglishName(material.Key));
            }

            SynthesizeUI.Instance.OpenSynthesizeMiniGame(count);
        }

        bool CanSynthesize()
        {
            foreach (var material in formula.formula)
            {
                if (!DataManager.Instance.CurrentPlayerData.inventory.ContainsKey(GetEnglishName(material.Key)) || 
                    DataManager.Instance.CurrentPlayerData.inventory.ContainsKey(GetEnglishName(material.Key)) && 
                    DataManager.Instance.CurrentPlayerData.inventory[GetEnglishName(material.Key)] < material.Value * count)
                {
                    return false;
                }
            }

            return true;
        }

        void AddCount()
        {
            var materials = formula.formula;
            int materialCount = 0;

            count++;

            foreach (var material in materials)
            {
                requiredItemCountList[materialCount].text = (material.Value * count).ToString();
                materialCount++;
            }

            countNumber.text = count.ToString();
        }

        void SubtractCount()
        {
            if (count > 1)
            {
                var materials = formula.formula;
                int materialCount = 0;

                count--;

                foreach (var material in materials)
                {
                    requiredItemCountList[materialCount].text = (material.Value * count).ToString();
                    materialCount++;
                }
            }

            countNumber.text = count.ToString();
        }

        void Cancel()
        {
            SynthesizeUI.Instance.OpenSynthesizeSelectUI();
        }

        string GetEnglishName(string KoreanName)
        {
            foreach(var item in DataManager.Instance.itemInfo)
            {
                if(KoreanName == item.Value.item_name)
                    return item.Key;
            }

            return "찾을 수 없습니다.";
        }
    }
}