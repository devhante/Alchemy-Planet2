using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class DropMaterial : Drop
    {
        public MaterialName materialName;

        protected override void Gain()
        {
            Debug.Log("Gain " + materialName.ToString());
        }
    }
}
