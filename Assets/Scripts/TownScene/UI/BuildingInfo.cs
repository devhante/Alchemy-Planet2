using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

public class BuildingInfo : MonoBehaviour
{

    public Image backGroundImage;
    public Image buildingImage;
    public Sprite tentImage;
    public Text nameText;
    public Text descText;
    public Text levelText;
    public Button upgradeButton;
    public Button closeButton;

    private Building building;
    private string buildingName;
    private float waitingTime;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(() => { CloseInfo(); });
        upgradeButton.onClick.AddListener(() => { StartCoroutine("UpgradeBuilding"); });
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

    IEnumerator UpgradeBuilding()
    {
        if (waitingTime <= 0)
        {
            waitingTime = building.buildingLevel * 2f;
            Debug.Log(waitingTime);
            buildingImage.sprite = tentImage;
            for (int i = 0; i < waitingTime; i++)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log(i);
            }
            waitingTime = 0;
            building.buildingLevel++;
            levelText.text = "Lv." + building.buildingLevel;
            buildingImage.sprite = DataManager.Instance.structures[buildingName].image;
            DataManager.Instance.structures[buildingName] = building;
            yield return null;
        }
        yield return null;
    }
}
