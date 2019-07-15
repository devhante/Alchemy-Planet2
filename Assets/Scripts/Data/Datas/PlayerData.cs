using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace AlchemyPlanet.Data
{
    public class CollectionName
    {
        public string playerId;
        public string playerName;

        public CollectionName(string playerId, string playerName)
        {
            this.playerId = playerId;
            this.playerName = playerName;
        }
    }

    public class CollectionLevel
    {
        public string playerId;
        public int level;
        public int exp;

        public CollectionLevel(string playerId, int level, int exp)
        {
            this.playerId = playerId;
            this.level = level;
            this.exp = exp;
        }
    }

    public class CollectionGoods
    {
        public string playerId;
        public int uniCoin;
        public int cosmoStone;
        public int oxygenTank;

        public CollectionGoods(string playerId, int uniCoin, int cosmoStone, int oxygenTank)
        {
            this.playerId = playerId;
            this.uniCoin = uniCoin;
            this.cosmoStone = cosmoStone;
            this.oxygenTank = oxygenTank;
        }
    }

    public class CollectionItem
    {
        public string playerId;
        public string itemId;
        public int number;

        public CollectionItem(string playerId, string itemId, int number)
        {
            this.playerId = playerId;
            this.itemId = itemId;
            this.number = number;
        }
    }

    public class CollectionBuilding
    {
        public string playerId;
        public string playerBuildingId;
        public string buildingId;
        public int level;
        public float position;
        public bool isConstructed;
        public bool isFlipped;
        public bool isUpgrading;
        public DateTime endDate;

        public CollectionBuilding(string playerId, string playerBuildingId, string buildingId, int level, float position, bool isConstructed, bool isFlipped, bool isUpgrading, DateTime endDate)
        {
            this.playerId = playerId;
            this.playerBuildingId = playerBuildingId;
            this.buildingId = buildingId;
            this.level = level;
            this.position = position;
            this.isConstructed = isConstructed;
            this.isFlipped = isFlipped;
            this.isUpgrading = isUpgrading;
            this.endDate = endDate;
        }
    }

    public class CollectionInterior
    {
        public string playerId;
        public string playerInteriorId;
        public string interiorId;
        public float position;
        public bool isConstructed;
        public bool isFlipped;

        public CollectionInterior(string playerId, string playerInteriorId, string interiorId, float position, bool isConstructed, bool isFlipped)
        {
            this.playerId = playerId;
            this.playerInteriorId = playerInteriorId;
            this.interiorId = interiorId;
            this.position = position;
            this.isConstructed = isConstructed;
            this.isFlipped = isFlipped;
        }
    }

    public class CollectionCharacter
    {
        public string playerId;
        public string characterId;
        public int level;
        public int health;
        public int speed;
        public int attackPower;

        public CollectionCharacter(string playerId, string characterId, int level, int health, int speed, int attackPower)
        {
            this.playerId = playerId;
            this.characterId = characterId;
            this.level = level;
            this.health = health;
            this.speed = speed;
            this.attackPower = attackPower;
        }
    }

    public class CollectionParty
    {
        public string playerId;
        public int partyIndex;
        public int slotIndex;
        public string characterId;

        public CollectionParty(string playerId, int partyIndex, int slotIndex, string characterId)
        {
            this.playerId = playerId;
            this.partyIndex = partyIndex;
            this.slotIndex = slotIndex;
            this.characterId = characterId;
        }
    }

    public class CollectionRequest
    {
        public string playerId;
        public string requestId;

        public CollectionRequest(string playerId, string requestId)
        {
            this.playerId = playerId;
            this.requestId = requestId;
        }
    }


    public class CollectionStroyStar
    {
        public string playerId;
        public string stageNumber;
        public int number;

        public CollectionStroyStar(string playerId, string stageNumber, int number)
        {
            this.playerId = playerId;
            this.stageNumber = stageNumber;
            this.number = number;
        }
    }

    public class PlayerData
    {
        public string player_id;
        public string player_name;

        public int level;
        public int exp;

        //재화
        public int unicoin;
        public int cosmostone;
        public int oxygentank;
        public int Max_oxygentank;

        //재료
        public Dictionary<string, int> inventory;

        //건물
        public List<Building> buildings;
        public List<Interior> interiors;
        public List<GameObject> setupBuildilngs;
        public int boundary;

        // 연금술
        public int alchemyLevel;
        public int alchemyEXP;
        public int starPowder;

        //캐릭터
        public List<Character> characters;

        //파티 편성
        public CharacterEnum[,] party;

        //의뢰
        public Request[] request;

        //스토리 별
        public Dictionary<string, int> stroystar;

        public PlayerData()
        {
            this.player_id = "";
            this.player_name = "포핀";

            this.level = 1;
            this.exp = 0;

            this.unicoin = 0;
            this.cosmostone = 0;
            this.oxygentank = 10;
            this.Max_oxygentank = 10;

            this.inventory = new Dictionary<string, int>();
            this.buildings = new List<Building>();
            this.interiors = new List<Interior>();
            this.characters = new List<Character>();

            this.alchemyEXP = 0;
            this.alchemyLevel = 0;
            this.starPowder = 0;


            party = new CharacterEnum[9, 3];

            request = new Request[4];

            stroystar = new Dictionary<string, int>();

            this.boundary = 15;

            //AddSampleDatas();
        }

        //따로 처리해줘야 하는 경험치는 아래 함수를 사용한다.
        public void UpdateExp(int value)
        {
            exp += value;
            int MAX_EXP = this.level * 100;

            while (exp < MAX_EXP)
            {
                this.level++;
                this.exp -= MAX_EXP;
            }
        }

        public void UpdateRequest()
        {
            for (int i = 0; i < request.Length; ++i)
            {
                int rand = UnityEngine.Random.Range(0, 4);
                request[i] = AlchemyScene.AlchemyManager.Instance.requests[rand];
            }
        }

        public void AddSampleDatas()
        {
            interiors.Add(DataManager.Instance.interiorInfo["Tree"].Clone());
            GiveId(interiors[0]);
            buildings.Add((DataManager.Instance.buildingInfo["House"] as Building).Clone());
            GiveId(buildings[0]);
            interiors.Add(DataManager.Instance.interiorInfo["Tree"].Clone());
            GiveId(interiors[1]);

            inventory.Add("붉은 꽃잎", 3);
            inventory.Add("블루베리", 2);

            unicoin += 100000;
        }

        public void SetBuilding(Building building)
        {
            foreach (GameObject obj in setupBuildilngs)
            {
                if (building.id == int.Parse(obj.name.Substring(0, obj.name.Length - 7)))
                {
                    obj.GetComponent<SpriteRenderer>().sprite = building.image;
                    obj.transform.position = new Vector2(obj.transform.position.x, building.image.bounds.size.y / 2 - 1);
                    GameObject.Destroy(obj.GetComponent<PolygonCollider2D>());
                    obj.AddComponent<PolygonCollider2D>().isTrigger = true;
                    break;
                }
            }
        }

        public void GiveId<T>(T strc)
        {
            if (strc is Building)
            {
                int buildingCount = 0;
                foreach (Building structure in buildings)
                    buildingCount++;
                (strc as Building).id = int.Parse("1" + buildingCount.ToString());
            }
            else
            {
                int interiorCount = 0;
                foreach (Interior structure in interiors)
                    interiorCount++;
                (strc as Interior).id = int.Parse("2" + interiorCount.ToString());
            }
        }
    }

}