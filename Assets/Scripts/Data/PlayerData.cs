using System;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.Data
{
    public class PlayerName
    {
        public string playerId;
        public string playerName;

        public PlayerName(string playerId, string playerName)
        {
            this.playerId = playerId;
            this.playerName = playerName;
        }
    }

    public class PlayerLevel
    {
        public string playerId;
        public int level;
        public int exp;

        public PlayerLevel(string playerId, int level, int exp)
        {
            this.playerId = playerId;
            this.level = level;
            this.exp = exp;
        }
    }

    public class PlayerGoods
    {
        public string playerId;
        public int uniCoin;
        public int cosmoStone;
        public int oxygenTank;

        public PlayerGoods(string playerId, int uniCoin, int cosmoStone, int oxygenTank)
        {
            this.playerId = playerId;
            this.uniCoin = uniCoin;
            this.cosmoStone = cosmoStone;
            this.oxygenTank = oxygenTank;
        }
    }

    public class PlayerItem
    {
        public string playerId;
        public string itemId;
        public int number;

        public PlayerItem(string playerId, string itemId, int number)
        {
            this.playerId = playerId;
            this.itemId = itemId;
            this.number = number;
        }
    }

    public class PlayerStructure
    {
        public string playerId;
        public string structureUniqueId;
        public string structureId;
        public int level;

        public PlayerStructure(string playerId, string structureUniqueId, string structureId, int level)
        {
            this.playerId = playerId;
            this.structureUniqueId = structureUniqueId;
            this.structureId = structureId;
            this.level = level;
        }
    }

    public class PlayerTownStructure
    {
        public string playerId;
        public string structureUniqueId;
        public int posion;

        public PlayerTownStructure(string playerId, string structureUniqueId, int posion)
        {
            this.playerId = playerId;
            this.structureUniqueId = structureUniqueId;
            this.posion = posion;
        }
    }

    public class PlayerUpgradingStructure
    {
        public string playerId;
        public string structureUniqueId;
        public DateTime startDate;
        public int requireTime;

        public PlayerUpgradingStructure(string playerId, string structureUniqueId, DateTime startDate, int requireTime)
        {
            this.playerId = playerId;
            this.structureUniqueId = structureUniqueId;
            this.startDate = startDate;
            this.requireTime = requireTime;
        }
    }

    public class PlayerCharacter
    {
        public string playerId;
        public string characterId;
        public int level;
        public int health;
        public int speed;
        public int attackPower;

        public PlayerCharacter(string playerId, string characterId, int level, int health, int speed, int attackPower)
        {
            this.playerId = playerId;
            this.characterId = characterId;
            this.level = level;
            this.health = health;
            this.speed = speed;
            this.attackPower = attackPower;
        }
    }

    public class PlayerParty
    {
        public string playerId;
        public int partyIndex;
        public int slotIndex;
        public string characterId;

        public PlayerParty(string playerId, int partyIndex, int slotIndex, string characterId)
        {
            this.playerId = playerId;
            this.partyIndex = partyIndex;
            this.slotIndex = slotIndex;
            this.characterId = characterId;
        }
    }

    public class PlayerRequest
    {
        public string playerId;
        public string requestId;

        public PlayerRequest(string playerId, string requestId)
        {
            this.playerId = playerId;
            this.requestId = requestId;
        }
    }

    public class PlayerData
    {
        public string player_id;
        public string player_name;

        public int level;
        public int MAX_EXP;
        public int exp;

        //재화
        public int unicoin;
        public int cosmoston;
        public int oxygentank;
        public int Max_oxygentank;

        //재료
        public Dictionary<string, int> inventory;

        //건물
        public List<Structure> structures;
        public List<GameObject> setupBuildilngs;
        public int boundary; // 경계

        //캐릭터
        public List<Character> characters;

        //파티 편성
        public CharacterEnum[,] party;

        //의뢰
        public Request[] request;

        public PlayerData()
        {
            this.player_id = Social.localUser.id;
            this.player_name = "포핀";

            this.level = 0;
            this.MAX_EXP = 100;
            this.exp = 0;

            this.unicoin = 0;
            this.cosmoston = 0;
            this.oxygentank = 10;
            this.Max_oxygentank = 10;


            this.inventory = new Dictionary<string, int>();
            this.structures = new List<Structure>();
            this.characters = new List<Character> { new Character(CharacterEnum.Popin, 1, 50, 10, 6, "아무거나 적어놓는다") };

            party = new CharacterEnum[9, 3];
            party[0, 0] = CharacterEnum.Popin;

            request = new Request[4];

            this.boundary = 15;

            AddSampleDatas();
        }

        //따로 처리해줘야 하는 경험치는 아래 함수를 사용한다.
        public void UpdateExp(int value)
        {
            exp += value;

            while (exp < MAX_EXP)
            {
                this.level++;
                this.exp -= this.MAX_EXP;
                this.MAX_EXP = this.level * 100;
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
            structures.Add(DataManager.Instance.structures["Tree"].Clone());
            structures[0].id = 20;
            structures[0].setup = true;
            structures[0].position = new Vector2(-2f, 2.5f);
            structures.Add(DataManager.Instance.structures["House"].Clone());
            structures[1].id = 10;
            structures[1].setup = true;
            structures[1].position = new Vector2(2f, 1.6f);
            structures.Add(DataManager.Instance.structures["Tree"].Clone());
            structures[2].id = 21;
            structures[2].setup = false;
            structures[2].position = new Vector2(-4f, 2.5f);

            inventory.Add("붉은 꽃잎", 3);
            inventory.Add("블루베리", 2);

            unicoin += 100000;
        }

        public void SetBuildingImage()
        {
            foreach (GameObject obj in setupBuildilngs)
            {
                foreach (Structure strc in structures)
                {
                    if (strc.id == int.Parse(obj.name.Substring(0, obj.name.Length - 7)))
                    {
                        obj.GetComponent<SpriteRenderer>().sprite = strc.image;
                    }
                }
            }
        }
    }

}