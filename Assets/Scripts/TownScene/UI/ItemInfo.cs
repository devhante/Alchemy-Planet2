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
    public Button closeButton;

    private string item_name;
    private string discription;
    private Sprite image;


    public void SetInfo(string item_name, string item_discription, Sprite item_image, int item_count)
    {
        this.item_name = item_name;
        this.discription = item_discription;
        this.image = item_image;
        this.count.text = item_count>1? item_count.ToString():null;
        GetComponent<Image>().sprite = item_image;
    }

    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(() => { OpenInfo(); });
        closeButton.onClick.AddListener(() => { CloseInfo(); });
    }

    void OpenInfo()
    {
        nameInfo.text = item_name;
        discriptionInfo.text = discription;
        imageInfo.sprite = image;
        backGroundImage.gameObject.SetActive(true);
    }

    void CloseInfo()
    {
        backGroundImage.gameObject.SetActive(false);
    }
}
