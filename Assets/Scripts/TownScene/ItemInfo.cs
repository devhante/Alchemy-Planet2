using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour {

    public Image backGroundImage;
    public Text nameInfo;
    public Text discriptionInfo;
    public Image imageInfo;
    public Text count;

    private string name;
    private string discription;
    private Sprite image;


    public void SetInfo(string item_name, string item_discription, Sprite item_image, int item_count)
    {
        name = item_name;
        discription = item_discription;
        Debug.Log(item_image);
        image = item_image;
        count.text = item_count>1? item_count.ToString():null;
        GetComponent<Image>().sprite = item_image;
    }

    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(() => { OpenInfo(); });
    }

    void OpenInfo()
    {
        nameInfo.text = name;
        discriptionInfo.text = discription;
        imageInfo.sprite = image;
        backGroundImage.gameObject.SetActive(true);
    }
}
