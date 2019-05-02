using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;

public class BackendManager : MonoBehaviour
{

    public static BackendManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        Backend.Initialize(BRO =>
        {
            if (BRO.IsSuccess())
            {

            }
            else
            {
                Debug.Log("Failed");
            }
        });
    }

    #region Common

    public void CustomSignUp()
    {
        Backend.BMember.CustomSignUp("alchemyplanet", "3712");
    }

    public void CustomLogin()
    {
        Backend.BMember.CustomLogin("alchemyplanet", "3712");
    }

    public void CreateNickname()
    {
        Backend.BMember.CreateNickname("alchemyplanet");
    }

    private Dictionary<string, int> GetDictFromJsonData(JsonData jsonData)
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        foreach(string key in jsonData.Keys)
        {
            int value = int.Parse(jsonData[key]["N"].ToString());
            result.Add(key, value);
        }

        return result;
    }

    #endregion

    #region Table : player

    private void CreatePlayer(string playerId, string name)
    {
        Param param = new Param();
        param.Add("playerid", playerId);
        param.Add("name", name);
        param.Add("level", 1);
        param.Add("exp", 0);
        param.Add("uniCoin", 0);
        param.Add("cosmoStone", 0);
        param.Add("oxygenTank", 0);

        Backend.GameInfo.Insert("player", param);
    }

    public void TestCreatePlayer()
    {
        CreatePlayer("1", "bliss");
    }

    private void DestroyPlayer(string inDate)
    {
        Backend.GameInfo.Delete("player", inDate);
    }

    private void UpdatePlayerName(string inDate, string name)
    {
        Param param = new Param();
        param.Add("name", name);
        Backend.GameInfo.Update("player", inDate, param);
    }

    private void UpdatePlayerLevel(string inDate, int level)
    {
        Param param = new Param();
        param.Add("level", level);
        Backend.GameInfo.Update("player", inDate, param);
    }

    private void UpdatePlayerExp(string inDate, int exp)
    {
        Param param = new Param();
        param.Add("exp", exp);
        Backend.GameInfo.Update("player", inDate, param);
    }

    private void UpdatePlayerUniCoin(string inDate, int uniCoin)
    {
        Param param = new Param();
        param.Add("uniCoin", uniCoin);
        Backend.GameInfo.Update("player", inDate, param);
    }

    private void UpdatePlayerCosmoStone(string inDate, int cosmoStone)
    {
        Param param = new Param();
        param.Add("cosmoStone", cosmoStone);
        Backend.GameInfo.Update("player", inDate, param);
    }

    private void UpdatePlayerOxygenTank(string inDate, int oxygenTank)
    {
        Param param = new Param();
        param.Add("oxygenTank", oxygenTank);
        Backend.GameInfo.Update("player", inDate, param);
    }

    

    #endregion

    #region Table : item

    private void CreateItem(string playerId)
    {
        Param param = new Param();
        param.Add("playerId", playerId);

        Param[] items = new Param[0];
        param.Add("items", items);

        Backend.GameInfo.Insert("item", param);
    }

    private void DestroyItem(string inDate)
    {
        Backend.GameInfo.Delete("item", inDate);
    }

    private void AddItem(string inDate, string itemName, int number)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("item", inDate).GetReturnValuetoJSON()["row"][0]["items"]["L"];
        Param[] items = GetItemsFromJsonData(jsonData);

        Param[] result = new Param[items.Length + 1];
        Array.Copy(items, 0, result, 0, items.Length);

        Param newItem = new Param();
        newItem.Add("itemName", itemName);
        newItem.Add("number", number);
        result[items.Length] = newItem;

        Param param = new Param();
        param.Add("items", result);

        Backend.GameInfo.Update("item", inDate, param);
    }

    private void UpdateItemNumber(string inDate, string itemName, int number)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("item", inDate).GetReturnValuetoJSON()["row"][0]["items"]["L"];
        Param[] items = GetItemsFromJsonData(jsonData);

        SortedList[] itemsList = new SortedList[items.Length];
        for(int i = 0; i < itemsList.Length; i++)
        {
            itemsList[i] = items[i].GetValue();
        }

        for(int i = 0; i < items.Length; i++)
        {
            if(itemsList[i]["itemName"].ToString() == itemName)
            {
                Param newItem = new Param();
                newItem.Add("itemName", itemsList[i]["itemName"].ToString());
                newItem.Add("number", number);

                items[i] = newItem;
            }
        }

        Param param = new Param();
        param.Add("items", items);

        Backend.GameInfo.Update("item", inDate, param);
    }

    private void DeleteItem(string inDate, string itemName)
    {
        bool isDeleted = false;
        int count = 0;

        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("item", inDate).GetReturnValuetoJSON()["row"][0]["items"]["L"];
        Param[] items = GetItemsFromJsonData(jsonData);
        Param[] result = new Param[Math.Max(items.Length - 1, 0)];

        SortedList[] itemsList = new SortedList[items.Length];
        for (int i = 0; i < itemsList.Length; i++)
        {
            itemsList[i] = items[i].GetValue();
        }

        for(int i = 0; i < items.Length; i++)
        {
            if (itemsList[i]["itemName"].ToString() != itemName)
            {
                Param newItem = new Param();
                newItem.Add("itemName", itemsList[i]["itemName"].ToString());
                newItem.Add("number", int.Parse(itemsList[i]["number"].ToString()));

                result[count] = newItem;
                count++;
            }
            else
            {
                isDeleted = true;
            }
        }

        if (isDeleted == true)
        {
            Param param = new Param();
            param.Add("items", result);

            Backend.GameInfo.Update("item", inDate, param);
        }
    }

    private Param[] GetItemsFromJsonData(JsonData jsonData)
    {
        Param[] items = new Param[jsonData.Count];

        for (int i = 0; i < jsonData.Count; i++)
        {
            items[i] = new Param();
            items[i].Add("itemName", jsonData[i]["M"]["itemName"]["S"].ToString());
            items[i].Add("number", int.Parse(jsonData[i]["M"]["number"]["N"].ToString()));
        }

        return items;
    }

    #endregion

    #region Table : town

    private void CreateTown(string playerId)
    {
        Param param = new Param();
        param.Add("playerId", playerId);

        Param[] structures = new Param[0];
        param.Add("structures", structures);

        Backend.GameInfo.Insert("town", param);
    }

    private void DestroyTown(string inDate)
    {
        Backend.GameInfo.Delete("town", inDate);
    }

    private void AddTown(string inDate, string structureId, string buildingName, int level, int position, bool isConstructed, bool isFlipped, bool isUpgrading, DateTime endDate)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("town", inDate).GetReturnValuetoJSON()["row"][0]["structures"]["L"];
        Param[] structures = GetStructuresFromJsonData(jsonData);

        Param[] result = new Param[structures.Length + 1];
        Array.Copy(structures, 0, result, 0, structures.Length);

        Param newStructure = new Param();
        newStructure.Add("structureId", structureId);
        newStructure.Add("buildingName", buildingName);
        newStructure.Add("level", level);
        newStructure.Add("position", position);
        newStructure.Add("isConstructed", isConstructed);
        newStructure.Add("isFlipped", isFlipped);
        newStructure.Add("isUpgrading", isUpgrading);
        newStructure.Add("endDate", endDate.ToString());
        result[structures.Length] = newStructure;

        Param param = new Param();
        param.Add("structures", result);

        Backend.GameInfo.Update("town", inDate, param);
    }

    private void UpdateTownBuildingName(string inDate, string structureId, string buildingName)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("town", inDate).GetReturnValuetoJSON()["row"][0]["structures"]["L"];
        Param[] structures = GetStructuresFromJsonData(jsonData);

        SortedList[] structuresList = new SortedList[structures.Length];
        for(int i = 0; i < structuresList.Length; i++)
        {
            structuresList[i] = structures[i].GetValue();
        }

        for(int i = 0; i < structures.Length; i++)
        {
            if (structuresList[i]["structureId"].ToString() == structureId)
            {
                Param newStructure = new Param();
                newStructure.Add("structureId", structuresList[i]["structureId"].ToString());
                newStructure.Add("buildingName", buildingName);
                newStructure.Add("level", int.Parse(structuresList[i]["level"].ToString()));
                newStructure.Add("position", int.Parse(structuresList[i]["position"].ToString()));
                newStructure.Add("isConstructed", bool.Parse(structuresList[i]["isConstructed"].ToString()));
                newStructure.Add("isFlipped", bool.Parse(structuresList[i]["isFlipped"].ToString()));
                newStructure.Add("isUpgrading", bool.Parse(structuresList[i]["isUpgrading"].ToString()));
                newStructure.Add("endDate", structuresList[i]["endDate"].ToString());

                structures[i] = newStructure;
            }
        }

        Param param = new Param();
        param.Add("structures", structures);

        Backend.GameInfo.Update("town", inDate, param);
    }

    private void UpdateTownLevel(string inDate, string structureId, int level)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("town", inDate).GetReturnValuetoJSON()["row"][0]["structures"]["L"];
        Param[] structures = GetStructuresFromJsonData(jsonData);

        SortedList[] structuresList = new SortedList[structures.Length];
        for (int i = 0; i < structuresList.Length; i++)
        {
            structuresList[i] = structures[i].GetValue();
        }

        for (int i = 0; i < structures.Length; i++)
        {
            if (structuresList[i]["structureId"].ToString() == structureId)
            {
                Param newStructure = new Param();
                newStructure.Add("structureId", structuresList[i]["structureId"].ToString());
                newStructure.Add("buildingName", structuresList[i]["buildingName"].ToString());
                newStructure.Add("level", level);
                newStructure.Add("position", int.Parse(structuresList[i]["position"].ToString()));
                newStructure.Add("isConstructed", bool.Parse(structuresList[i]["isConstructed"].ToString()));
                newStructure.Add("isFlipped", bool.Parse(structuresList[i]["isFlipped"].ToString()));
                newStructure.Add("isUpgrading", bool.Parse(structuresList[i]["isUpgrading"].ToString()));
                newStructure.Add("endDate", structuresList[i]["endDate"].ToString());

                structures[i] = newStructure;
            }
        }

        Param param = new Param();
        param.Add("structures", structures);

        Backend.GameInfo.Update("town", inDate, param);
    }

    private void UpdateTownPosition(string inDate, string structureId, int position)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("town", inDate).GetReturnValuetoJSON()["row"][0]["structures"]["L"];
        Param[] structures = GetStructuresFromJsonData(jsonData);

        SortedList[] structuresList = new SortedList[structures.Length];
        for (int i = 0; i < structuresList.Length; i++)
        {
            structuresList[i] = structures[i].GetValue();
        }

        for (int i = 0; i < structures.Length; i++)
        {
            if (structuresList[i]["structureId"].ToString() == structureId)
            {
                Param newStructure = new Param();
                newStructure.Add("structureId", structuresList[i]["structureId"].ToString());
                newStructure.Add("buildingName", structuresList[i]["buildingName"].ToString());
                newStructure.Add("level", int.Parse(structuresList[i]["level"].ToString()));
                newStructure.Add("position", position);
                newStructure.Add("isConstructed", bool.Parse(structuresList[i]["isConstructed"].ToString()));
                newStructure.Add("isFlipped", bool.Parse(structuresList[i]["isFlipped"].ToString()));
                newStructure.Add("isUpgrading", bool.Parse(structuresList[i]["isUpgrading"].ToString()));
                newStructure.Add("endDate", structuresList[i]["endDate"].ToString());

                structures[i] = newStructure;
            }
        }

        Param param = new Param();
        param.Add("structures", structures);

        Backend.GameInfo.Update("town", inDate, param);
    }

    private void UpdateTownConstructed(string inDate, string structureId, bool isConstructed)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("town", inDate).GetReturnValuetoJSON()["row"][0]["structures"]["L"];
        Param[] structures = GetStructuresFromJsonData(jsonData);

        SortedList[] structuresList = new SortedList[structures.Length];
        for (int i = 0; i < structuresList.Length; i++)
        {
            structuresList[i] = structures[i].GetValue();
        }

        for (int i = 0; i < structures.Length; i++)
        {
            if (structuresList[i]["structureId"].ToString() == structureId)
            {
                Param newStructure = new Param();
                newStructure.Add("structureId", structuresList[i]["structureId"].ToString());
                newStructure.Add("buildingName", structuresList[i]["buildingName"].ToString());
                newStructure.Add("level", int.Parse(structuresList[i]["level"].ToString()));
                newStructure.Add("position", int.Parse(structuresList[i]["position"].ToString()));
                newStructure.Add("isConstructed", isConstructed);
                newStructure.Add("isFlipped", bool.Parse(structuresList[i]["isFlipped"].ToString()));
                newStructure.Add("isUpgrading", bool.Parse(structuresList[i]["isUpgrading"].ToString()));
                newStructure.Add("endDate", structuresList[i]["endDate"].ToString());

                structures[i] = newStructure;
            }
        }

        Param param = new Param();
        param.Add("structures", structures);

        Backend.GameInfo.Update("town", inDate, param);
    }

    private void UpdateTownFlipped(string inDate, string structureId, bool isFlipped)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("town", inDate).GetReturnValuetoJSON()["row"][0]["structures"]["L"];
        Param[] structures = GetStructuresFromJsonData(jsonData);

        SortedList[] structuresList = new SortedList[structures.Length];
        for (int i = 0; i < structuresList.Length; i++)
        {
            structuresList[i] = structures[i].GetValue();
        }

        for (int i = 0; i < structures.Length; i++)
        {
            if (structuresList[i]["structureId"].ToString() == structureId)
            {
                Param newStructure = new Param();
                newStructure.Add("structureId", structuresList[i]["structureId"].ToString());
                newStructure.Add("buildingName", structuresList[i]["buildingName"].ToString());
                newStructure.Add("level", int.Parse(structuresList[i]["level"].ToString()));
                newStructure.Add("position", int.Parse(structuresList[i]["position"].ToString()));
                newStructure.Add("isConstructed", bool.Parse(structuresList[i]["isConstructed"].ToString()));
                newStructure.Add("isFlipped", isFlipped);
                newStructure.Add("isUpgrading", bool.Parse(structuresList[i]["isUpgrading"].ToString()));
                newStructure.Add("endDate", structuresList[i]["endDate"].ToString());

                structures[i] = newStructure;
            }
        }

        Param param = new Param();
        param.Add("structures", structures);

        Backend.GameInfo.Update("town", inDate, param);
    }

    private void UpdateTownUpgrading(string inDate, string structureId, bool isUpgrading)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("town", inDate).GetReturnValuetoJSON()["row"][0]["structures"]["L"];
        Param[] structures = GetStructuresFromJsonData(jsonData);

        SortedList[] structuresList = new SortedList[structures.Length];
        for (int i = 0; i < structuresList.Length; i++)
        {
            structuresList[i] = structures[i].GetValue();
        }

        for (int i = 0; i < structures.Length; i++)
        {
            if (structuresList[i]["structureId"].ToString() == structureId)
            {
                Param newStructure = new Param();
                newStructure.Add("structureId", structuresList[i]["structureId"].ToString());
                newStructure.Add("buildingName", structuresList[i]["buildingName"].ToString());
                newStructure.Add("level", int.Parse(structuresList[i]["level"].ToString()));
                newStructure.Add("position", int.Parse(structuresList[i]["position"].ToString()));
                newStructure.Add("isConstructed", bool.Parse(structuresList[i]["isConstructed"].ToString()));
                newStructure.Add("isFlipped", bool.Parse(structuresList[i]["isFlipped"].ToString()));
                newStructure.Add("isUpgrading", isUpgrading);
                newStructure.Add("endDate", structuresList[i]["endDate"].ToString());

                structures[i] = newStructure;
            }
        }

        Param param = new Param();
        param.Add("structures", structures);

        Backend.GameInfo.Update("town", inDate, param);
    }

    private void UpdateTownEndDate(string inDate, string structureId, DateTime endDate)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("town", inDate).GetReturnValuetoJSON()["row"][0]["structures"]["L"];
        Param[] structures = GetStructuresFromJsonData(jsonData);

        SortedList[] structuresList = new SortedList[structures.Length];
        for (int i = 0; i < structuresList.Length; i++)
        {
            structuresList[i] = structures[i].GetValue();
        }

        for (int i = 0; i < structures.Length; i++)
        {
            if (structuresList[i]["structureId"].ToString() == structureId)
            {
                Param newStructure = new Param();
                newStructure.Add("structureId", structuresList[i]["structureId"].ToString());
                newStructure.Add("buildingName", structuresList[i]["buildingName"].ToString());
                newStructure.Add("level", int.Parse(structuresList[i]["level"].ToString()));
                newStructure.Add("position", int.Parse(structuresList[i]["position"].ToString()));
                newStructure.Add("isConstructed", bool.Parse(structuresList[i]["isConstructed"].ToString()));
                newStructure.Add("isFlipped", bool.Parse(structuresList[i]["isFlipped"].ToString()));
                newStructure.Add("isUpgrading", bool.Parse(structuresList[i]["isUpgrading"].ToString()));
                newStructure.Add("endDate", endDate.ToString());

                structures[i] = newStructure;
            }
        }

        Param param = new Param();
        param.Add("structures", structures);

        Backend.GameInfo.Update("town", inDate, param);
    }

    private void DeleteTown(string inDate, string structureId)
    {
        bool isDeleted = false;
        int count = 0;

        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("town", inDate).GetReturnValuetoJSON()["row"][0]["structures"]["L"];
        Param[] structures = GetStructuresFromJsonData(jsonData);
        Param[] result = new Param[Math.Max(structures.Length - 1, 0)];

        SortedList[] structuresList = new SortedList[structures.Length];
        for (int i = 0; i < structuresList.Length; i++)
        {
            structuresList[i] = structures[i].GetValue();
        }

        for(int i = 0; i < structures.Length; i++)
        {
            if(structuresList[i]["structureId"].ToString() != structureId)
            {
                Param newStructure = new Param();
                newStructure.Add("structureId", structuresList[i]["structureId"].ToString());
                newStructure.Add("buildingName", structuresList[i]["buildingName"].ToString());
                newStructure.Add("level", int.Parse(structuresList[i]["level"].ToString()));
                newStructure.Add("position", int.Parse(structuresList[i]["position"].ToString()));
                newStructure.Add("isConstructed", bool.Parse(structuresList[i]["isConstructed"].ToString()));
                newStructure.Add("isFlipped", bool.Parse(structuresList[i]["isFlipped"].ToString()));
                newStructure.Add("isUpgrading", bool.Parse(structuresList[i]["isUpgrading"].ToString()));
                newStructure.Add("endDate", structuresList[i]["endDate"].ToString());

                result[count] = newStructure;
                count++;
            }
            else
            {
                isDeleted = true;
            }
        }

        if(isDeleted == true)
        {
            Param param = new Param();
            param.Add("structures", result);

            Backend.GameInfo.Update("town", inDate, param);
        }
    }

    private Param[] GetStructuresFromJsonData(JsonData jsonData)
    {
        Param[] structures = new Param[jsonData.Count];

        for (int i = 0; i < jsonData.Count; i++)
        {
            structures[i] = new Param();
            structures[i].Add("structureId", jsonData[i]["M"]["structureId"]["S"].ToString());
            structures[i].Add("buildingName", jsonData[i]["M"]["buildingName"]["S"].ToString());
            structures[i].Add("level", int.Parse(jsonData[i]["M"]["level"]["N"].ToString()));
            structures[i].Add("position", int.Parse(jsonData[i]["M"]["position"]["N"].ToString()));
            structures[i].Add("isConstructed", bool.Parse(jsonData[i]["M"]["isConstructed"]["BOOL"].ToString()));
            structures[i].Add("isFlipped", bool.Parse(jsonData[i]["M"]["isFlipped"]["BOOL"].ToString()));
            structures[i].Add("isUpgrading", bool.Parse(jsonData[i]["M"]["isUpgrading"]["BOOL"].ToString()));
            structures[i].Add("endDate", jsonData[i]["M"]["endDate"]["S"].ToString());
        }

        return structures;
    }

    #endregion

    #region Table : character

    private void CreateCharacter(string playerId)
    {
        Param param = new Param();
        param.Add("playerId", playerId);

        Param[] characters = new Param[0];
        param.Add("characters", characters);

        Backend.GameInfo.Insert("character", param);
    }

    private void DestroyCharacter(string inDate)
    {
        Backend.GameInfo.Delete("character", inDate);
    }

    private void AddCharacter(string inDate, string characterName, int level, int health, int speed, int attackPower)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("character", inDate).GetReturnValuetoJSON()["row"][0]["characters"]["L"];
        Param[] characters = GetCharactersFromJsonData(jsonData);

        Param[] result = new Param[characters.Length + 1];
        Array.Copy(characters, 0, result, 0, characters.Length);

        Param newCharacter = new Param();
        newCharacter.Add("characterName", characterName);
        newCharacter.Add("level", level);
        newCharacter.Add("health", health);
        newCharacter.Add("speed", speed);
        newCharacter.Add("attackPower", attackPower);
        result[characters.Length] = newCharacter;

        Param param = new Param();
        param.Add("characters", result);

        Backend.GameInfo.Update("character", inDate, param);
    }

    private void UpdateCharacterLevel(string inDate, string characterName, int level)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("character", inDate).GetReturnValuetoJSON()["row"][0]["characters"]["L"];
        Param[] characters = GetCharactersFromJsonData(jsonData);

        SortedList[] charactersList = new SortedList[characters.Length];
        for (int i = 0; i < charactersList.Length; i++)
        {
            charactersList[i] = characters[i].GetValue();
        }

        for (int i = 0; i < characters.Length; i++)
        {
            if (charactersList[i]["characterName"].ToString() == characterName)
            {
                Param newCharacter = new Param();
                newCharacter.Add("characterName", charactersList[i]["characterName"].ToString());
                newCharacter.Add("level", level);
                newCharacter.Add("health", int.Parse(charactersList[i]["health"].ToString()));
                newCharacter.Add("speed", int.Parse(charactersList[i]["speed"].ToString()));
                newCharacter.Add("attackPower", int.Parse(charactersList[i]["attackPower"].ToString()));

                characters[i] = newCharacter;
            }
        }

        Param param = new Param();
        param.Add("characters", characters);

        Backend.GameInfo.Update("character", inDate, param);
    }

    private void UpdateCharacterHealth(string inDate, string characterName, int health)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("character", inDate).GetReturnValuetoJSON()["row"][0]["characters"]["L"];
        Param[] characters = GetCharactersFromJsonData(jsonData);

        SortedList[] charactersList = new SortedList[characters.Length];
        for (int i = 0; i < charactersList.Length; i++)
        {
            charactersList[i] = characters[i].GetValue();
        }

        for (int i = 0; i < characters.Length; i++)
        {
            if (charactersList[i]["characterName"].ToString() == characterName)
            {
                Param newCharacter = new Param();
                newCharacter.Add("characterName", charactersList[i]["characterName"].ToString());
                newCharacter.Add("level", int.Parse(charactersList[i]["level"].ToString()));
                newCharacter.Add("health", health);
                newCharacter.Add("speed", int.Parse(charactersList[i]["speed"].ToString()));
                newCharacter.Add("attackPower", int.Parse(charactersList[i]["attackPower"].ToString()));

                characters[i] = newCharacter;
            }
        }

        Param param = new Param();
        param.Add("characters", characters);

        Backend.GameInfo.Update("character", inDate, param);
    }

    private void UpdateCharacterSpeed(string inDate, string characterName, int speed)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("character", inDate).GetReturnValuetoJSON()["row"][0]["characters"]["L"];
        Param[] characters = GetCharactersFromJsonData(jsonData);

        SortedList[] charactersList = new SortedList[characters.Length];
        for (int i = 0; i < charactersList.Length; i++)
        {
            charactersList[i] = characters[i].GetValue();
        }

        for (int i = 0; i < characters.Length; i++)
        {
            if (charactersList[i]["characterName"].ToString() == characterName)
            {
                Param newCharacter = new Param();
                newCharacter.Add("characterName", charactersList[i]["characterName"].ToString());
                newCharacter.Add("level", int.Parse(charactersList[i]["level"].ToString()));
                newCharacter.Add("health", int.Parse(charactersList[i]["health"].ToString()));
                newCharacter.Add("speed", speed);
                newCharacter.Add("attackPower", int.Parse(charactersList[i]["attackPower"].ToString()));

                characters[i] = newCharacter;
            }
        }

        Param param = new Param();
        param.Add("characters", characters);

        Backend.GameInfo.Update("character", inDate, param);
    }

    private void UpdateCharacterAttackPower(string inDate, string characterName, int attackPower)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("character", inDate).GetReturnValuetoJSON()["row"][0]["characters"]["L"];
        Param[] characters = GetCharactersFromJsonData(jsonData);

        SortedList[] charactersList = new SortedList[characters.Length];
        for (int i = 0; i < charactersList.Length; i++)
        {
            charactersList[i] = characters[i].GetValue();
        }

        for (int i = 0; i < characters.Length; i++)
        {
            if (charactersList[i]["characterName"].ToString() == characterName)
            {
                Param newCharacter = new Param();
                newCharacter.Add("characterName", charactersList[i]["characterName"].ToString());
                newCharacter.Add("level", int.Parse(charactersList[i]["level"].ToString()));
                newCharacter.Add("health", int.Parse(charactersList[i]["health"].ToString()));
                newCharacter.Add("speed", int.Parse(charactersList[i]["speed"].ToString()));
                newCharacter.Add("attackPower", attackPower);

                characters[i] = newCharacter;
            }
        }

        Param param = new Param();
        param.Add("characters", characters);

        Backend.GameInfo.Update("character", inDate, param);
    }

    private void DeleteCharacter(string inDate, string characterName)
    {
        bool isDeleted = false;
        int count = 0;

        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("character", inDate).GetReturnValuetoJSON()["row"][0]["characters"]["L"];
        Param[] characters = GetCharactersFromJsonData(jsonData);
        Param[] result = new Param[Math.Max(characters.Length - 1, 0)];

        SortedList[] charactersList = new SortedList[characters.Length];
        for (int i = 0; i < charactersList.Length; i++)
        {
            charactersList[i] = characters[i].GetValue();
        }

        for (int i = 0; i < characters.Length; i++)
        {
            if (charactersList[i]["characterName"].ToString() != characterName)
            {
                Param newCharacter = new Param();
                newCharacter.Add("characterName", charactersList[i]["characterName"].ToString());
                newCharacter.Add("level", int.Parse(charactersList[i]["level"].ToString()));
                newCharacter.Add("health", int.Parse(charactersList[i]["health"].ToString()));
                newCharacter.Add("speed", int.Parse(charactersList[i]["speed"].ToString()));
                newCharacter.Add("attackPower", int.Parse(charactersList[i]["attackPower"].ToString()));

                result[count] = newCharacter;
                count++;
            }
            else
            {
                isDeleted = true;
            }
        }

        if (isDeleted == true)
        {
            Param param = new Param();
            param.Add("characters", result);

            Backend.GameInfo.Update("character", inDate, param);
        }
    }

    private Param[] GetCharactersFromJsonData(JsonData jsonData)
    {
        Param[] characters = new Param[jsonData.Count];

        for (int i = 0; i < jsonData.Count; i++)
        {
            characters[i] = new Param();
            characters[i].Add("characterName", jsonData[i]["M"]["characterName"]["S"].ToString());
            characters[i].Add("level", int.Parse(jsonData[i]["M"]["level"]["N"].ToString()));
            characters[i].Add("health", int.Parse(jsonData[i]["M"]["health"]["N"].ToString()));
            characters[i].Add("speed", int.Parse(jsonData[i]["M"]["speed"]["N"].ToString()));
            characters[i].Add("attackPower", int.Parse(jsonData[i]["M"]["attackPower"]["N"].ToString()));
        }

        return characters;
    }

    #endregion

    #region Table : party

    private void CreateParty(string playerId)
    {
        Param param = new Param();
        param.Add("playerId", playerId);

        Param[] parties = new Param[0];
        param.Add("parties", parties);

        Backend.GameInfo.Insert("party", param);
    }

    private void DestroyParty(string inDate)
    {
        Backend.GameInfo.Delete("party", inDate);
    }

    private void AddParty(string inDate, int number, string firstCharacterName, string secondCharacterName, string thirdCharacterName)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("party", inDate).GetReturnValuetoJSON()["row"][0]["parties"]["L"];
        Param[] parties = GetPartiesFromJsonData(jsonData);

        Param[] result = new Param[parties.Length + 1];
        Array.Copy(parties, 0, result, 0, parties.Length);

        Param newParty = new Param();
        newParty.Add("number", number);
        newParty.Add("firstCharacterName", firstCharacterName);
        newParty.Add("secondCharacterName", secondCharacterName);
        newParty.Add("thirdCharacterName", thirdCharacterName);
        result[parties.Length] = newParty;

        Param param = new Param();
        param.Add("parties", result);

        Backend.GameInfo.Update("party", inDate, param);
    }

    private void UpdatePartyFirstCharacterName(string inDate, int number, string firstCharacterName)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("party", inDate).GetReturnValuetoJSON()["row"][0]["parties"]["L"];
        Param[] parties = GetPartiesFromJsonData(jsonData);

        SortedList[] partiesList = new SortedList[parties.Length];
        for (int i = 0; i < partiesList.Length; i++)
        {
            partiesList[i] = parties[i].GetValue();
        }

        for (int i = 0; i < parties.Length; i++)
        {
            if (int.Parse(partiesList[i]["number"].ToString()) == number)
            {
                Param newParty = new Param();
                newParty.Add("number", int.Parse(partiesList[i]["number"].ToString()));
                newParty.Add("firstCharacterName", firstCharacterName);
                newParty.Add("secondCharacterName", partiesList[i]["secondCharacterName"].ToString());
                newParty.Add("thirdCharacterName", partiesList[i]["thirdCharacterName"].ToString());

                parties[i] = newParty;
            }
        }

        Param param = new Param();
        param.Add("parties", parties);

        Backend.GameInfo.Update("party", inDate, param);
    }

    private void UpdatePartySecondCharacterName(string inDate, int number, string secondCharacterName)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("party", inDate).GetReturnValuetoJSON()["row"][0]["parties"]["L"];
        Param[] parties = GetPartiesFromJsonData(jsonData);

        SortedList[] partiesList = new SortedList[parties.Length];
        for (int i = 0; i < partiesList.Length; i++)
        {
            partiesList[i] = parties[i].GetValue();
        }

        for (int i = 0; i < parties.Length; i++)
        {
            if (int.Parse(partiesList[i]["number"].ToString()) == number)
            {
                Param newParty = new Param();
                newParty.Add("number", int.Parse(partiesList[i]["number"].ToString()));
                newParty.Add("firstCharacterName", partiesList[i]["firstCharacterName"].ToString());
                newParty.Add("secondCharacterName", secondCharacterName);
                newParty.Add("thirdCharacterName", partiesList[i]["thirdCharacterName"].ToString());

                parties[i] = newParty;
            }
        }

        Param param = new Param();
        param.Add("parties", parties);

        Backend.GameInfo.Update("party", inDate, param);
    }

    private void UpdatePartyThirdCharacterName(string inDate, int number, string thirdCharacterName)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("party", inDate).GetReturnValuetoJSON()["row"][0]["parties"]["L"];
        Param[] parties = GetPartiesFromJsonData(jsonData);

        SortedList[] partiesList = new SortedList[parties.Length];
        for (int i = 0; i < partiesList.Length; i++)
        {
            partiesList[i] = parties[i].GetValue();
        }

        for (int i = 0; i < parties.Length; i++)
        {
            if (int.Parse(partiesList[i]["number"].ToString()) == number)
            {
                Param newParty = new Param();
                newParty.Add("number", int.Parse(partiesList[i]["number"].ToString()));
                newParty.Add("firstCharacterName", partiesList[i]["firstCharacterName"].ToString());
                newParty.Add("secondCharacterName", partiesList[i]["secondCharacterName"].ToString());
                newParty.Add("thirdCharacterName", thirdCharacterName);

                parties[i] = newParty;
            }
        }

        Param param = new Param();
        param.Add("parties", parties);

        Backend.GameInfo.Update("party", inDate, param);
    }

    private void DeleteParty(string inDate, int number)
    {
        bool isDeleted = false;
        int count = 0;

        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("party", inDate).GetReturnValuetoJSON()["row"][0]["parties"]["L"];
        Param[] parties = GetPartiesFromJsonData(jsonData);
        Param[] result = new Param[Math.Max(parties.Length - 1, 0)];

        SortedList[] partiesList = new SortedList[parties.Length];
        for (int i = 0; i < partiesList.Length; i++)
        {
            partiesList[i] = parties[i].GetValue();
        }

        for (int i = 0; i < parties.Length; i++)
        {
            if (int.Parse(partiesList[i]["number"].ToString()) != number)
            {
                Param newParty = new Param();
                newParty.Add("number", int.Parse(partiesList[i]["number"].ToString()));
                newParty.Add("firstCharacterName", partiesList[i]["firstCharacterName"].ToString());
                newParty.Add("secondCharacterName", partiesList[i]["secondCharacterName"].ToString());
                newParty.Add("thirdCharacterName", partiesList[i]["thirdCharacterName"].ToString());

                result[count] = newParty;
                count++;
            }
            else
            {
                isDeleted = true;
            }
        }

        if (isDeleted == true)
        {
            Param param = new Param();
            param.Add("parties", result);

            Backend.GameInfo.Update("party", inDate, param);
        }
    }

    private Param[] GetPartiesFromJsonData(JsonData jsonData)
    {
        Param[] parties = new Param[jsonData.Count];

        for (int i = 0; i < jsonData.Count; i++)
        {
            parties[i] = new Param();
            parties[i].Add("number", int.Parse(jsonData[i]["M"]["number"]["N"].ToString()));
            parties[i].Add("firstCharacterName", jsonData[i]["M"]["firstCharacterName"]["S"].ToString());
            parties[i].Add("secondCharacterName", jsonData[i]["M"]["secondCharacterName"]["S"].ToString());
            parties[i].Add("thirdCharacterName", jsonData[i]["M"]["thirdCharacterName"]["S"].ToString());
        }

        return parties;
    }

    #endregion

    #region Table : request

    #endregion

    #region Table : stage

    #endregion
}