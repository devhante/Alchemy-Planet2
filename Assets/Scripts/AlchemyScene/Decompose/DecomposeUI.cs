using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.AlchemyScene
{
    public class DecomposeUI : MonoBehaviour
    {
        [SerializeField]
        private List<ItemCell> decomposeCellList;
        [SerializeField]
        private List<ItemCell> itemCellList;
        [SerializeField]
        private List<Button> typeSelectButtonList;
        [SerializeField]
        private List<string> typeSelectNameList;
        [SerializeField]
        private Text starPowderText;
        [SerializeField]
        private Button nextPageButton;
        [SerializeField]
        private Button previousPageButton;
        [SerializeField]
        private Text pageText;

        private List<ItemData> itemList;
        private Dictionary<string,int> itemCountList;
        private int itemPage;
        private int currentPage;
        private int decomposeItemCount;

        void Start()
        {
            decomposeItemCount = 0;

            for(int i = 0 ; i<typeSelectButtonList.Count; i++)
                typeSelectButtonList[i].onClick.AddListener(()=>ChangeType(typeSelectNameList[i]));

            nextPageButton.onClick.AddListener(()=>NextPage());
            previousPageButton.onClick.AddListener(()=>PreviousPage());
            for (int i = 0; i < itemCellList.Count; i++)
            {
                int n = i;
                itemCellList[n].gameObject.GetComponent<Button>().onClick.AddListener(
                    ()=>AddDecomposeList(DataManager.Instance.itemInfo[itemCellList[n].itemName],itemCellList[n].itemCount)
                );
                decomposeCellList[n].gameObject.GetComponent<Button>().onClick.AddListener(
                    ()=>RemoveDecomposeList(DataManager.Instance.itemInfo[decomposeCellList[n].itemName])
                );
            } 
        }

        private void GetItem(string itemTypeName)
        {
            ItemScarcity itemType;
            switch (itemTypeName)
            {
                case nameof(ItemScarcity.Common):
                    itemType = ItemScarcity.Common;
                    break;
                case nameof(ItemScarcity.Rare):
                    itemType = ItemScarcity.Rare;
                    break;
                case nameof(ItemScarcity.Unique):
                    itemType = ItemScarcity.Unique;
                    break;
                case nameof(ItemScarcity.Legendry):
                    itemType = ItemScarcity.Legendry;
                    break;
                default:
                    return;
            }

            itemList.Clear();

            foreach (var item in DataManager.Instance.CurrentPlayerData.inventory)
            {
                if (DataManager.Instance.itemInfo[item.Key].item_scarcity == itemType)
                {
                    itemCountList.Add(item.Key,item.Value);
                    itemList.Add(DataManager.Instance.itemInfo[item.Key]);
                }
            }

            itemPage = itemList.Count / itemCellList.Count;
            currentPage = 0;
            SetItemPage();
        }

        private void SetItemPage()
        {
            for (int i = 0; i < itemCellList.Count && i < itemList.Count; i++)
            {
                itemCellList[i].gameObject.SetActive(true);
                itemCellList[i].SetItemCell(itemList[i + currentPage*itemCellList.Count].item_name, 
                itemList[i + currentPage*itemCellList.Count].image,itemCountList[itemList[i + currentPage*itemCellList.Count].item_name]);
            }
            for (int i = itemList.Count - currentPage * itemCellList.Count; i < itemCellList.Count; i++)
                itemCellList[i].gameObject.SetActive(false);
        }

        private void AddDecomposeList(ItemData itemData, int count)
        {
            decomposeCellList[decomposeItemCount].gameObject.SetActive(true);
            decomposeCellList[decomposeItemCount].SetItemCell(itemData.item_name, itemData.image, count);
            decomposeItemCount++;
        }

        private void RemoveDecomposeList(ItemData itemData)
        {
            decomposeItemCount--;
            decomposeCellList[decomposeItemCount].gameObject.SetActive(false);
        }

        private void NextPage()
        {
            currentPage++;
            SetPageButton();
            SetItemPage();
        }

        private void PreviousPage()
        {
            currentPage--;
            SetPageButton();
            SetItemPage();
        }

        private void SetPageButton()
        {
            if(currentPage>0)
                previousPageButton.gameObject.SetActive(true);
            else
                previousPageButton.gameObject.SetActive(false);
            if (currentPage < itemPage - 1)
                nextPageButton.gameObject.SetActive(true);
            else
                nextPageButton.gameObject.SetActive(false);
        }

        private void ChangeType(string typeName)
        {
            GetItem(typeName);
            SetPageButton();
            SetItemPage();
        }
    }
}