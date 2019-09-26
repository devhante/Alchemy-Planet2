using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace AlchemyPlanet.AlchemyScene
{
    public class Material : Bubble, IPointerEnterHandler, IPointerUpHandler
    {
        public string materialName;

        private Image image;
        private bool isMaterialOfItem;
        private bool isMaterialPointerDown;
        private bool isMaterialPointerUp;
        private MiniGame1 miniGame1;
        private Vector3 startPosition;

        protected override void Awake()
        {
            base.Awake();
            isMaterialPointerDown = false;
            isMaterialPointerUp = false;
        }

        protected override void Start()
        {
            base.Start();
            miniGame1 = GetComponentInParent<MiniGame1>();
            startPosition = gameObject.transform.position;
        }

        public void SetMaterial(string materialName, bool isMaterialOfItem)
        {
            this.materialName = materialName;
            this.isMaterialOfItem = isMaterialOfItem;

            image = GetComponent<Image>();
            image.sprite = Data.DataManager.Instance.itemInfo[AlchemyManager.Instance.GetEnglishName(materialName)].image;
        }

        public void RemoveMaterial()
        {
            isMaterialPointerDown = false;
            isBubblePointerDown = false;
            StopCoroutine(MoveCoroutine());
            StartCoroutine(Float());
            ChangeBubbleToUnselectedBubble();
            gameObject.transform.position = startPosition;
        }
        
        public void OnPointerUp(PointerEventData eventData){
            miniGame1.FailMiniGame1();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isMaterialOfItem)
            {
                if (miniGame1.IsFirstBubble())
                    ExpandAndDestroy();
                else
                    miniGame1.FailMiniGame1();
                return;
            }
            else if (isMaterialPointerDown) return;
            else isMaterialPointerDown = true;

            base.OnPointerDown(eventData);
            miniGame1.SetSelectedMaterial(this);
            transform.SetSiblingIndex(0);
            miniGame1.CheckSuccess();
        }
    }
}