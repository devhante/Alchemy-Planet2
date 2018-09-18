using System.Collections;
using System.Collections.Generic;
using AlchemyPlanet.Data;
using UnityEngine;

namespace AlchemyPlanet.AlchemyScene {

    public class Produce : MonoBehaviour
    {
        public Data.ItemData materialInfo;
        public ItemKind kind;

        public Produce(Data.ItemData materialInfo, ItemKind kind)
        {
            this.materialInfo = materialInfo;
            this.kind = kind;
        }
    }

}