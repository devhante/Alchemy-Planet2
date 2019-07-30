using System.Collections;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Coin : Drop
    {
        protected override void Gain()
        {
            GameManager.Instance.GainCoin(10);
        }
    }
}
