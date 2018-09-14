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
                var test = JsonConvert.DeserializeObject<StructureUpgradeInfo>(e.Data);
                Debug.Log(test.userId);
            };

            ws.Connect();
            GetStructureUpgradeInfo(0, 1);
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