using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.GameScene
{
    public class Material : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
    {
        public string materialName;
        Image bubble;
        Button button;
        bool isComboSelected;

        private void Awake()
        {
            bubble = transform.GetChild(0).GetComponent<Image>();
            button = GetComponent<Button>();
            isComboSelected = false;
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
            bubble.sprite = PrefabManager.Instance.SelectedBubble;

            if (RecipeManager.Instance.GetQueuePeekName() == materialName)
            {
                MaterialManager.Instance.IsClicked = true;
                RecipeManager.Instance.UpdateRecipeNameList();
                isComboSelected = true;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Time.timeScale == 0) return;

            MaterialManager.Instance.DecreaseMaterialNumber(materialName);
            MaterialManager.Instance.StartCoroutine("ReInstantiatematerial", transform.position);
            Destroy(gameObject);

            if (RecipeManager.Instance.GetQueuePeekName() == materialName)
                RecipeManager.Instance.DestroyQueuePeek();
            else GameUI.Instance.UpdateGage(Gages.PURIFY, -5);

            if (!MaterialManager.Instance.IsClicked) return;
            MaterialManager.Instance.IsClicked = false;

            foreach (var item in MaterialManager.Instance.MaterialCombo)
            {
                item.bubble.sprite = PrefabManager.Instance.UnselectedBubble;
                MaterialManager.Instance.DecreaseMaterialNumber(item.materialName);
                MaterialManager.Instance.StartCoroutine("ReInstantiatematerial", item.transform.position);
                RecipeManager.Instance.DestroyQueuePeek();
                Destroy(item.gameObject);
            }

            MaterialManager.Instance.MaterialCombo.Clear();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (MaterialManager.Instance.IsClicked && !isComboSelected && MaterialManager.Instance.MaterialCombo.Count < 4)
                if(RecipeManager.Instance.RecipeNameList[MaterialManager.Instance.MaterialCombo.Count + 1] == materialName)
                {
                    bubble.sprite = PrefabManager.Instance.SelectedBubble;
                    MaterialManager.Instance.MaterialCombo.Add(this);
                    isComboSelected = true;
                }
        }
    }
}