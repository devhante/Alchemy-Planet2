using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class ItemCell : MonoBehaviour
    {
        public string itemName { get; private set; }

        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private Text countText;
        

        public void SetItemCell(string itemName,Sprite itemSprite, int count = 0)
        {
            this.itemName = itemName;
            if (countText != null)
            {
                if (count == 0)
                    countText.gameObject.SetActive(false);
                else
                    countText.text = count.ToString();
            }
            itemImage.sprite = itemSprite;
        }
    }
}