using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.TownScene
{
    public class GetStructureUpgradeInfo
    {
        public string type = "GetStructureUpgradeInfo";
        public int userId;
        public int structureId;

        public GetStructureUpgradeInfo(int uid, int sid)
        {
            userId = uid;
            structureId = sid;
        }
    }

    public class SetStructureUpgradeInfo
    {
        public string type = "SetStructureUpgradeInfo";
        public int userId;
        public int structureId;
        public DateTime startDate;
        public int requireTime;

        public SetStructureUpgradeInfo(int uid, int sid, DateTime sdate, int rtime)
        {
            userId = uid;
            structureId = sid;
            startDate = sdate;
            requireTime = rtime;
        }
    }

    public class StructureUpgradeInfo
    {
        public string type;
        public int userId;
        public int structureId;
        public DateTime startDate;
        public int requireTime;
    }
}