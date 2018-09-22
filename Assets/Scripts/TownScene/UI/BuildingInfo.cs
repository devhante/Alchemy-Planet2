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
    }

    public void SetInfo(Building building)
    {
        this.building = building;
        buildingImage.sprite = building.image;
        nameText.text = "이름 : " + building.structureName;
        descText.text = "설명 : " + building.buildingDiscription;
        levelText.text = "Lv." + building.buildingLevel;

        if (DataManager.Instance.CurrentPlayerData.structures.Contains(building))
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => { UpgradeBuilding(); });
            upgradeButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Town/UpgradeButton");
        }
        else
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => { MakeBuilding(); });
            upgradeButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Town/MakeButton");
        }

        material1Image.sprite = DataManager.Instance.itemInfo[building.material1Name].image;
        material1Text.text = DataManager.Instance.CurrentPlayerData.inventory[building.material1Name].ToString()
            + " / " + building.material1Count.ToString();

        material2Image.sprite = DataManager.Instance.itemInfo[building.material2Name].image;
        material2Text.text = DataManager.Instance.CurrentPlayerData.inventory[building.material2Name].ToString()
            + " / " + building.material2Count.ToString();
    }

    public void OpenInfo()
    {
        backGroundImage.gameObject.SetActive(true);
    }

    void CloseInfo()
    {
        backGroundImage.gameObject.SetActive(false);
    }

    void UpgradeBuilding()
    {
        if (!building.upgrading && DataManager.Instance.CurrentPlayerData.inventory[building.material2Name]>=building.material2Count
            && DataManager.Instance.CurrentPlayerData.inventory[building.material1Name] >= building.material1Count)
        {
            DataManager.Instance.CurrentPlayerData.inventory[building.material1Name] -= building.material1Count;
            DataManager.Instance.CurrentPlayerData.inventory[building.material2Name] -= building.material2Count;
            building.UpgradeStart();
            SetInfo(building);
        }
    }

    void MakeBuilding()
    {
        if (DataManager.Instance.CurrentPlayerData.inventory[building.material2Name] >= building.material2Count
            && DataManager.Instance.CurrentPlayerData.inventory[building.material1Name] >= building.material1Count)
        {
            DataManager.Instance.CurrentPlayerData.inventory[building.material1Name] -= building.material1Count;
            DataManager.Instance.CurrentPlayerData.inventory[building.material2Name] -= building.material2Count;
            DataManager.Instance.CurrentPlayerData.structures.Add(building.Clone() as Building);
            AlchemyPlanet.TownScene.BuildingManagement.Instance.GetOwnBuilding();
            AlchemyPlanet.TownScene.BuildingManagement.Instance.SendMessage("SetImage");
            SetInfo(DataManager.Instance.CurrentPlayerData.structures.Find(a => a.structureName == building.structureName) as  Building);
            DataManager.Instance.CurrentPlayerData.GiveId(building);
            WebSocketManager.Instance.SendInsertBuilding("", DataManager.Instance.CurrentPlayerData.player_id, building.id.ToString(), building.structureName, building.buildingLevel,
                building.position, building.setup, building.flip, building.upgrading, building.UpgradeEndTime);
        }
    }
}
