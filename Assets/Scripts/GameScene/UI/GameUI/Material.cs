using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace AlchemyPlanet.GameScene
{
    public enum MaterialName { Red, Yellow, Green, Blue, Purple }

    public class Material : Bubble, IPointerEnterHandler, IPointerUpHandler
    {
        public MaterialName materialName;

        private Image image;
        private Image mask;
        private bool isChainSelected;
        private bool isHighlighted;
        private bool isMaterialPointerDown;
        private bool isMaterialPointerUp;

        protected override void Awake()
        {
            base.Awake();
            mask = transform.GetChild(1).GetComponent<Image>();
            isChainSelected = false;
            isHighlighted = false;
            isMaterialPointerDown = false;
            isMaterialPointerUp = false;
        }

        protected override void Start()
        {
            base.Start();
            image = GetComponent<Image>();
        }

        private void Update()
        {
            if (isHighlighted == false && materialName == MaterialManager.Instance.HighlightedMaterialName && bubble.sprite == PrefabManager.Instance.unselectedBubble && MaterialManager.Instance.MaterialChain.Count + 1 < MaterialManager.Instance.MaxChainNumber)
            {
                isHighlighted = true;
                ChangeBubbleToHighlightedBubble();
            }
            else if (isHighlighted == true && materialName != MaterialManager.Instance.HighlightedMaterialName && bubble.sprite == PrefabManager.Instance.highlightedBubble)
            {
                isHighlighted = false;
                ChangeBubbleToUnselectedBubble();
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (isMaterialPointerDown) return;
            else isMaterialPointerDown = true;

            base.OnPointerDown(eventData);

            if (RecipeManager.Instance.GetQueuePeekName() == materialName)
            {
                MaterialManager.Instance.IsClickedRightMaterial = true;
                RecipeManager.Instance.HighlightRecipe();
                isChainSelected = true;
                transform.SetSiblingIndex(0);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isMaterialPointerUp) return;
            else isMaterialPointerUp = true;

            if (Time.timeScale == 0) return;
            MaterialManager.Instance.RespawnMaterial(this);

            if (RecipeManager.Instance.GetQueuePeekName() == materialName)
            {
                MaterialManager.Instance.DestroyedMaterialNumber++;
                Player.Instance.GetMaterialMessage(materialName);
                Popin.Instance.SkillGage += 5;
                GameManager.Instance.GainScore(ScoreType.TouchRightRecipe);
                GameManager.Instance.Combo++;
                RecipeManager.Instance.DestroyQueuePeek();
                RecipeManager.Instance.HighlightedRecipeCount = 0;

                if (Random.Range(1, 100) <= GameSettings.Instance.itemPercent)
                    ItemManager.Instance.CreateItem(ItemManager.Instance.GetItemName());
            }
            else
            {
                GameManager.Instance.Combo = 0;
            }

            if (!MaterialManager.Instance.IsClickedRightMaterial) return;
            MaterialManager.Instance.IsClickedRightMaterial = false;

            foreach (var item in MaterialManager.Instance.MaterialChain)
            {
                MaterialManager.Instance.DestroyedMaterialNumber++;
                Player.Instance.GetMaterialMessage(item.materialName);
                Popin.Instance.SkillGage += 5;
                item.ChangeBubbleToUnselectedBubble();
                MaterialManager.Instance.RespawnMaterial(item);
                RecipeManager.Instance.DestroyQueuePeek();
            }

            if (MaterialManager.Instance.MaterialChain.Count > 0)
            {
                MaterialManager.Instance.ChainedNumber++;
                Player.Instance.Attack(MaterialManager.Instance.MaterialChain.Count, GameUI.Instance.GetGage(Gages.PURIFY));
                GameUI.Instance.UpdateGage(Gages.PURIFY, -100);
            }
            else
            {
                GameUI.Instance.UpdateGage(Gages.PURIFY, 3);
            }

            MaterialManager.Instance.MaterialChain.Clear();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (MaterialManager.Instance.IsClickedRightMaterial && !isChainSelected && MaterialManager.Instance.MaterialChain.Count < MaterialManager.Instance.MaxChainNumber - 1)
            {
                if (RecipeManager.Instance.RecipeNameList[MaterialManager.Instance.MaterialChain.Count + 1] == materialName)
                {
                    StopCoroutine(Float());
                    ChangeBubbleToSelectedBubble();
                    MaterialManager.Instance.MaterialChain.Add(this);
                    isChainSelected = true;
                    transform.SetSiblingIndex(0);
                    StartCoroutine(MoveCoroutine());
                    RecipeManager.Instance.HighlightRecipe();
                }
            }
        }
    }
}