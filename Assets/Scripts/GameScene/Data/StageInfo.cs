using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    [Serializable]
    public class StageInfo
    {
        public string name;
        public GameObject backgroundPrefab;
        public GameObject tilePrefab;
        public GameObject[] monsters;
    }
}