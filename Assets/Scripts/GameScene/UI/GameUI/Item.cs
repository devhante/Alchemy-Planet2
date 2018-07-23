using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.GameScene
{
    public class Item : Bubble, IPointerUpHandler
    {
        public string itemName;

        public void OnPointerUp(PointerEventData eventData)
        {
            ItemManager.Instance.Objects.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
