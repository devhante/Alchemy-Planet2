using System.Collections;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Coin : Drop
    {
        protected override void Gain()
        {
            DropManager.Instance.GainCoin(10);
        }
    }
}
