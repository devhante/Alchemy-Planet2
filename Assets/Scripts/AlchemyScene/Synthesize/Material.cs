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

        protected override void Awake()
        {
            base.Awake();
            isMaterialPointerDown = false;
            isMaterialPointerUp = false;
        }

        protected override void Start()
        {
            base.Start();
            image = GetComponent<Image>();
            miniGame1 = GetComponentInParent<MiniGame1>();
        }

        public void SetMaterial(string materialName, bool isMaterialOfItem)
        {
            this.materialName = materialName;
            this.isMaterialOfItem = isMaterialOfItem;

            image.sprite = Data.DataManager.Instance.itemInfo[AlchemyManager.Instance.GetEnglishName(materialName)].image;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (isMaterialPointerDown) return;
            else isMaterialPointerDown = true;

            base.OnPointerDown(eventData);
            if (!isMaterialOfItem)
                ExpandAndDestroy();

            else
            {
                miniGame1.SetSelectedMaterial(this);
                StopCoroutine("Float");
                StartCoroutine("Shrink");
                ChangeBubbleToSelectedBubble();
                transform.SetSiblingIndex(0);
                StartCoroutine("MoveCoroutine");
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isMaterialPointerUp) return;
            else isMaterialPointerUp = true;

            if (Time.timeScale == 0) return;

            ExpandAndDestroy();
            miniGame1.CheckFail();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isMaterialOfItem)
            {
                miniGame1.FailMiniGame1();
                ExpandAndDestroy();
            }

            else
            {
                miniGame1.SetSelectedMaterial(this);
                StopCoroutine("Float");
                StartCoroutine("Shrink");
                ChangeBubbleToSelectedBubble();
                transform.SetSiblingIndex(0);
                StartCoroutine("MoveCoroutine");
            }
        }
    }
}