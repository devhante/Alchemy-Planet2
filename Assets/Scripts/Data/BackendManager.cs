using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

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

    private void InsertPlayer(string playerId, string name)
    {
        Param param = new Param();
        param.Add("playerid", playerId);
        param.Add("name", name);
        param.Add("level", 1);
        param.Add("exp", 0);
        param.Add("uniCoin", 0);
        param.Add("cosmoStone", 0);
        param.Add("oxygenTank", 0);

        BackendReturnObject bro = Backend.GameInfo.Insert("player", param);
        if (bro.IsSuccess()) Debug.Log("Insert 성공");
        else Debug.Log("Insert 실패");
    }

    public void TestInsertPlayer()
    {
        string playerId = "1";
        string name = "bliss";

        Param param = new Param();
        param.Add("playerid", playerId);
        param.Add("name", name);
        param.Add("level", 1);
        param.Add("exp", 0);
        param.Add("uniCoin", 0);
        param.Add("cosmoStone", 0);
        param.Add("oxygenTank", 0);

        BackendReturnObject bro = Backend.GameInfo.Insert("player", param);
        if (bro.IsSuccess()) Debug.Log("Insert 성공");
        else Debug.Log("Insert 실패");
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

    private void DeletePlayer(string inDate)
    {
        Backend.GameInfo.Delete("player", inDate);
    }

    #endregion

    #region Table : item

    private void CreateItem(string playerId)
    {
        Param param = new Param();
        param.Add("playerId", playerId);
        Dictionary<string, int> dict = new Dictionary<string, int>();
        param.Add("itemDict", dict);

        BackendReturnObject bro = Backend.GameInfo.Insert("item", param);
    }

    private void DestroyItem(string inDate, string itemName)
    {
        Backend.GameInfo.Delete("item", inDate);
    }

    private void UpdateItem(string inDate, string itemName, int number)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("item", inDate).GetReturnValuetoJSON()["row"][0]["itemDict"]["M"];
        Dictionary<string, int> dict = GetDictFromJsonData(jsonData);
        dict[itemName] = number;

        Param param = new Param();
        param.Add("itemDict", dict);

        Backend.GameInfo.Update("item", inDate, param);
    }

    private void DeleteItem(string inDate, string itemName, int number)
    {
        JsonData jsonData = Backend.GameInfo.GetContentsByIndate("item", inDate).GetReturnValuetoJSON()["row"][0]["itemDict"]["M"];
        Dictionary<string, int> dict = GetDictFromJsonData(jsonData);
        dict.Remove(itemName);

        Param param = new Param();
        param.Add("itemDict", dict);

        Backend.GameInfo.Update("item", inDate, param);
    }

    #endregion

    #region Table : town

    #endregion

    #region Table : character

    #endregion

    #region Table : party

    #endregion

    #region Table : request

    #endregion

    #region Table : stage

    #endregion
}