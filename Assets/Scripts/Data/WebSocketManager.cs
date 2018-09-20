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

                if (message.status == "PlayerName")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionName>(data_string);

                    Debug.Log(data.playerName);
                }

                if (message.status == "PlayerLevel")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionLevel>(data_string);

                    Debug.Log(data.level);
                }

                if (message.status == "PlayerGoods")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionGoods>(data_string);

                    Debug.Log(data.uniCoin + " | " + data.cosmoStone + " | " + data.oxygenTank);
                }

                if (message.status == "PlayerItem")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionItem[]>(data_string);

                    Debug.Log(data[0].itemId + " | " + data[1].itemId + " | " + data[2].itemId);
                }

                if (message.status == "PlayerStructure")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionStructure[]>(data_string);

                    Debug.Log(data[0].playerStructureId + " | " + data[1].playerStructureId + " | " + data[2].playerStructureId);
                }

                if (message.status == "PlayerCharacter")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionCharacter[]>(data_string);

                    Debug.Log(data[0].level + " | " + data[1].level + " | " + data[2].level);
                }

                if (message.status == "7100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionParty[]>(data_string);

                    UnityMainThreadDispatcher.Instance().Enqueue(() => CharacterScene.GameManager.Instance.InitParty(data));
                }

                if (message.status == "PlayerRequest")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionRequest[]>(data_string);

                    Debug.Log(data[0].requestId + " | " + data[1].requestId + " | " + data[2].requestId);
                }
            };

            ws.Connect();


            if(ws.IsConnected)
            {
                //SendDropTable("PlayerName");
                SendFindName("0", "0");
                SendFindLevel("0", "0");
            }
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

        private void SendDropTable(string tableName)
        {
            var message = new Message("dropTable", tableName);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #region Send_Name

        private void SendFindName(string status, string playerId)
        {
            var data = new CollectionName(playerId, "");
            var message = new Message("110" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertName(string status, string playerId, string playerName)
        {
            var data = new CollectionName(playerId, playerName);
            var message = new Message("120" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeleteName(string status, string playerId)
        {
            var data = new CollectionName(playerId, "");
            var message = new Message("130" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdateName(string status, string playerId, string playerName)
        {
            var data = new CollectionName(playerId, playerName);
            var message = new Message("140" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Name

        #region Send_Level

        private void SendFindLevel(string status, string playerId)
        {
            var data = new CollectionLevel(playerId, 0, 0);
            var message = new Message("210" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertLevel(string status, string playerId, int level, int exp)
        {
            var data = new CollectionLevel(playerId, level, exp);
            var message = new Message("220" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeleteLevel(string status, string playerId)
        {
            var data = new CollectionLevel(playerId, 0, 0);
            var message = new Message("230" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdateLevel(string status, string playerId, int level, int exp)
        {
            var data = new CollectionLevel(playerId, level, exp);
            var message = new Message("240" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Level

        #region Send_Goods

        private void SendFindGoods(string status, string playerId)
        {
            var data = new CollectionGoods(playerId, 0, 0, 0);
            var message = new Message("310" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertGoods(string status, string playerId, int unicoin, int cosmostone, int oxygentank)
        {
            var data = new CollectionGoods(playerId, unicoin, cosmostone, oxygentank);
            var message = new Message("320" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeleteGoods(string status, string playerId)
        {
            var data = new CollectionGoods(playerId, 0, 0, 0);
            var message = new Message("330" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdateGoods(string status, string playerId, int unicoin, int cosmostone, int oxygentank)
        {
            var data = new CollectionGoods(playerId, unicoin, cosmostone, oxygentank);
            var message = new Message("340" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Goods

        #region Send_Item

        private void SendFindItems(string status, string playerId)
        {
            var data = new CollectionItem(playerId, "", 0);
            var message = new Message("410" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        
        private void SendInsertItem(string status, string playerId, string itemId, int count)
        {
            var data = new CollectionItem(playerId, itemId, count);
            var message = new Message("420" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        private void SendDeleteItem(string status, string playerId, string itemId)
        {
            var data = new CollectionItem(playerId, itemId, 0);
            var message = new Message("430" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        private void SendDeleteItems(string status, string playerId)
        {
            var data = new CollectionItem(playerId, "", 0);
            var message = new Message("431" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        private void SendUpdateItem(string status, string playerId, string itemId, int count)
        {
            var data = new CollectionItem(playerId, itemId, count);
            var message = new Message("440" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Item

        #region Send_PlayerStructure
        
        private void SendFindStructures(string status, string playerId)
        {
            var data = new CollectionStructure(playerId, "", "", 0, 0, false, false, false, new DateTime());
            var message = new Message("510" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertStructure(string status, string playerId, string playerStructureId, string structureId, int level, int position, bool isConstructed, bool isFlipped, bool isUpgrading, DateTime endDate)
        {
            var data = new CollectionStructure(playerId, playerStructureId, structureId, level, position, isConstructed, isFlipped, isUpgrading, endDate);
            var message = new Message("520" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeleteStructure(string status, string playerId, string playerStructureId)
        {
            var data = new CollectionStructure(playerId, playerStructureId, "", 0, 0, false, false, false, new DateTime());
            var message = new Message("530" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeleteStructures(string status, string playerId)
        {
            var data = new CollectionStructure(playerId, "", "", 0, 0, false, false, false, new DateTime());
            var message = new Message("531" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdateStructure(string status, string playerId, string playerStructureId, string structureId, int level, int position, bool isConstructed, bool isFlipped, bool isUpgrading, DateTime endDate)
        {
            var data = new CollectionStructure(playerId, playerStructureId, structureId, level, position, isConstructed, isFlipped, isUpgrading, endDate);
            var message = new Message("540" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_PlayerStructure

        #region Send_Character

        private void SendFindCharacters(string status, string playerId)
        {
            var data = new CollectionCharacter(playerId, "", 0, 0, 0, 0);
            var message = new Message("610" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertCharacter(string status, string playerId, string characterId, int level, int health, int speed, int attackPower)
        {
            var data = new CollectionCharacter(playerId, characterId, level, health, speed, attackPower);
            var message = new Message("620" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeleteCharacter(string status, string playerId, string characterId)
        {
            var data = new CollectionCharacter(playerId, characterId, 0, 0, 0, 0);
            var message = new Message("630" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeleteCharacters(string status, string playerId)
        {
            var data = new CollectionCharacter(playerId, "", 0, 0, 0, 0);
            var message = new Message("631" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdateCharacter(string status, string playerId, string characterId, int level, int health, int speed, int attackPower)
        {
            var data = new CollectionCharacter(playerId, characterId, level, health, speed, attackPower);
            var message = new Message("640" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Character

        #region Send_Party

        public void SendFindParties(string status, string playerId)
        {
            var data = new CollectionParty(playerId, 0, 0, "");
            var message = new Message("710" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertParty(string status, string playerId, int partyIndex, int slotIndex, string characterId)
        {
            var data = new CollectionParty(playerId, partyIndex, slotIndex, characterId);
            var message = new Message("720" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteParty(string status, string playerId, int partyIndex, int slotIndex)
        {
            var data = new CollectionParty(playerId, partyIndex, slotIndex, "");
            var message = new Message("730" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteParties(string status, string playerId)
        {
            var data = new CollectionParty(playerId, 0, 0, "");
            var message = new Message("731" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdatePlayerParty(string status, string playerId, int partyIndex, int slotIndex, string characterId)
        {
            var data = new CollectionParty(playerId, partyIndex, slotIndex, characterId);
            var message = new Message("740" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Party

        #region Send_Request

        private void SendFindRequests(string status, string playerId)
        {
            var data = new CollectionRequest(playerId, "");
            var message = new Message("810" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertRequest(string status, string playerId, string requestId)
        {
            var data = new CollectionRequest(playerId, requestId);
            var message = new Message("820" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeleteRequest(string status, string playerId, string requestId)
        {
            var data = new CollectionRequest(playerId, requestId);
            var message = new Message("830" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeleteRequests(string status, string playerId)
        {
            var data = new CollectionRequest(playerId, "");
            var message = new Message("831" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Request
    }
}