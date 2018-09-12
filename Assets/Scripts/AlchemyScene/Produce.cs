using System.Collections;
using System.Collections.Generic;
using AlchemyPlanet.Data;
using UnityEngine;

namespace AlchemyPlanet.AlchemyScene {

    public class Produce : MonoBehaviour
    {
        public Data.Material materialInfo;
        public ItemKind kind;

        public Produce(Data.Material materialInfo, ItemKind kind)
        {
            this.materialInfo = materialInfo;
            this.kind = kind;
        }
    }

}