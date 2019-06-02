using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.GameScene
{
    public enum ItemName { IncreasePurify, NoReducedOxygen, RainbowColorBall, SlowReducedOxygen, PopinPotionBlack }

    public class Item : Bubble, IPointerUpHandler
    {
        public ItemName itemName;
        private bool isItemPointerUp = false;

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (isItemPointerUp) return;
            else isItemPointerUp = true;

            switch (itemName)
            {
                case ItemName.IncreasePurify: ItemManager.Instance.IncreasePurify(); break;
                case ItemName.NoReducedOxygen: ItemManager.Instance.NoReducedOxygen(); break;
                case ItemName.RainbowColorBall: ItemManager.Instance.RainbowColorBall(); break;
                case ItemName.SlowReducedOxygen: ItemManager.Instance.SlowReducedOxygen(); break;
            }

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
