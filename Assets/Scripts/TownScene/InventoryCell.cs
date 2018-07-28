using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCell : MonoBehaviour {

    public List<GameObject> itemButtonList;
    public GameObject backGroundImage;

    public void SetItem()
    {
        int i = 0;
        foreach(string itemName in DataManager.Instance.currentPlayerData.inventory.Keys)
        {
            itemButtonList[i].SetActive(true);
            itemButtonList[i].GetComponent<ItemInfo>().SetInfo(
                DataManager.Instance.materials[itemName].item_name,
                DataManager.Instance.materials[itemName].discription,
                DataManager.Instance.materials[itemName].image,
                DataManager.Instance.currentPlayerData.inventory[itemName]);
            i++;
        }
    }
    
    public void CloseInfo()
    {
        backGroundImage.SetActive(false);
    }
}
