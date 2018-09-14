using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.Data
{
    public class Message
    {
        public string status { get; set; }
        public string data { get; set; }

        /*
        public Message(string status, string data)
        {
            this.status = status;
            this.data = data;
        }
        */
        public Message(string status, object data)
        {
            this.status = status;
            this.data = JsonConvert.SerializeObject(data);
        }
    }
}
