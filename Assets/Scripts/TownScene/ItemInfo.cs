using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour {

    private Text name;
    private Text discription;

    void GetInfo(string item_name, string item_discription)
    {
        name.text = item_name;
        discription.text = item_discription;
    }
}
