using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.TownScene
{
    public class StructureUpgradeInfo
    {
        public string userId { get; set; }
        public int structureId { get; set; }
        public DateTime startDate { get; set; }
        public int requireTime { get; set; }

        public StructureUpgradeInfo(string userId, int structureId, DateTime startDate, int requireTime)
        {
            this.userId = userId;
            this.structureId = structureId;
            this.startDate = startDate;
            this.requireTime = requireTime;
        }
    }
}