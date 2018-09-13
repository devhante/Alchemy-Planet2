using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.ShopScene
{
    public class Tag : MonoBehaviour
    {
        private Image image;
        private Button button;

        private void Awake()
        {
            image = GetComponent<Image>();
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClickTag);
        }

        private void OnClickTag()
        {
            if(ShopUI.Instance.selectedTag != image)
            {
                ShopUI.Instance.DeselectTag();
                ShopUI.Instance.SelectTag(image);
            }
        }
    }
}
