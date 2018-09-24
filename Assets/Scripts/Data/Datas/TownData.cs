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
        public string material1Name;
        public string material2Name;
        public string material3Name;
        public int material1Count;
        public int material2Count;
        public int material3Count;
        public long UpgradeEndTime;
        public int money;
        

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
            strc.buildingObject = buildingObject;
            strc.position = position;
            strc.setup = setup;
            strc.material1Name = material1Name;
            strc.material2Name = material2Name;
            strc.material3Name = material3Name;
            return strc;
        }
        public void UpgradeStart()
        {
            upgrading = true;
            image = Resources.Load<Sprite>("Sprites/Town/Tent");
            DataManager.Instance.CurrentPlayerData.SetBuilding(this);
            AlchemyPlanet.TownScene.BuildingManagement.Instance.SendMessage("SetImage");
            int UpgradeTime = buildingLevel * 10;
            UpgradeEndTime = DateTime.Now.AddSeconds(UpgradeTime).ToBinary();
            WebSocketManager.Instance.SendUpdateBuilding("", DataManager.Instance.CurrentPlayerData.player_id, id.ToString(), buildingName, buildingLevel, position, setup, flip, upgrading, UpgradeEndTime);
        }

        public void UpgradeEnd()
        {
            buildingLevel++;
            image = Resources.Load<Sprite>("Sprites/Town/" + buildingName + buildingLevel.ToString());
            upgrading = false;
            DataManager.Instance.CurrentPlayerData.SetBuilding(this);

            Debug.Log(AlchemyPlanet.TownScene.BuildingManagement.Instance);

            if(AlchemyPlanet.TownScene.BuildingManagement.Instance != null)
                AlchemyPlanet.TownScene.BuildingManagement.Instance.SendMessage("SetImage");

            WebSocketManager.Instance.SendUpdateBuilding("", DataManager.Instance.CurrentPlayerData.player_id, id.ToString(), buildingName, buildingLevel, position, setup, flip, upgrading, UpgradeEndTime);
        }

        public void Build()
        {
            buildingObject.GetComponent<SpriteRenderer>().sprite = image;
            buildingObject.transform.position = new Vector2(position, image.bounds.size.y / 2 - 1);
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
        public string material1Name;
        public string material2Name;
        public string material3Name;
        public int material1Count;
        public int material2Count;
        public int material3Count;
        public int money;

        
        public Interior(string interiorName)
        {
            this.interiorName = interiorName;
        }

        public Interior Clone()
        {
            Interior strc = new Interior(interiorName);
            strc.interiorDiscription = interiorDiscription;
            strc.image = image;
            strc.interiorObject = interiorObject;
            strc.position = position;
            strc.setup = setup;
            strc.material1Name = material1Name;
            strc.material2Name = material2Name;
            strc.material3Name = material3Name;
            return strc;
        }

        public void Build()
        {
            interiorObject.GetComponent<SpriteRenderer>().sprite = image;
            interiorObject.transform.position = new Vector2(position, image.bounds.size.y / 2 - 1);
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