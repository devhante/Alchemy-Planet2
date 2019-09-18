using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.AlchemyScene
{
    public class SynthesizeSelectUI : MonoBehaviour
    {
        [SerializeField]
        private List<ItemCell> itemCellList;
        [SerializeField]
        private List<Button> pageSelectButtonList;
        [SerializeField]
        private List<Button> typeSelectButtonList;
        [SerializeField]
        private List<string> typeSelectNameList;


        private List<ItemData> itemList;
        private List<string> canSynthesizeItemList;
        private int itemButtonCount;
        private int selectedPage;

        private void Start()
        {
            selectedPage = 0;
            AddOnClick();

            itemButtonCount = itemCellList.Count;
            itemList = new List<ItemData>();
            canSynthesizeItemList = new List<string>();

            foreach (FormulaData formula in AlchemyManager.Instance.formulaDictionary.Values)
            {
                canSynthesizeItemList.Add(formula.result);
            }

            ChangeType("PurifyPotion");
        }

        private void GetItem(string itemTypeName)
        {
            ItemKind itemType;
            switch (itemTypeName)
            {
                case nameof(ItemKind.Abrasive):
                    itemType = ItemKind.Abrasive;
                    break;
                case nameof(ItemKind.ActivePotion):
                    itemType = ItemKind.ActivePotion;
                    break;
                case nameof(ItemKind.PurifyPotion):
                    itemType = ItemKind.PurifyPotion;
                    break;
                case nameof(ItemKind.Material):
                    itemType = ItemKind.Material;
                    break;
                case nameof(ItemKind.Event):
                    itemType = ItemKind.Event;
                    break;
                default:
                    return;
            }

            itemList.Clear();

            foreach (var item in DataManager.Instance.itemInfo.Values)
            {
                if (item.item_kind == itemType && canSynthesizeItemList.Contains(item.item_name))
                {
                    itemList.Add(item);
                }
            }

            int count = itemList.Count - itemButtonCount;

            for (int i = 1; count > itemButtonCount; i++)
            {
                count -= itemButtonCount;
                pageSelectButtonList[i].gameObject.SetActive(true);
            }
        }

        private void SetItemButton()
        {
            int buttonCount = 0;
            int itemCountInPage = itemList.Count - itemButtonCount * selectedPage;

            for (int i = 0; i < itemCountInPage && i < itemButtonCount; i++)
            {
                itemCellList[i].gameObject.SetActive(true);
                itemCellList[i].SetItemCell(itemList[i + itemButtonCount * selectedPage].item_name, itemList[i + itemButtonCount * selectedPage].image);
                buttonCount = i + 1;
            }

            for (int i = buttonCount; i < itemButtonCount; i++)
            {
                itemCellList[i].gameObject.SetActive(false);
            }
        }

        private void AddOnClick()
        {
            for (int i = 0; i < pageSelectButtonList.Count; i++)
            {
                int n = i;
                pageSelectButtonList[n].onClick.AddListener(() => ChangePage(n));
                itemCellList[n].gameObject.GetComponentInChildren<Button>().onClick.AddListener(() => ClickItem(itemCellList[n].itemName));
                typeSelectButtonList[n].onClick.AddListener(() => ChangeType(typeSelectNameList[n]));
            }
        }

        private void ClickItem(string itemName)
        {
            SynthesizeManager.Instance.OpenSynthesizeInfoUI(itemName);
            Destroy(gameObject);
        }

        private void ChangePage(int page)
        {
            if (page != selectedPage)
            {
                pageSelectButtonList[page].transform.Translate(-10, 0, 0);
                pageSelectButtonList[selectedPage].transform.Translate(10, 0, 0);
                selectedPage = page;
                SetItemButton();
            }
        }

        private void ChangeType(string itemType)
        {
            GetItem(itemType);
            itemList.Sort((ItemData i1, ItemData i2) => { return string.Compare(i1.item_name, i2.item_name); });
            SetItemButton();
        }
    }
}