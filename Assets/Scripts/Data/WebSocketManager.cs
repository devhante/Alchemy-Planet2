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
        private WebSocket ws;

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
                var message = JsonConvert.DeserializeObject<Message>(e.Data);

                if (message.status == "1100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionName>(data_string);

                    UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitName(data));
                }

                if (message.status == "2100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionLevel>(data_string);

                    UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitLevel(data));
                }

                if (message.status == "3100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionGoods>(data_string);

                    UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitGoods(data));
                }

                if (message.status == "4100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionItem[]>(data_string);

                    UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitItem(data));
                }

                if (message.status == "5100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionStructure[]>(data_string);

                    //
                }

                if (message.status == "6100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionCharacter[]>(data_string);

                    UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitCharacter(data));
                }

                if (message.status == "7100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionParty[]>(data_string);

                    UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitParty(data));
                }

                if (message.status == "8100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionRequest[]>(data_string);
                    
                    //
                }
            };

            ws.Connect();


            if(ws.IsConnected)
            {
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

        public void SendDropTable(string tableName)
        {
            var message = new Message("dropTable", tableName);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #region Send_Name

        public void SendFindName(string status, string playerId)
        {
            var data = new CollectionName(playerId, "");
            var message = new Message("110" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertName(string status, string playerId, string playerName)
        {
            var data = new CollectionName(playerId, playerName);
            var message = new Message("120" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteName(string status, string playerId)
        {
            var data = new CollectionName(playerId, "");
            var message = new Message("130" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdateName(string status, string playerId, string playerName)
        {
            var data = new CollectionName(playerId, playerName);
            var message = new Message("140" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Name

        #region Send_Level

        public void SendFindLevel(string status, string playerId)
        {
            var data = new CollectionLevel(playerId, 0, 0);
            var message = new Message("210" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertLevel(string status, string playerId, int level, int exp)
        {
            var data = new CollectionLevel(playerId, level, exp);
            var message = new Message("220" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteLevel(string status, string playerId)
        {
            var data = new CollectionLevel(playerId, 0, 0);
            var message = new Message("230" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdateLevel(string status, string playerId, int level, int exp)
        {
            var data = new CollectionLevel(playerId, level, exp);
            var message = new Message("240" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Level

        #region Send_Goods

        public void SendFindGoods(string status, string playerId)
        {
            var data = new CollectionGoods(playerId, 0, 0, 0);
            var message = new Message("310" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertGoods(string status, string playerId, int unicoin, int cosmostone, int oxygentank)
        {
            var data = new CollectionGoods(playerId, unicoin, cosmostone, oxygentank);
            var message = new Message("320" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteGoods(string status, string playerId)
        {
            var data = new CollectionGoods(playerId, 0, 0, 0);
            var message = new Message("330" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdateGoods(string status, string playerId, int unicoin, int cosmostone, int oxygentank)
        {
            var data = new CollectionGoods(playerId, unicoin, cosmostone, oxygentank);
            var message = new Message("340" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Goods

        #region Send_Item

        public void SendFindItems(string status, string playerId)
        {
            var data = new CollectionItem(playerId, "", 0);
            var message = new Message("410" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        
        public void SendInsertItem(string status, string playerId, string itemId, int count)
        {
            var data = new CollectionItem(playerId, itemId, count);
            var message = new Message("420" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        public void SendDeleteItem(string status, string playerId, string itemId)
        {
            var data = new CollectionItem(playerId, itemId, 0);
            var message = new Message("430" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        public void SendDeleteItems(string status, string playerId)
        {
            var data = new CollectionItem(playerId, "", 0);
            var message = new Message("431" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        public void SendUpdateItem(string status, string playerId, string itemId, int count)
        {
            var data = new CollectionItem(playerId, itemId, count);
            var message = new Message("440" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Item

        #region Send_PlayerStructure
        
        public void SendFindStructures(string status, string playerId)
        {
            var data = new CollectionStructure(playerId, "", "", 0, 0, false, false, false, new DateTime());
            var message = new Message("510" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertStructure(string status, string playerId, string playerStructureId, string structureId, int level, int position, bool isConstructed, bool isFlipped, bool isUpgrading, DateTime endDate)
        {
            var data = new CollectionStructure(playerId, playerStructureId, structureId, level, position, isConstructed, isFlipped, isUpgrading, endDate);
            var message = new Message("520" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteStructure(string status, string playerId, string playerStructureId)
        {
            var data = new CollectionStructure(playerId, playerStructureId, "", 0, 0, false, false, false, new DateTime());
            var message = new Message("530" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteStructures(string status, string playerId)
        {
            var data = new CollectionStructure(playerId, "", "", 0, 0, false, false, false, new DateTime());
            var message = new Message("531" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdateStructure(string status, string playerId, string playerStructureId, string structureId, int level, int position, bool isConstructed, bool isFlipped, bool isUpgrading, DateTime endDate)
        {
            var data = new CollectionStructure(playerId, playerStructureId, structureId, level, position, isConstructed, isFlipped, isUpgrading, endDate);
            var message = new Message("540" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_PlayerStructure

        #region Send_Character

        public void SendFindCharacters(string status, string playerId)
        {
            var data = new CollectionCharacter(playerId, "", 0, 0, 0, 0);
            var message = new Message("610" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertCharacter(string status, string playerId, string characterId, int level, int health, int speed, int attackPower)
        {
            var data = new CollectionCharacter(playerId, characterId, level, health, speed, attackPower);
            var message = new Message("620" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteCharacter(string status, string playerId, string characterId)
        {
            var data = new CollectionCharacter(playerId, characterId, 0, 0, 0, 0);
            var message = new Message("630" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteCharacters(string status, string playerId)
        {
            var data = new CollectionCharacter(playerId, "", 0, 0, 0, 0);
            var message = new Message("631" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdateCharacter(string status, string playerId, string characterId, int level, int health, int speed, int attackPower)
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

        public void SendFindRequests(string status, string playerId)
        {
            var data = new CollectionRequest(playerId, "");
            var message = new Message("810" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertRequest(string status, string playerId, string requestId)
        {
            var data = new CollectionRequest(playerId, requestId);
            var message = new Message("820" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteRequest(string status, string playerId, string requestId)
        {
            var data = new CollectionRequest(playerId, requestId);
            var message = new Message("830" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteRequests(string status, string playerId)
        {
            var data = new CollectionRequest(playerId, "");
            var message = new Message("831" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Request
    }
}