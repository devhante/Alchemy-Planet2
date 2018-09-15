using AlchemyPlanet.TownScene;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace AlchemyPlanet.Data
{
    public class WebSocketManager : MonoBehaviour
    {
        public static WebSocketManager Instance { get; private set; }
        public WebSocket ws;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        private void Start()
        {
            ws = new WebSocket("ws://54.180.47.195:3000/");

            ws.OnMessage += (sender, e) =>
            {
                Debug.Log(e.Data);
                var message = JsonConvert.DeserializeObject<Message>(e.Data);
                
                if (message.status == "StructureUpgradeInfo")
                {
                    Debug.Log(message.data);
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<StructureUpgradeInfo>(data_string);
                    Debug.Log("TEST_TEST_TEST_TEST_TEST");
                    OnMessageGetStructureUpgradeInfo(data);
                }
                if (message.status == "PlayerName")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<PlayerName>(data_string);
                    Debug.Log(data.playerName);
                }

            };

            ws.Connect();
            //SendGetStructureUpgradeInfo("0", 1);

            SendInsertPlayerName("0", "포핀");
            SendUpdatePlayerName("0", "조경욱");
            SendFindPlayerName("0");
            Debug.Log("Sended");
        }

        private void OnDestroy()
        {
            Instance = null;
            ws.Close();
        }

        private string ConvertLappedJsonString(string Jsonstring)
        {
            string BackSlashDeleted = Jsonstring.Replace("\\\"", "\"");
            return BackSlashDeleted.Remove(0, 1).Remove(BackSlashDeleted.Length - 2);
        }

        #region Send_PlayerName

        private void SendFindPlayerName(string player_id)
        {
            var data = new PlayerName(player_id, "");
            var message = new Message("findPlayerName", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertPlayerName(string player_id, string player_name)
        {
            var data = new PlayerName(player_id, player_name);
            var message = new Message("insertPlayerName", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerName(string player_id)
        {
            var data = new PlayerName(player_id, "");
            var message = new Message("deletePlayerName", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdatePlayerName(string player_id, string player_name)
        {
            var data = new PlayerName(player_id, player_name);
            var message = new Message("updatePlayerName", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_PlayerName
        

        private void SendGetStructureUpgradeInfo(string uid, int sid)
        {
            var data = new StructureUpgradeInfo(uid, sid, new DateTime(), 0);
            var message = new Message("GetStructureUpgradeInfo", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        private void OnMessageGetStructureUpgradeInfo(StructureUpgradeInfo data)
        {
            Debug.Log(data);
        }

        private void SendSetStructureUpgradeInfo(string uid, int sid, DateTime sDate, int rTime)
        {
            var data = new StructureUpgradeInfo(uid, sid, sDate, rTime);
            var message = new Message("SetStructureUpgradeInfo", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
    }
}