using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.Data
{
    public class Building
    {
        public int id;
        public string buildingName;
        public Sprite image;
        public GameObject buildingObject;
        public float position;
        public bool setup = false;
        public bool flip = false;
        public string buildingDiscription;
        public int buildingLevel;
        public bool upgrading = false;
        public Dictionary<string, int> material;
        public DateTime UpgradeEndTime;
        public int money;
        public string effect;
        

        public Building(string buildingName, string buildingDiscription, int buildingLevel)
        {
            this.buildingName = buildingName;
            this.buildingDiscription = buildingDiscription;
            this.buildingLevel = buildingLevel;
        }

        public Building Clone()
        {
            Building strc = new Building(buildingName, buildingDiscription, buildingLevel);
            strc.image = image;
            strc.effect = effect;
            strc.buildingObject = buildingObject;
            strc.position = position;
            strc.setup = setup;
            strc.material = material;
            strc.money = money;
            return strc;
        }
        public void UpgradeStart()
        {
            upgrading = true;
            image = Resources.Load<Sprite>("Sprites/TownScene/Tent");
            DataManager.Instance.CurrentPlayerData.SetBuilding(this);
            AlchemyPlanet.TownScene.BuildingManagement.Instance.SendMessage("SetImage");
            int UpgradeTime = buildingLevel * 10;
            UpgradeEndTime = DateTime.Now.AddSeconds(UpgradeTime);

            BackendManager.Instance.UpdateTownUpgrading(BackendManager.Instance.GetInDate("town"), id, upgrading);
            BackendManager.Instance.UpdateTownEndDate(BackendManager.Instance.GetInDate("town"), id, UpgradeEndTime);
        }

        public void UpgradeEnd()
        {
            buildingLevel++;
            image = Resources.Load<Sprite>("Sprites/TownScene/" + buildingName + buildingLevel.ToString());
            upgrading = false;
            DataManager.Instance.CurrentPlayerData.SetBuilding(this);

            if(AlchemyPlanet.TownScene.BuildingManagement.Instance != null)
                AlchemyPlanet.TownScene.BuildingManagement.Instance.SendMessage("SetImage");

            BackendManager.Instance.UpdateTownUpgrading(BackendManager.Instance.GetInDate("town"), id, upgrading);
        }

        public void Build()
        {
            buildingObject = DataManager.Instance.buildingInfo[buildingName].buildingObject;
            buildingObject.GetComponent<SpriteRenderer>().sprite = DataManager.Instance.buildingInfo[buildingName].image;
            buildingObject.transform.position = new Vector2(position, image.bounds.size.y / 2 - 0.9f);
            if (buildingObject.GetComponent<PolygonCollider2D>())
                GameObject.DestroyImmediate(buildingObject.GetComponent<PolygonCollider2D>(), true);
            buildingObject.AddComponent<PolygonCollider2D>();
            buildingObject.GetComponent<PolygonCollider2D>().isTrigger = true;
            buildingObject.GetComponent<SpriteRenderer>().sortingOrder = -90;
            buildingObject.name = id.ToString();
            buildingObject.GetComponent<SpriteRenderer>().flipX = flip;
        }
    }

    public class Interior
    {
        public int id;
        public string interiorName;
        public Sprite image;
        public GameObject interiorObject;
        public float position;
        public bool setup = false;
        public bool flip = false;
        public string interiorDiscription;
        public Dictionary<string, int> material;
        public int money;

        
        public Interior(string interiorName, string interiorDiscription)
        {
            this.interiorName = interiorName;
            this.interiorDiscription = interiorDiscription;
        }

        public Interior Clone()
        {
            Interior strc = new Interior(interiorName, interiorDiscription);
            strc.image = image;
            strc.interiorObject = interiorObject;
            strc.position = position;
            strc.setup = setup;
            strc.material = material;
            strc.money = money;
            return strc;
        }

        public void Build()
        {
            interiorObject = DataManager.Instance.interiorInfo[interiorName].interiorObject;
            interiorObject.GetComponent<SpriteRenderer>().sprite = DataManager.Instance.interiorInfo[interiorName].image;
            interiorObject.transform.position = new Vector2(position, image.bounds.size.y / 2 - 0.9f);
            if (interiorObject.GetComponent<PolygonCollider2D>())
                GameObject.DestroyImmediate(interiorObject.GetComponent<PolygonCollider2D>(), true);
            interiorObject.AddComponent<PolygonCollider2D>();
            interiorObject.GetComponent<PolygonCollider2D>().isTrigger = true;
            interiorObject.GetComponent<SpriteRenderer>().sortingOrder = -90;
            interiorObject.name = id.ToString();
            interiorObject.GetComponent<SpriteRenderer>().flipX = flip;
        }
    }
}