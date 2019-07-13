using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.AlchemyScene
{
    public class ItemPage : MonoBehaviour
    {
        [SerializeField]
        private List<Button> itemButtonList;
        [SerializeField]
        private List<Image> itemBackgroundList;
        [SerializeField]
        private List<Text> itemTextList;
        [SerializeField]
        private List<Button> pageSelectButtonList;
        [SerializeField]
        private Button sortButton;

        private List<ItemData> itemList;
        private List<string> canSynthesizeItemList;
        private int itemButtonCount;
        private int selectedPage;

        // Use this for initialization
        private void Start()
        {
            selectedPage = 0;
            AddOnClick();

            itemButtonCount = itemButtonList.Count;
            itemList = new List<ItemData>();
            canSynthesizeItemList = new List<string>();

            foreach(FormulaData formula in AlchemyManager.Instance.formulas)
            {
                canSynthesizeItemList.Add(formula.result);
            }

            ChangeType("PurifyPotion");
        }

        private void GetItem(string itemTypeName)
        {
            ItemKind itemType;
            if (itemTypeName == "Abrasive")
                itemType = ItemKind.Abrasive;
            else if (itemTypeName == "ActivePotion")
                itemType = ItemKind.ActivePotion;
            else if (itemTypeName == "PurifyPotion")
                itemType = ItemKind.PurifyPotion;
            else if (itemTypeName == "Material")
                itemType = ItemKind.Material;
            else if (itemTypeName == "Event")
                itemType = ItemKind.Event;
            else
                return;

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
                itemButtonList[i].gameObject.SetActive(true);
                itemBackgroundList[i].gameObject.SetActive(true);
                itemTextList[i].gameObject.SetActive(true);

                itemTextList[i].text = itemList[i + itemButtonCount * selectedPage].item_name;
                itemButtonList[i].image.sprite = itemList[i + itemButtonCount * selectedPage].image;
                buttonCount = i + 1;
            }

            for (int i = buttonCount; i < itemButtonCount; i++)
            {
                itemButtonList[i].gameObject.SetActive(false);
                itemBackgroundList[i].gameObject.SetActive(false);
                itemTextList[i].gameObject.SetActive(false);
            }
        }

        private void AddOnClick()
        {
            pageSelectButtonList[0].onClick.AddListener(()=>ChangePage(0));
            pageSelectButtonList[1].onClick.AddListener(()=>ChangePage(1));
            pageSelectButtonList[2].onClick.AddListener(()=>ChangePage(2));
            pageSelectButtonList[3].onClick.AddListener(()=>ChangePage(3));
            itemButtonList[0].onClick.AddListener(() => ClickItem(itemTextList[0].text));
            itemButtonList[1].onClick.AddListener(() => ClickItem(itemTextList[1].text));
            itemButtonList[2].onClick.AddListener(() => ClickItem(itemTextList[2].text));
            itemButtonList[3].onClick.AddListener(() => ClickItem(itemTextList[3].text));
            itemButtonList[4].onClick.AddListener(() => ClickItem(itemTextList[4].text));
            itemButtonList[5].onClick.AddListener(() => ClickItem(itemTextList[5].text));
            itemButtonList[6].onClick.AddListener(() => ClickItem(itemTextList[6].text));
            itemButtonList[7].onClick.AddListener(() => ClickItem(itemTextList[7].text));
            itemButtonList[8].onClick.AddListener(() => ClickItem(itemTextList[8].text));
            itemButtonList[9].onClick.AddListener(() => ClickItem(itemTextList[9].text));
            itemButtonList[10].onClick.AddListener(() => ClickItem(itemTextList[10].text));
            itemButtonList[11].onClick.AddListener(() => ClickItem(itemTextList[11].text));
        }

        private void ClickItem(string itemName)
        {
            SynthesizeUI.Instance.OpenSynthesizeInfoUI(itemName);
        }

        private void ChangePage(int page)
        {
            if(page != selectedPage)
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
            Sort("Name");
            SetItemButton();
        }

        private void Sort(string sortType = "Basic")
        {
            if (sortType == "Basic")
                itemList.Sort();
            else if (sortType == "Name")
                itemList.Sort(NameCompare);
            else if (sortType == "Newest")
                itemList.Sort();
        }

        int BasicCompare(ItemData i1, ItemData i2)
        {

            return 1;
        }

        int NameCompare(ItemData i1, ItemData i2)
        {
            return string.Compare(i1.item_name, i2.item_name);
        }

        int NewestCompare(ItemData i1, ItemData i2)
        {

            return 1;
        }
    }
}