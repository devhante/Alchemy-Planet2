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

    public void CustomSignUp()
    {
        Backend.BMember.CustomSignUp("alchemyplanet", "3712");
    }

    public void CustomLogin()
    {
        Backend.BMember.CustomLogin("alchemyplanet", "3712");
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

    // TABLE: player
    
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
        if (bro.IsSuccess()) Debug.Log(playerId + "번" + name + " 삽입 성공");
        else Debug.Log(playerId + "번" + name + " 삽입 실패");
    }
}
