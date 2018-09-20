using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.TownScene
{
    public class InventoryCell : Common.UI<InventoryCell>
    {

        [SerializeField] private Button CloseButton;

        public List<GameObject> itemButtonList;
        public GameObject backGroundImage;


        protected override void Awake()
        {
            base.Awake();
            CloseButton.onClick.AddListener(() => { UIManager.Instance.CloseMenu(); });
        }

        private void OnEnable()
        {
            SetItem();
        }

        public void SetItem()
        {
            int i = 0;
            foreach (string itemName in DataManager.Instance.CurrentPlayerData.inventory.Keys)
            {
                if (DataManager.Instance.CurrentPlayerData.inventory[itemName]>0)
                {
                    itemButtonList[i].SetActive(true);
                    itemButtonList[i].GetComponent<ItemInfo>().SetInfo(
                        DataManager.Instance.itemInfo[itemName].item_name,
                        DataManager.Instance.itemInfo[itemName].discription,
                        DataManager.Instance.itemInfo[itemName].image,
                        DataManager.Instance.CurrentPlayerData.inventory[itemName]);
                }
                i++;
            }
        }
    }
}