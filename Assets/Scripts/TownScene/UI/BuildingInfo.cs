using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;
using System;

public class BuildingInfo : MonoBehaviour
{
    public List<GameObject> materialCell;
    public List<Image> materialImage;
    public List<Text> materialText;
    public Image backGroundImage;
    public Image buildingImage;
    public Sprite tentImage;
    public Text nameText;
    public Text descText;
    public Text levelText;
    public Text moneyText;
    public Text effectText;
    public Text effectTitle;
    public Button upgradeButton;
    public Button closeButton;

    [SerializeField] private Sprite makeButtonSprite;
    [SerializeField] private Sprite upgradeButtonSprite;
    [SerializeField] private RawImage blackMask;

    private Building building;
    private Interior interior;
    private float waitingTime;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(() => { CloseInfo(); });
    }

    public void SetInfo<T>(T strc)
    {
        foreach (Image image in materialImage)
            image.gameObject.SetActive(false);
        if (strc is Building)
        {
            building = strc as Building;
            buildingImage.sprite = building.image;
            nameText.text = "이름 : " + building.buildingName;
            descText.text = "설명 : " + building.buildingDiscription;
            levelText.text = "Lv." + building.buildingLevel;
            effectText.text = building.effect;
            effectTitle.gameObject.SetActive(true);

            if (DataManager.Instance.CurrentPlayerData.buildings.Contains(building))
            {
                upgradeButton.onClick.RemoveAllListeners();
                upgradeButton.onClick.AddListener(() => { UpgradeBuilding(); });
                upgradeButton.GetComponent<Image>().sprite = upgradeButtonSprite;
            }
            else
            {
                upgradeButton.onClick.RemoveAllListeners();
                upgradeButton.onClick.AddListener(() => { MakeBuilding(); });
                upgradeButton.GetComponent<Image>().sprite = makeButtonSprite;
            }
            int i;
            for (i = 0; i < 3; i++)
            {
                materialCell[i].SetActive(false);
                materialImage[i].gameObject.SetActive(false);
                materialText[i].gameObject.SetActive(false);
            }
            i = 0;
            foreach (string str in building.material.Keys)
            {
                materialCell[i].SetActive(true);
                materialImage[i].gameObject.SetActive(true);
                materialText[i].gameObject.SetActive(true);
                materialImage[i].sprite = DataManager.Instance.itemInfo[str].image;
                materialText[i++].text = "x" + building.material[str].ToString();
            }

            Debug.Log(building.money);

            moneyText.text = building.money.ToString();
        }
        if (strc is Interior)
        {
            interior = strc as Interior;
            Debug.Log(interior.interiorDiscription);
            buildingImage.sprite = interior.image;
            nameText.text = "이름 : " + interior.interiorName;
            descText.text = "설명 : " + interior.interiorDiscription;
            levelText.text = "인테리어";
            effectTitle.gameObject.SetActive(false);


            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => { MakeInterior(); });
            upgradeButton.GetComponent<Image>().sprite = makeButtonSprite;

            int i;
            for (i = 0; i < 3; i++)
            {
                materialCell[i].SetActive(false);
                materialImage[i].gameObject.SetActive(false);
                materialText[i].gameObject.SetActive(false);
            }
            i = 0;
            foreach (string str in interior.material.Keys)
            {
                materialCell[i].SetActive(true);
                materialImage[i].gameObject.SetActive(true);
                materialText[i].gameObject.SetActive(true);
                materialImage[i].sprite = DataManager.Instance.itemInfo[str].image;
                materialText[i++].text = "x" + interior.material[str].ToString();
            }

            moneyText.text = interior.money.ToString();
        }
    }

    public void OpenInfo()
    {
        backGroundImage.gameObject.SetActive(true);
    }

    void CloseInfo()
    {
        backGroundImage.gameObject.SetActive(false);
        blackMask.gameObject.SetActive(false);
    }

    void UpgradeBuilding()
    {
        if (!building.upgrading && CheckCanManageBuilding())
        {
            foreach (string str in building.material.Keys)
                DataManager.Instance.CurrentPlayerData.inventory[str] -= building.material[str];

            DataManager.Instance.CurrentPlayerData.unicoin -= building.money;

            building.UpgradeStart();
            SetInfo(building);
        }
    }

    void MakeBuilding()
    {
        if (CheckCanManageBuilding())
        {
            foreach (string str in building.material.Keys)
                DataManager.Instance.CurrentPlayerData.inventory[str] -= building.material[str];

            DataManager.Instance.CurrentPlayerData.unicoin -= building.money;

            DataManager.Instance.CurrentPlayerData.buildings.Add(building.Clone());

            AlchemyPlanet.TownScene.BuildingManagement.Instance.GetOwnBuilding();
            AlchemyPlanet.TownScene.BuildingManagement.Instance.SendMessage("SetBuildingImage");
            SetInfo(DataManager.Instance.CurrentPlayerData.buildings.Find(a => a.buildingName == building.buildingName));
            DataManager.Instance.CurrentPlayerData.GiveId(building);
            BackendManager.Instance.AddTown(BackendManager.Instance.GetInDate("town"), building.id, building.buildingName, building.buildingLevel,
                building.position, building.setup, building.flip, building.upgrading, building.endDate);
        }
    }

    void MakeInterior()
    {
        if (CheckCanManageInterior())
        {
            foreach (string str in interior.material.Keys)
                DataManager.Instance.CurrentPlayerData.inventory[str] -= interior.material[str];

            DataManager.Instance.CurrentPlayerData.unicoin -= interior.money;

            DataManager.Instance.CurrentPlayerData.interiors.Add(interior.Clone());

            AlchemyPlanet.TownScene.BuildingManagement.Instance.GetOwnBuilding();
            AlchemyPlanet.TownScene.BuildingManagement.Instance.SendMessage("SetInteriorImage");
            SetInfo(DataManager.Instance.CurrentPlayerData.interiors.Find(a => a.interiorName == interior.interiorName));
            DataManager.Instance.CurrentPlayerData.GiveId(interior);
            BackendManager.Instance.AddTown(BackendManager.Instance.GetInDate("town"), interior.id, interior.interiorName, 0,
                interior.position, interior.setup, interior.flip, false, new DateTime());
        }
    }

    bool CheckCanManageBuilding()
    {
        bool b = true;
        foreach (string str in building.material.Keys)
        {
            if (!DataManager.Instance.CurrentPlayerData.inventory.ContainsKey(str) || (DataManager.Instance.CurrentPlayerData.inventory.ContainsKey(str)
                && building.material[str] >= DataManager.Instance.CurrentPlayerData.inventory[str]))
                b = false;
        }
        if (b && building.money > DataManager.Instance.CurrentPlayerData.unicoin)
            b = false;

        return b;
    }

    bool CheckCanManageInterior()
    {
        bool b = true;
        foreach (string str in interior.material.Keys)
        {
            if (!DataManager.Instance.CurrentPlayerData.inventory.ContainsKey(str) || (DataManager.Instance.CurrentPlayerData.inventory.ContainsKey(str)
                && interior.material[str] >= DataManager.Instance.CurrentPlayerData.inventory[str]))
                b = false;
        }
        if (b && interior.money > DataManager.Instance.CurrentPlayerData.unicoin)
            b = false;

        return b;
    }
}
