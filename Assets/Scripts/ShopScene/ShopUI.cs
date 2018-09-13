using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.ShopScene
{
    public class ShopUI : MonoBehaviour
    {
        public static ShopUI Instance { get; private set; }

        public Sprite tagSprite;
        public Sprite selectedTagSprite;
        public GameObject tags;

        [HideInInspector]
        public Image selectedTag;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;

            SelectTag(tags.transform.GetChild(0).GetComponent<Image>());
        }

        public void DeselectTag()
        {
            selectedTag.sprite = tagSprite;
            selectedTag = null;
        }

        public void SelectTag(Image tag)
        {
            selectedTag = tag;
            selectedTag.sprite = selectedTagSprite;
        }
    }
}