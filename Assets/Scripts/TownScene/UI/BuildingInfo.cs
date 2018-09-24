using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

public class BuildingInfo : MonoBehaviour
{
    public Image material1Image;
    public Image material2Image;
    public Image material3Image;
    public Image backGroundImage;
    public Image buildingImage;
    public Sprite tentImage;
    public Text nameText;
    public Text descText;
    public Text levelText;
    public Text moneyText;
    public Text material1Text;
    public Text material2Text;
    public Text material3Text;
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
        if(strc is Building)
        {
            building = strc as Building;
            buildingImage.sprite = building.image;
            nameText.text = "이름 : " + building.buildingName;
            descText.text = "설명 : " + building.buildingDiscription;
            levelText.text = "Lv." + building.buildingLevel;

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

            if (building.material1Count > 0)
            {
                material1Image.gameObject.SetActive(true);
                material1Text.gameObject.SetActive(true);
                material1Image.sprite = DataManager.Instance.itemInfo[building.material1Name].image;
                material1Text.text = "x" + building.material1Count.ToString();
            }
            else
            {
                material1Image.gameObject.SetActive(false);
                material1Text.gameObject.SetActive(false);
            }
            if (building.material2Count > 0)
            {
                material2Image.gameObject.SetActive(true);
                material2Text.gameObject.SetActive(true);
                material2Image.sprite = DataManager.Instance.itemInfo[building.material2Name].image;
                material2Text.text = "x" + building.material2Count.ToString();
            }
            else
            {
                material2Image.gameObject.SetActive(false);
                material2Text.gameObject.SetActive(false);
            }
            if (building.material3Count > 0)
            {
                material3Image.gameObject.SetActive(true);
                material3Text.gameObject.SetActive(true);
                material3Image.sprite = DataManager.Instance.itemInfo[building.material3Name].image;
                material3Text.text = "x" + building.material3Count.ToString();
            }
            else
            {
                material3Image.gameObject.SetActive(false);
                material3Text.gameObject.SetActive(false);
            }
            moneyText.text = building.money.ToString();
        }
        if (strc is Interior)
        {
            interior = strc as Interior;
            buildingImage.sprite = interior.image;
            nameText.text = "이름 : " + interior.interiorName;
            descText.text = "설명 : " + interior.interiorDiscription;
            levelText.text = "인테리어";

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => { MakeBuilding(); });
            upgradeButton.GetComponent<Image>().sprite = makeButtonSprite;

            if (interior.material1Count>0)
            {
                material1Image.gameObject.SetActive(true);
                material1Text.gameObject.SetActive(true);
                material1Image.sprite = DataManager.Instance.itemInfo[interior.material1Name].image;
                material1Text.text = "x" + interior.material1Count.ToString();
            }
            else
            {
                material1Image.gameObject.SetActive(false);
                material1Text.gameObject.SetActive(false);
            }
            if (interior.material2Count > 0)
            {
                material2Image.gameObject.SetActive(true);
                material2Text.gameObject.SetActive(true);
                material2Image.sprite = DataManager.Instance.itemInfo[interior.material2Name].image;
                material2Text.text = "x" + interior.material2Count.ToString();
            }
            else
            {
                material2Image.gameObject.SetActive(false);
                material2Text.gameObject.SetActive(false);
            }
            if (interior.material3Count > 0)
            {
                material3Image.gameObject.SetActive(true);
                material3Text.gameObject.SetActive(true);
                material3Image.sprite = DataManager.Instance.itemInfo[interior.material3Name].image;
                material3Text.text = "x" + interior.material3Count.ToString();
            }
            else
            {
                material3Image.gameObject.SetActive(false);
                material3Text.gameObject.SetActive(false);
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
        if (!building.upgrading && DataManager.Instance.CurrentPlayerData.inventory[building.material2Name]>=building.material2Count
            && DataManager.Instance.CurrentPlayerData.inventory[building.material1Name] >= building.material1Count
            && DataManager.Instance.CurrentPlayerData.inventory[building.material3Name] >= building.material3Count
            && DataManager.Instance.CurrentPlayerData.unicoin >= building.money)
        {
            DataManager.Instance.CurrentPlayerData.inventory[building.material1Name] -= building.material1Count;
            DataManager.Instance.CurrentPlayerData.inventory[building.material2Name] -= building.material2Count;
            DataManager.Instance.CurrentPlayerData.inventory[building.material3Name] -= building.material3Count;
            DataManager.Instance.CurrentPlayerData.unicoin -= building.money;

            building.UpgradeStart();
            SetInfo(building);
        }
    }

    void MakeBuilding()
    {
        if (DataManager.Instance.CurrentPlayerData.inventory[building.material2Name] >= building.material2Count
            && DataManager.Instance.CurrentPlayerData.inventory[building.material1Name] >= building.material1Count
            && DataManager.Instance.CurrentPlayerData.inventory[building.material3Name] >= building.material3Count
            && DataManager.Instance.CurrentPlayerData.unicoin >= building.money)
        {
            DataManager.Instance.CurrentPlayerData.inventory[building.material1Name] -= building.material1Count;
            DataManager.Instance.CurrentPlayerData.inventory[building.material2Name] -= building.material2Count;
            DataManager.Instance.CurrentPlayerData.inventory[building.material3Name] -= building.material3Count;
            DataManager.Instance.CurrentPlayerData.unicoin -= building.money;

            DataManager.Instance.CurrentPlayerData.buildings.Add(building.Clone());

            AlchemyPlanet.TownScene.BuildingManagement.Instance.GetOwnBuilding();
            AlchemyPlanet.TownScene.BuildingManagement.Instance.SendMessage("SetImage");
            SetInfo(DataManager.Instance.CurrentPlayerData.buildings.Find(a => a.buildingName == building.buildingName));
            DataManager.Instance.CurrentPlayerData.GiveId(building);
            WebSocketManager.Instance.SendInsertBuilding("", DataManager.Instance.CurrentPlayerData.player_id, building.id.ToString(), building.buildingName, building.buildingLevel,
                building.position, building.setup, building.flip, building.upgrading, building.UpgradeEndTime);
        }
    }

    void MakeInterior()
    {
        if (DataManager.Instance.CurrentPlayerData.inventory[interior.material2Name] >= interior.material2Count
            && DataManager.Instance.CurrentPlayerData.inventory[interior.material1Name] >= interior.material1Count
            && DataManager.Instance.CurrentPlayerData.inventory[interior.material3Name] >= interior.material3Count
            && DataManager.Instance.CurrentPlayerData.unicoin >= interior.money)
        {
            DataManager.Instance.CurrentPlayerData.inventory[interior.material1Name] -= interior.material1Count;
            DataManager.Instance.CurrentPlayerData.inventory[interior.material2Name] -= interior.material2Count;
            DataManager.Instance.CurrentPlayerData.inventory[interior.material3Name] -= interior.material3Count;
            DataManager.Instance.CurrentPlayerData.unicoin -= interior.money;

            DataManager.Instance.CurrentPlayerData.interiors.Add(interior.Clone());
            AlchemyPlanet.TownScene.BuildingManagement.Instance.GetOwnBuilding();
            AlchemyPlanet.TownScene.BuildingManagement.Instance.SendMessage("SetImage");
            SetInfo(DataManager.Instance.CurrentPlayerData.interiors.Find(a => a.interiorName == interior.interiorName));
            DataManager.Instance.CurrentPlayerData.GiveId(interior);
            WebSocketManager.Instance.SendInsertInterior("", DataManager.Instance.CurrentPlayerData.player_id, interior.id.ToString(), interior.interiorName, building.position, building.setup, building.flip);
        }
    }
}
