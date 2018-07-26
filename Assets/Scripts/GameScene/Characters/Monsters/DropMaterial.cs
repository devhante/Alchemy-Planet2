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
            DropManager.Instance.GainDropMaterial(materialName);
        }
    }
}
