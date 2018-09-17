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
                    var data = JsonConvert.DeserializeObject<PlayerName>(data_string);

                    Debug.Log(data.playerName);
                }

                if (message.status == "PlayerLevel")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<PlayerLevel>(data_string);

                    Debug.Log(data.level);
                }

                if (message.status == "PlayerGoods")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<PlayerGoods>(data_string);

                    Debug.Log(data.uniCoin + " | " + data.cosmoStone + " | " + data.oxygenTank);
                }

                if (message.status == "PlayerItem")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<PlayerItem[]>(data_string);

                    Debug.Log(data[0].itemId + " | " + data[1].itemId + " | " + data[2].itemId);
                }

                if (message.status == "PlayerStructure")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<PlayerStructure[]>(data_string);

                    Debug.Log(data[0].structureUniqueId + " | " + data[1].structureUniqueId + " | " + data[2].structureUniqueId);
                }

                if (message.status == "PlayerTownStructure")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<PlayerTownStructure[]>(data_string);

                    Debug.Log(data[0].structureUniqueId + " | " + data[1].structureUniqueId + " | " + data[2].structureUniqueId);
                }

                if (message.status == "PlayerUpgradingStructure")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<PlayerUpgradingStructure[]>(data_string);

                    Debug.Log(data[0].structureUniqueId + " | " + data[1].structureUniqueId + " | " + data[2].structureUniqueId);
                }

                if (message.status == "PlayerCharacter")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<PlayerCharacter[]>(data_string);

                    Debug.Log(data[0].level + " | " + data[1].level + " | " + data[2].level);
                }

                if (message.status == "PlayerParty")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<PlayerParty[]>(data_string);

                    Debug.Log(data[0].characterId + " | " + data[1].characterId + " | " + data[2].characterId);
                }

                if (message.status == "PlayerRequest")
                {
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<PlayerRequest[]>(data_string);

                    Debug.Log(data[0].requestId + " | " + data[1].requestId + " | " + data[2].requestId);
                }
            };

            ws.Connect();


            if(ws.IsConnected)
            {
                //SendDropTable("PlayerName");
                SendFindPlayerName("0");
                SendFindPlayerLevel("0");
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

        #region Send_PlayerName

        private void SendFindPlayerName(string playerId)
        {
            var data = new PlayerName(playerId, "");
            var message = new Message("findPlayerName", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertPlayerName(string playerId, string playerName)
        {
            var data = new PlayerName(playerId, playerName);
            var message = new Message("insertPlayerName", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerName(string playerId)
        {
            var data = new PlayerName(playerId, "");
            var message = new Message("deletePlayerName", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdatePlayerName(string playerId, string playerName)
        {
            var data = new PlayerName(playerId, playerName);
            var message = new Message("updatePlayerName", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_PlayerName

        #region Send_PlayerLevel

        private void SendFindPlayerLevel(string playerId)
        {
            var data = new PlayerLevel(playerId, 0, 0);
            var message = new Message("findPlayerLevel", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertPlayerLevel(string playerId, int level, int exp)
        {
            var data = new PlayerLevel(playerId, level, exp);
            var message = new Message("insertPlayerLevel", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerLevel(string playerId)
        {
            var data = new PlayerLevel(playerId, 0, 0);
            var message = new Message("deletePlayerLevel", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdatePlayerLevel(string playerId, int level, int exp)
        {
            var data = new PlayerLevel(playerId, level, exp);
            var message = new Message("updatePlayerLevel", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_PlayerLevel

        #region Send_PlayerGoods

        private void SendFindPlayerGoods(string playerId)
        {
            var data = new PlayerGoods(playerId, 0, 0, 0);
            var message = new Message("findPlayerGoods", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertPlayerGoods(string playerId, int unicoin, int cosmostone, int oxygentank)
        {
            var data = new PlayerGoods(playerId, unicoin, cosmostone, oxygentank);
            var message = new Message("insertPlayerGoods", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerGoods(string playerId)
        {
            var data = new PlayerGoods(playerId, 0, 0, 0);
            var message = new Message("deletePlayerGoods", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdatePlayerGoods(string playerId, int unicoin, int cosmostone, int oxygentank)
        {
            var data = new PlayerGoods(playerId, unicoin, cosmostone, oxygentank);
            var message = new Message("updatePlayerGoods", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_PlayerGoods

        #region Send_PlayerItem

        private void SendFindPlayerItems(string playerId)
        {
            var data = new PlayerItem(playerId, "", 0);
            var message = new Message("findPlayerItems", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        
        private void SendInsertPlayerItem(string playerId, string itemId, int count)
        {
            var data = new PlayerItem(playerId, itemId, count);
            var message = new Message("insertPlayerItem", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        private void SendDeletePlayerItem(string playerId, string itemId)
        {
            var data = new PlayerItem(playerId, itemId, 0);
            var message = new Message("deletePlayerItem", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        private void SendDeletePlayerItems(string playerId)
        {
            var data = new PlayerItem(playerId, "", 0);
            var message = new Message("deletePlayerItems", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        private void SendUpdatePlayerItem(string playerId, string itemId, int count)
        {
            var data = new PlayerItem(playerId, itemId, count);
            var message = new Message("updatePlayerItem", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_PlayerItem

        #region Send_PlayerStructure
        
        private void SendFindPlayerStructures(string playerId)
        {
            var data = new PlayerStructure(playerId, "", "", 0);
            var message = new Message("findPlayerStructures", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertPlayerStructure(string playerId, string structureUniqueId, string structureId, int level)
        {
            var data = new PlayerStructure(playerId, structureUniqueId, structureId, level);
            var message = new Message("insertPlayerStructure", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerStructure(string playerId, string structureUniqueId)
        {
            var data = new PlayerStructure(playerId, structureUniqueId, "", 0);
            var message = new Message("deletePlayerStructure", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerStructures(string playerId)
        {
            var data = new PlayerStructure(playerId, "", "", 0);
            var message = new Message("deletePlayerStructures", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdatePlayerStructure(string playerId, string structureUniqueId, string structureId, int level)
        {
            var data = new PlayerStructure(playerId, structureUniqueId, structureId, level);
            var message = new Message("updatePlayerStructure", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_PlayerStructure

        #region Send_PlayerTownStructure

        private void SendFindPlayerTownStructures(string playerId)
        {
            var data = new PlayerTownStructure(playerId, "", 0);
            var message = new Message("findPlayerTownStructures", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertPlayerTownStructure(string playerId, string structureUniqueId, int position)
        {
            var data = new PlayerTownStructure(playerId, structureUniqueId, position);
            var message = new Message("insertPlayerTownStructure", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerTownStructure(string playerId, string structureUniqueId)
        {
            var data = new PlayerTownStructure(playerId, structureUniqueId, 0);
            var message = new Message("deletePlayerTownStructure", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerTownStructures(string playerId)
        {
            var data = new PlayerTownStructure(playerId, "", 0);
            var message = new Message("deletePlayerTownStructures", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdatePlayerTownStructure(string playerId, string structureUniqueId, int position)
        {
            var data = new PlayerTownStructure(playerId, structureUniqueId, position);
            var message = new Message("updatePlayerTownStructure", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_PlayerTownStructure

        #region Send_PlayerUpgradingStructure

        private void SendFindPlayerUpgradingStructures(string playerId)
        {
            var data = new PlayerUpgradingStructure(playerId, "", new DateTime(), 0);
            var message = new Message("findPlayerUpgradingStructures", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertPlayerUpgradingStructure(string playerId, string structureUniqueId, int requireTime)
        {
            var data = new PlayerUpgradingStructure(playerId, structureUniqueId, new DateTime(), requireTime);
            var message = new Message("insertPlayerUpgradingStructure", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerUpgradingStructure(string playerId, string structureUniqueId)
        {
            var data = new PlayerUpgradingStructure(playerId, structureUniqueId, new DateTime(), 0);
            var message = new Message("deletePlayerUpgradingStructure", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerUpgradingStructures(string playerId)
        {
            var data = new PlayerUpgradingStructure(playerId, "", new DateTime(), 0);
            var message = new Message("deletePlayerUpgradingStructures", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdatePlayerUpgradingStructure(string playerId, string structureUniqueId , int requireTime)
        {
            var data = new PlayerUpgradingStructure(playerId, structureUniqueId, new DateTime(), requireTime);
            var message = new Message("updatePlayerUpgradingStructure", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_PlayerUpgradingStructure

        #region Send_PlayerCharacter

        private void SendFindPlayerCharacters(string playerId)
        {
            var data = new PlayerCharacter(playerId, "", 0, 0, 0, 0);
            var message = new Message("findPlayerCharacters", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertPlayerCharacter(string playerId, string characterId, int level, int health, int speed, int attackPower)
        {
            var data = new PlayerCharacter(playerId, characterId, level, health, speed, attackPower);
            var message = new Message("insertPlayerCharacter", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerCharacter(string playerId, string characterId)
        {
            var data = new PlayerCharacter(playerId, characterId, 0, 0, 0, 0);
            var message = new Message("deletePlayerCharacter", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerCharacters(string playerId)
        {
            var data = new PlayerCharacter(playerId, "", 0, 0, 0, 0);
            var message = new Message("deletePlayerCharacters", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdatePlayerCharacter(string playerId, string characterId, int level, int health, int speed, int attackPower)
        {
            var data = new PlayerCharacter(playerId, characterId, level, health, speed, attackPower);
            var message = new Message("updatePlayerCharacter", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_PlayerCharacter

        #region Send_PlayerParty

        private void SendFindPlayerParties(string playerId)
        {
            var data = new PlayerParty(playerId, 0, 0, "");
            var message = new Message("findPlayerParties", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertPlayerParty(string playerId, int playerIndex, int slotIndox, string characterId)
        {
            var data = new PlayerParty(playerId, playerIndex, slotIndox, characterId);
            var message = new Message("insertPlayerParty", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerParty(string playerId, int playerIndex, int slotIndox)
        {
            var data = new PlayerParty(playerId, playerIndex, slotIndox, "");
            var message = new Message("deletePlayerParty", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerParties(string playerId)
        {
            var data = new PlayerParty(playerId, 0, 0, "");
            var message = new Message("deletePlayerParties", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendUpdatePlayerParty(string playerId, int playerIndex, int slotIndox, string characterId)
        {
            var data = new PlayerParty(playerId, playerIndex, slotIndox, characterId);
            var message = new Message("updatePlayerParty", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_PlayerParty

        #region Send_PlayerRequest

        private void SendFindPlayerRequests(string playerId)
        {
            var data = new PlayerRequest(playerId, "");
            var message = new Message("findPlayerRequests", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendInsertPlayerRequest(string playerId, string requestId)
        {
            var data = new PlayerRequest(playerId, requestId);
            var message = new Message("insertPlayerRequest", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerRequest(string playerId, string requestId)
        {
            var data = new PlayerRequest(playerId, requestId);
            var message = new Message("deletePlayerRequest", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
        private void SendDeletePlayerRequests(string playerId)
        {
            var data = new PlayerRequest(playerId, "");
            var message = new Message("deletePlayerRequests", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        #endregion Send_PlayerRequest
    }
}