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
        private Sprite failSprite;
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

        // Update is called once per frame
        void Update()
        {

        }

        void GetResult(int completionTime)
        {
            if (completionTime > 12)
            {
                greatProbability = 0;
                SetResult(false);
            }
            else if (completionTime > 9)
            {
                SetResult(true);
            }
            else if (completionTime > 7)
            {
                greatProbability += 3;
                SetResult(true);
            }
            else if (completionTime > 5)
            {
                greatProbability += 5;
                SetResult(true);
            }
            else
            {
                greatProbability += 10;
                SetResult(true);
            }
        }

        void SetResult(bool success)
        {
            itemCount = SynthesizeUI.Instance.itemCount;
            MeasureGreatProbability();
            if (success)
            {
                resultItemName.text = SynthesizeUI.Instance.itemName + " " + itemCount + " 개";
                resultItemImage.sprite = DataManager.Instance.itemInfo[GetEnglishName(SynthesizeUI.Instance.itemName)].image;

                if (DataManager.Instance.CurrentPlayerData.inventory.ContainsKey(GetEnglishName(SynthesizeUI.Instance.itemName)))
                {
                    DataManager.Instance.CurrentPlayerData.inventory[GetEnglishName(SynthesizeUI.Instance.itemName)] += itemCount;
                    BackendManager.Instance.UpdateItemNumber(BackendManager.Instance.GetInDate("item"), GetEnglishName(SynthesizeUI.Instance.itemName),
                        DataManager.Instance.CurrentPlayerData.inventory[GetEnglishName(SynthesizeUI.Instance.itemName)]);
                }
                else
                {
                    DataManager.Instance.CurrentPlayerData.inventory.Add(GetEnglishName(SynthesizeUI.Instance.itemName), itemCount);
                    BackendManager.Instance.AddItem(BackendManager.Instance.GetInDate("item"), GetEnglishName(SynthesizeUI.Instance.itemName),
                        DataManager.Instance.CurrentPlayerData.inventory[GetEnglishName(SynthesizeUI.Instance.itemName)]);
                }
            }
            else
            {
                resultItemName.text = "합성 실패";
                resultItemImage.sprite = failSprite;
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

        string GetEnglishName(string KoreanName)
        {
            foreach (var item in DataManager.Instance.itemInfo)
            {
                if (KoreanName == item.Value.item_name)
                    return item.Key;
            }

            return "찾을 수 없습니다.";
        }
    }
}