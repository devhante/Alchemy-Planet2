using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class ItemCell : MonoBehaviour
    {
        public string itemName { get; private set; }
        public int itemCount { get; private set; }

        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private Text countText;
        
        public void SetItemCell(string name,Sprite itemSprite, int count = 0)
        {
            itemName = name;
            itemCount = count;
            if (countText != null)
            {
                if (count > 2)
                    countText.gameObject.SetActive(false);
                else
                    countText.text = count.ToString();
            }
            itemImage.sprite = itemSprite;
        }
    }
}