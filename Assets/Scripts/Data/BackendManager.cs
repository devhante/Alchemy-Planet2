using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class BackendManager : MonoBehaviour {

	void Start () {
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

    private string[] GetPrivateTableList()
    {
        string[] result;

        BackendReturnObject bro = Backend.GameInfo.GetTableList();
        JsonData privateTables = bro.GetReturnValuetoJSON()["privateTables"];

        result = new string[privateTables.Count];
        for(int i = 0; i < result.Length; i++)
        {
            result[i] = privateTables[i].ToString();
        }

        return result;
    }

    private string[] GetPublicTableList()
    {
        string[] result;

        BackendReturnObject bro = Backend.GameInfo.GetTableList();
        JsonData privateTables = bro.GetReturnValuetoJSON()["publicTables"];

        result = new string[privateTables.Count];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = privateTables[i].ToString();
        }

        return result;
    }

    private BackendReturnObject GetPrivateTableContents(string tableName)
    {
        return Backend.GameInfo.GetPrivateContents(tableName);
    }

    private string GetInDateByNickName(string nickname)
    {
        return Backend.Social.GetGamerIndateByNickname(nickname).GetReturnValuetoJSON()["rows"][0]["inDate"]["S"].ToString();
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
        if (bro.IsSuccess()) Debug.Log(playerId + "번 " + name + " 삽입 성공");
        else Debug.Log(playerId + "번 " + name + " 삽입 실패");
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
        if (bro.IsSuccess()) Debug.Log(playerId + "번 " + name + " 삽입 성공");
        else Debug.Log(playerId + "번 " + name + " 삽입 실패");
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
}
