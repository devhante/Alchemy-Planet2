using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

public class BuildingInfo : MonoBehaviour {

    public Image backGroundImage;
    public Image buildingImage;
    public Text nameText;
    public Text descText;
    public Text levelText;
    public Button upgradeButton;
    public Button closeButton;

    private Building building;
    private string buildingName;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(() => { CloseInfo(); });
        upgradeButton.onClick.AddListener(() => { UpgradeBuilding(); });
    }

    public void SetInfo(string str)
    {
        buildingName = str;
        building = (Building)DataManager.Instance.structures[str];
        OpenInfo();
    }

    void OpenInfo()
    {
        buildingImage.sprite = building.image;
        nameText.text = "이름 : " + building.structureName;
        descText.text = "설명 : " + building.buildingDiscription;
        levelText.text = "Lv." + building.buildingLevel;
        backGroundImage.gameObject.SetActive(true);
    }

    void CloseInfo()
    {
        backGroundImage.gameObject.SetActive(false);
    }

    void UpgradeBuilding()
    {
        building.buildingLevel++;
        levelText.text = "Lv." + building.buildingLevel;
        DataManager.Instance.structures[buildingName] = building;
    }
}
