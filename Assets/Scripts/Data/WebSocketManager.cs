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

                if (message.status == "01100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionName>(data_string);
                    if(data != null)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitName(data));
                }

                if (message.status == "02100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionLevel>(data_string);
                    if (data != null)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitLevel(data));
                }

                if (message.status == "03100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionGoods>(data_string);
                    if (data != null)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitGoods(data));
                }

                if (message.status == "04100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionItem[]>(data_string);
                    if (data != null)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitItem(data));
                }

                if (message.status == "05100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionBuilding[]>(data_string);
                    if (data != null)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitBuilding(data));
                }

                if (message.status == "06100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionInterior[]>(data_string);
                    if (data != null)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitInterior(data));
                }

                if (message.status == "07100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionCharacter[]>(data_string);
                    if (data != null)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitCharacter(data));
                }

                if (message.status == "08100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionParty[]>(data_string);
                    if (data != null)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitParty(data));
                }

                if (message.status == "09100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionRequest[]>(data_string);
                    if (data != null)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitRequest(data));
                }

                if (message.status == "10100")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<CollectionStroyStar[]>(data_string);
                    if (data != null)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => DataManager.Instance.CommitStoryStar(data));
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
            var message = new Message("0110" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertName(string status, string playerId, string playerName)
        {
            var data = new CollectionName(playerId, playerName);
            var message = new Message("0120" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteName(string status, string playerId)
        {
            var data = new CollectionName(playerId, "");
            var message = new Message("0130" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdateName(string status, string playerId, string playerName)
        {
            var data = new CollectionName(playerId, playerName);
            var message = new Message("0140" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Name

        #region Send_Level

        public void SendFindLevel(string status, string playerId)
        {
            var data = new CollectionLevel(playerId, 0, 0);
            var message = new Message("0210" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertLevel(string status, string playerId, int level, int exp)
        {
            var data = new CollectionLevel(playerId, level, exp);
            var message = new Message("0220" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteLevel(string status, string playerId)
        {
            var data = new CollectionLevel(playerId, 0, 0);
            var message = new Message("0230" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdateLevel(string status, string playerId, int level, int exp)
        {
            var data = new CollectionLevel(playerId, level, exp);
            var message = new Message("0240" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Level

        #region Send_Goods

        public void SendFindGoods(string status, string playerId)
        {
            var data = new CollectionGoods(playerId, 0, 0, 0);
            var message = new Message("0310" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertGoods(string status, string playerId, int unicoin, int cosmostone, int oxygentank)
        {
            var data = new CollectionGoods(playerId, unicoin, cosmostone, oxygentank);
            var message = new Message("0320" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteGoods(string status, string playerId)
        {
            var data = new CollectionGoods(playerId, 0, 0, 0);
            var message = new Message("0330" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdateGoods(string status, string playerId, int unicoin, int cosmostone, int oxygentank)
        {
            var data = new CollectionGoods(playerId, unicoin, cosmostone, oxygentank);
            var message = new Message("0340" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Goods

        #region Send_Item

        public void SendFindItems(string status, string playerId)
        {
            var data = new CollectionItem(playerId, "", 0);
            var message = new Message("0410" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        
        public void SendInsertItem(string status, string playerId, string itemId, int count)
        {
            var data = new CollectionItem(playerId, itemId, count);
            var message = new Message("0420" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        public void SendDeleteItem(string status, string playerId, string itemId)
        {
            var data = new CollectionItem(playerId, itemId, 0);
            var message = new Message("0430" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        public void SendDeleteItems(string status, string playerId)
        {
            var data = new CollectionItem(playerId, "", 0);
            var message = new Message("0431" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        public void SendUpdateItem(string status, string playerId, string itemId, int count)
        {
            var data = new CollectionItem(playerId, itemId, count);
            var message = new Message("0440" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Item

        #region Send_Building
        
        public void SendFindBuildings(string status, string playerId)
        {
            var data = new CollectionBuilding(playerId, "", "", 0, 0, false, false, false, 0);
            var message = new Message("0510" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertBuilding(string status, string playerId, string playerStructureId, string structureId, int level, float position, bool isConstructed, bool isFlipped, bool isUpgrading, long endDate)
        {
            var data = new CollectionBuilding(playerId, playerStructureId, structureId, level, position, isConstructed, isFlipped, isUpgrading, endDate);
            var message = new Message("0520" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteBuilding(string status, string playerId, string playerStructureId)
        {
            var data = new CollectionBuilding(playerId, playerStructureId, "", 0, 0, false, false, false, 0);
            var message = new Message("0530" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteBuildings(string status, string playerId)
        {
            var data = new CollectionBuilding(playerId, "", "", 0, 0, false, false, false, 0);
            var message = new Message("0531" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdateBuilding(string status, string playerId, string playerStructureId, string structureId, int level, float position, bool isConstructed, bool isFlipped, bool isUpgrading, long endDate)
        {
            var data = new CollectionBuilding(playerId, playerStructureId, structureId, level, position, isConstructed, isFlipped, isUpgrading, endDate);
            var message = new Message("0540" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Building

        #region Send_Interior

        public void SendFindInteriors(string status, string playerId)
        {
            var data = new CollectionInterior(playerId, "", "", 0, false, false);
            var message = new Message("0610" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertInterior(string status, string playerId, string playerInteriorId, string InteriorId, int level, float position, bool isConstructed, bool isFlipped)
        {
            var data = new CollectionInterior(playerId, playerInteriorId, InteriorId, position, isConstructed, isFlipped);
            var message = new Message("0620" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteInterior(string status, string playerId, string playerInteriorId)
        {
            var data = new CollectionInterior(playerId, playerInteriorId, "", 0, false, false);
            var message = new Message("0630" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteInteriors(string status, string playerId)
        {
            var data = new CollectionInterior(playerId, "", "", 0, false, false);
            var message = new Message("0631" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdateInterior(string status, string playerId, string playerInteriorId, string InteriorId, float position, bool isConstructed, bool isFlipped)
        {
            var data = new CollectionInterior(playerId, playerInteriorId, InteriorId, position, isConstructed, isFlipped);
            var message = new Message("0640" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Interior

        #region Send_Character

        public void SendFindCharacters(string status, string playerId)
        {
            var data = new CollectionCharacter(playerId, "", 0, 0, 0, 0);
            var message = new Message("0710" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertCharacter(string status, string playerId, string characterId, int level, int health, int speed, int attackPower)
        {
            var data = new CollectionCharacter(playerId, characterId, level, health, speed, attackPower);
            var message = new Message("0720" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteCharacter(string status, string playerId, string characterId)
        {
            var data = new CollectionCharacter(playerId, characterId, 0, 0, 0, 0);
            var message = new Message("0730" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteCharacters(string status, string playerId)
        {
            var data = new CollectionCharacter(playerId, "", 0, 0, 0, 0);
            var message = new Message("0731" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdateCharacter(string status, string playerId, string characterId, int level, int health, int speed, int attackPower)
        {
            var data = new CollectionCharacter(playerId, characterId, level, health, speed, attackPower);
            var message = new Message("0740" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Character

        #region Send_Party

        public void SendFindParties(string status, string playerId)
        {
            var data = new CollectionParty(playerId, 0, 0, "");
            var message = new Message("0810" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertParty(string status, string playerId, int partyIndex, int slotIndex, string characterId)
        {
            var data = new CollectionParty(playerId, partyIndex, slotIndex, characterId);
            var message = new Message("0820" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteParty(string status, string playerId, int partyIndex, int slotIndex)
        {
            var data = new CollectionParty(playerId, partyIndex, slotIndex, "");
            var message = new Message("0830" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteParties(string status, string playerId)
        {
            var data = new CollectionParty(playerId, 0, 0, "");
            var message = new Message("0831" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdatePlayerParty(string status, string playerId, int partyIndex, int slotIndex, string characterId)
        {
            var data = new CollectionParty(playerId, partyIndex, slotIndex, characterId);
            var message = new Message("0840" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Party

        #region Send_Request

        public void SendFindRequests(string status, string playerId)
        {
            var data = new CollectionRequest(playerId, "");
            var message = new Message("0910" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertRequest(string status, string playerId, string requestId)
        {
            var data = new CollectionRequest(playerId, requestId);
            var message = new Message("0920" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteRequest(string status, string playerId, string requestId)
        {
            var data = new CollectionRequest(playerId, requestId);
            var message = new Message("0930" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteRequests(string status, string playerId)
        {
            var data = new CollectionRequest(playerId, "");
            var message = new Message("0931" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_Request

        #region Send_StoryStar

        public void SendFindStoryStars(string status, string playerId)
        {
            var data = new CollectionStroyStar(playerId, "", 0);
            var message = new Message("1010" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendInsertStoryStar(string status, string playerId, string stageNumber, int number)
        {
            var data = new CollectionStroyStar(playerId, stageNumber, number);
            var message = new Message("1020" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteStoryStar(string status, string playerId, string stageNumber)
        {
            var data = new CollectionStroyStar(playerId, stageNumber, 0);
            var message = new Message("1030" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendDeleteStoryStars(string status, string playerId)
        {
            var data = new CollectionStroyStar(playerId, "", 0);
            var message = new Message("1031" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        public void SendUpdatePlayerStoryStar(string status, string playerId, string stageNumber, int number)
        {
            var data = new CollectionStroyStar(playerId, stageNumber, number);
            var message = new Message("1040" + status, data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_StoryStar
    }
}