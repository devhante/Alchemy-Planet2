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
            Instance = this;
        }

        private void Start()
        {
            ws = new WebSocket("ws://127.0.0.1:3000");

            ws.OnMessage += (sender, e) =>
            {
                Debug.Log(e.Data);
            };

            ws.Connect();
        }

        private void OnDestroy()
        {
            Instance = null;
            ws.Close();
        }

        private void GetStructureUpgradeInfo(int uid, int sid)
        {
            var obj = new GetStructureUpgradeInfo(uid, sid);
            var str = JsonConvert.SerializeObject(obj);
            ws.Send(str);
        }

        private void SetStructureUpgradeInfo(int uid, int sid, DateTime sDate, int rTime)
        {
            var obj = new SetStructureUpgradeInfo(uid, sid, sDate, rTime);
            var str = JsonConvert.SerializeObject(obj);
            ws.Send(str);
        }
    }
}