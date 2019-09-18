using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.AlchemyScene
{
    public class SynthesizeInfoUI : MonoBehaviour
    {
        [SerializeField]
        private List<ItemCell> requiredItemCellList;
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

        private void Start()
        {
            countUpButton.onClick.AddListener(() => AddCount());
            countDownButton.onClick.AddListener(() => SubtractCount());
            cancelButton.onClick.AddListener(() => Cancel());
            OKButton.onClick.AddListener(() => Synthesize());

            SetFomula();
            if (!CanSynthesize())
                OKButton.interactable = false;
        }

        void SetFomula()
        {
            formula = AlchemyManager.Instance.formulaDictionary[SynthesizeManager.Instance.itemName];

            var materials = formula.formula;
            int materialCount = 0;

            count = 1;
            resultItemImage.sprite = DataManager.Instance.itemInfo[AlchemyManager.Instance.GetEnglishName(formula.result)].image;

            foreach (var material in materials)
            {
                requiredItemCellList[materialCount].gameObject.SetActive(true);
                requiredItemCellList[materialCount++].SetItemCell(material.Key, DataManager.Instance.itemInfo[AlchemyManager.Instance.GetEnglishName(material.Key)].image, material.Value);
            }

            if (materialCount > 2)
            {
                for (int i = 1; i < materialCount; i++)
                {
                    requiredItemCellList[i].gameObject.transform.RotateAround(resultItemImage.gameObject.transform.position, Vector3.back, 360 * i / materialCount);
                    requiredItemCellList[i].gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
                }
            }
            else
            {
                requiredItemCellList[0].gameObject.transform.RotateAround(resultItemImage.gameObject.transform.position, Vector3.back, 90);
                requiredItemCellList[0].gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
                requiredItemCellList[1].gameObject.transform.RotateAround(resultItemImage.gameObject.transform.position, Vector3.forward, 90);
                requiredItemCellList[1].gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        }

        void Synthesize()
        {
            foreach (var material in formula.formula)
            {
                DataManager.Instance.CurrentPlayerData.inventory[AlchemyManager.Instance.GetEnglishName(material.Key)] -= material.Value * count;

                if (DataManager.Instance.CurrentPlayerData.inventory[AlchemyManager.Instance.GetEnglishName(material.Key)] > 0)
                    BackendManager.Instance.UpdateItemNumber(BackendManager.Instance.GetInDate("item"), AlchemyManager.Instance.GetEnglishName(material.Key),
                        DataManager.Instance.CurrentPlayerData.inventory[AlchemyManager.Instance.GetEnglishName(material.Key)]);
                else
                    BackendManager.Instance.DeleteItem(BackendManager.Instance.GetInDate("item"), AlchemyManager.Instance.GetEnglishName(material.Key));
            }

            SynthesizeManager.Instance.OpenSynthesizeMiniGame(count);
        }

        bool CanSynthesize()
        {
            foreach (var material in formula.formula)
            {
                if (!DataManager.Instance.CurrentPlayerData.inventory.ContainsKey(AlchemyManager.Instance.GetEnglishName(material.Key)) ||
                    DataManager.Instance.CurrentPlayerData.inventory.ContainsKey(AlchemyManager.Instance.GetEnglishName(material.Key)) &&
                    DataManager.Instance.CurrentPlayerData.inventory[AlchemyManager.Instance.GetEnglishName(material.Key)] < material.Value * count)
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
                requiredItemCellList[materialCount].SetItemCell(material.Key, DataManager.Instance.itemInfo[AlchemyManager.Instance.GetEnglishName(material.Key)].image, material.Value * count);
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
                    requiredItemCellList[materialCount].SetItemCell(material.Key, DataManager.Instance.itemInfo[AlchemyManager.Instance.GetEnglishName(material.Key)].image, material.Value * count);
                    materialCount++;
                }
            }

            countNumber.text = count.ToString();
        }

        void Cancel()
        {
            SynthesizeManager.Instance.OpenSynthesizeSelectUI();
            Destroy(gameObject);
        }
    }
}