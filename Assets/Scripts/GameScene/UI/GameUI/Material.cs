using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace AlchemyPlanet.GameScene
{
    public class Material : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
    {
        public string materialName;
        Image bubble;
        Button button;
        bool isChainSelected;

        private void Awake()
        {
            bubble = transform.GetChild(0).GetComponent<Image>();
            button = GetComponent<Button>();
            isChainSelected = false;
        }

        private void Update()
        {
            if (Time.timeScale == 1)
                button.enabled = true;
            else
                button.enabled = false;
                
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Time.timeScale == 0) return;
            ChangeBubbleToSelectedBubble();

            if (RecipeManager.Instance.GetQueuePeekName() == materialName)
            {
                MaterialManager.Instance.IsClickedRightMaterial = true;
                RecipeManager.Instance.UpdateRecipeNameList();
                isChainSelected = true;
                MaterialManager.Instance.Lines.Add(Instantiate(PrefabManager.Instance.line, transform.parent).GetComponent<Line>());
                MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].start = transform.position;
                MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].transform.SetAsFirstSibling();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Time.timeScale == 0) return;

            MaterialManager.Instance.ReinstantiateMaterial(this);

            if (RecipeManager.Instance.GetQueuePeekName() == materialName)
                RecipeManager.Instance.DestroyQueuePeek();
            else GameUI.Instance.UpdateGage(Gages.PURIFY, -5);

            if (!MaterialManager.Instance.IsClickedRightMaterial) return;
            MaterialManager.Instance.IsClickedRightMaterial = false;

            foreach (var item in MaterialManager.Instance.MaterialChain)
            {
                item.ChangeBubbleToUnselectedBubble();
                MaterialManager.Instance.ReinstantiateMaterial(item);
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
                    ChangeBubbleToSelectedBubble();
                    MaterialManager.Instance.MaterialChain.Add(this);
                    isChainSelected = true;

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

        public void ChangeBubbleToSelectedBubble()
        {
            bubble.sprite = PrefabManager.Instance.selectedBubble;
        }

        public void ChangeBubbleToUnselectedBubble()
        {
            bubble.sprite = PrefabManager.Instance.unselectedBubble;
        }
    }
}