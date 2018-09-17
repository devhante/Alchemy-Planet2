using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.Data
{
    public class Structure
    {
        public int id;
        public string structureName;
        public Sprite image;
        public GameObject StructureObject;
        public Vector2 position;
        public bool setup = false;
        public bool flip = false;


        public virtual Structure Clone()
        {
            Structure strc = new Structure();
            strc.id = id;
            strc.structureName = structureName;
            strc.image = image;
            strc.StructureObject = StructureObject;
            strc.position = position;
            strc.setup = setup;
            return strc;
        }
        public void Build()
        {
            position = new Vector2(position.x, image.bounds.size.y / 2 - 1);
            StructureObject.GetComponent<SpriteRenderer>().sprite = image;
            StructureObject.transform.position = position;
            if (StructureObject.GetComponent<PolygonCollider2D>())
                GameObject.DestroyImmediate(StructureObject.GetComponent<PolygonCollider2D>(), true);
            StructureObject.AddComponent<PolygonCollider2D>();
            StructureObject.GetComponent<PolygonCollider2D>().isTrigger = true;
            StructureObject.GetComponent<SpriteRenderer>().sortingOrder = -90;
            StructureObject.name = id.ToString();
            StructureObject.GetComponent<SpriteRenderer>().flipX = flip;
        }
    }

    public class Building : Structure
    {
        public string buildingDiscription;
        public int buildingLevel;
        public bool upgrading = false;
        public string material1Name;
        public string material2Name;
        public int material1Count;
        public int material2Count;


        public Building(string buildingName, string buildingDiscription, int buildingLevel)
        {
            this.structureName = buildingName;
            this.buildingDiscription = buildingDiscription;
            this.buildingLevel = buildingLevel;
        }
        public override Structure Clone()
        {

            Structure strc = new Building(structureName, buildingDiscription, buildingLevel);
            strc.image = image;
            strc.StructureObject = StructureObject;
            strc.position = position;
            strc.setup = setup;
            return strc;
        }
        public void Upgrade()
        {
            upgrading = true;
            image = Resources.Load<Sprite>("Sprites/Town/Tent");
            // 시간 보내고 받기 서버랑 연동하자 민제야!!
            buildingLevel++;
            image = Resources.Load<Sprite>("Sprites/Town/" + structureName + buildingLevel.ToString());
            upgrading = false;
        }
    }

    public class Interior : Structure
    {
        public Interior(string interiorName)
        {
            this.structureName = interiorName;
        }
        public override Structure Clone()
        {
            Structure strc = new Interior(structureName);
            strc.image = image;
            strc.StructureObject = StructureObject;
            strc.position = position;
            strc.setup = setup;
            return strc;
        }
    }
}