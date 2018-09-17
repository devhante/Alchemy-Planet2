using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

public class BuildingInfo : MonoBehaviour
{
    public Image material1Image;
    public Image material2Image;
    public Image backGroundImage;
    public Image buildingImage;
    public Sprite tentImage;
    public Text nameText;
    public Text descText;
    public Text levelText;
    public Text material1Text;
    public Text material2Text;
    public Button upgradeButton;
    public Button closeButton;

    private Building building;
    private float waitingTime;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(() => { CloseInfo(); });
        upgradeButton.onClick.AddListener(() => { UpgradeBuilding(); });
    }

    public void SetInfo(Building building)
    {
        this.building = building;
        OpenInfo();
    }

    void OpenInfo()
    {
        buildingImage.sprite = building.image;
        nameText.text = "이름 : " + building.structureName;
        descText.text = "설명 : " + building.buildingDiscription;
        levelText.text = "Lv." + building.buildingLevel;
        backGroundImage.gameObject.SetActive(true);
        SetMaterial();
    }

    void CloseInfo()
    {
        backGroundImage.gameObject.SetActive(false);
    }

     void SetMaterial()
    {
        material1Image.sprite = DataManager.Instance.itemInfo[building.material1Name].image;
        material2Image.sprite = DataManager.Instance.itemInfo[building.material2Name].image;
        material1Text.text = DataManager.Instance.CurrentPlayerData.inventory[building.material1Name].ToString() 
            + " / " + building.material1Count.ToString();
        material2Text.text = DataManager.Instance.CurrentPlayerData.inventory[building.material2Name].ToString() 
            + " / " + building.material2Count.ToString();
    }

    void UpgradeBuilding()
    {
        if (!building.upgrading && DataManager.Instance.CurrentPlayerData.inventory[building.material2Name]>=building.material2Count
            && DataManager.Instance.CurrentPlayerData.inventory[building.material1Name] >= building.material1Count)
        {
            DataManager.Instance.CurrentPlayerData.inventory[building.material1Name] -= building.material1Count;
            DataManager.Instance.CurrentPlayerData.inventory[building.material2Name] -= building.material2Count;
            building.Upgrade();
            DataManager.Instance.CurrentPlayerData.SetBuilding();
            OpenInfo();
            AlchemyPlanet.TownScene.TownUpgrade.Instance.SendMessage("SetImage");
        }
    }
}
