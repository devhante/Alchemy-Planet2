using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.Data
{
    public enum ItemKind
    {
        Diffuser, Quest, PurifyPosion, Material, Event
    }

    public class ItemData
    {
        public string item_name;
        public ItemKind item_kind;
        public string discription;
        public Sprite image;

        public ItemData(string item_name, ItemKind item_kind, string discription)
        {
            this.item_name = item_name;
            this.item_kind = item_kind;
            this.discription = discription;
        }
    }

    public class FormulaData
    {
        public Dictionary<string, int> formula;
        public string result;
        public int resultcount;

        public FormulaData(Dictionary<string, int> formula, string result, int resultcount)
        {
            this.formula = formula;
            this.result = result;
            this.resultcount = resultcount;
        }
    }

    public class Request
    {
        public Dictionary<string, int> requires;
        //필요물품

        public string request_name;
        //리퀘스트 이름

        public int unicoin;
        public int cosmoston;
        public int exp;
        //보상

        public Request(Dictionary<string, int> requires, string request_name, int unicoin, int cosmoston, int exp)
        {
            this.requires = requires;
            this.request_name = request_name;
            this.unicoin = unicoin;
            this.cosmoston = cosmoston;
            this.exp = exp;
        }
    }
}