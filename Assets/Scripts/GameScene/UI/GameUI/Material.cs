using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace AlchemyPlanet.GameScene
{
    public enum MaterialName { Red, Yellow, Green, Blue, Purple, Chicken }

    public class Material : Bubble, IPointerUpHandler, IPointerEnterHandler
    {
        public MaterialName materialName;
        bool isChainSelected;

        protected override void Awake()
        {
            base.Awake();
            isChainSelected = false;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (RecipeManager.Instance.GetQueuePeekName() == materialName)
            {
                MaterialManager.Instance.IsClickedRightMaterial = true;
                RecipeManager.Instance.UpdateRecipeNameList();
                RecipeManager.Instance.HighlightRecipe();
                isChainSelected = true;
                MaterialManager.Instance.Lines.Add(Instantiate(PrefabManager.Instance.line, transform.parent).GetComponent<Line>());
                MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].start = transform.position;
                MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].transform.SetAsFirstSibling();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Time.timeScale == 0) return;
            else MaterialManager.Instance.RespawnMaterial(this);

            if (RecipeManager.Instance.GetQueuePeekName() == materialName)
            {
                Player.Instance.GetMaterialMessage(materialName);
                GameManager.Instance.GainScore(ScoreType.TouchRightRecipe);
                GameManager.Instance.Combo++;
                RecipeManager.Instance.DestroyQueuePeek();
                RecipeManager.Instance.HighlightedRecipeCount = 0;

                if (Random.Range(1, 100) <= 20)
                    ItemManager.Instance.CreateItem();
            }
            else
            {
                GameUI.Instance.UpdateGage(Gages.PURIFY, -5);
                GameManager.Instance.Combo = 0;
            }

            if (!MaterialManager.Instance.IsClickedRightMaterial) return;
            MaterialManager.Instance.IsClickedRightMaterial = false;

            foreach (var item in MaterialManager.Instance.MaterialChain)
            {
                Player.Instance.GetMaterialMessage(item.materialName);
                item.ChangeBubbleToUnselectedBubble();
                MaterialManager.Instance.RespawnMaterial(item);
                RecipeManager.Instance.DestroyQueuePeek();
            }

            if (MaterialManager.Instance.MaterialChain.Count > 0)
                Player.Instance.Attack(MaterialManager.Instance.MaterialChain.Count);

            foreach(var item in MaterialManager.Instance.Lines)
                Destroy(item.gameObject);

            MaterialManager.Instance.MaterialChain.Clear();
            MaterialManager.Instance.Lines.Clear();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (MaterialManager.Instance.IsClickedRightMaterial && !isChainSelected && MaterialManager.Instance.MaterialChain.Count < MaterialManager.Instance.MaxChainNumber - 1)
                if(RecipeManager.Instance.RecipeNameList[MaterialManager.Instance.MaterialChain.Count + 1] == materialName)
                {
                    StopCoroutine("Float");
                    StartCoroutine("Shrink");
                    ChangeBubbleToSelectedBubble();
                    MaterialManager.Instance.MaterialChain.Add(this);
                    isChainSelected = true;

                    RecipeManager.Instance.HighlightRecipe();
                    MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].end = transform.position;
                    MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].StopCoroutine("DrawCoroutine");
                    MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].Draw();

                    if (MaterialManager.Instance.MaterialChain.Count < MaterialManager.Instance.MaxChainNumber - 1)
                    { 
                        MaterialManager.Instance.Lines.Add(Instantiate(PrefabManager.Instance.line, transform.parent).GetComponent<Line>());
                        MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].transform.SetAsFirstSibling();
                        MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].start = transform.position;
                    }
                }
        }
    }
}