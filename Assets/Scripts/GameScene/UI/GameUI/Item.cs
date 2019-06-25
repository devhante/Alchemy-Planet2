using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.GameScene
{
    public enum ItemName { PopinPotionBlack }

    public class Item : Bubble, IPointerUpHandler
    {
        public ItemName itemName;
        private bool isItemPointerUp = false;

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (isItemPointerUp) return;
            else isItemPointerUp = true;

            ItemManager.Instance.UsedItemNumber[itemName]++;
            DestroyItem();
        }

        private void DestroyItem()
        {
            ExpandAndDestroy();
            ItemManager.Instance.Objects.Remove(gameObject);
        }
    }
}
