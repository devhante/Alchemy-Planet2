using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.GameScene
{
    public class Material : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
    {
        public string materialName;
        Image bubble;
        bool isComboSelected;

        private void Awake()
        {
            bubble = transform.GetChild(0).GetComponent<Image>();
            isComboSelected = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            bubble.sprite = PrefabManager.Instance.SelectedBubble;

            if (RecipeManager.Instance.GetQueuePeekName() == materialName)
            {
                MaterialManager.Instance.IsClicked = true;
                MaterialManager.Instance.MaterialCombo.Add(this);
                RecipeManager.Instance.UpdateRecipeNameList();
                isComboSelected = true;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!MaterialManager.Instance.IsClicked)
            {
                MaterialManager.Instance.DecreaseMaterialNumber(materialName);
                MaterialManager.Instance.StartCoroutine("ReInstantiatematerial", transform.position);
                Destroy(gameObject);
                return;
            }

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
            if (MaterialManager.Instance.IsClicked && !isComboSelected)
                if(RecipeManager.Instance.RecipeNameList[MaterialManager.Instance.MaterialCombo.Count] == materialName)
                {
                    bubble.sprite = PrefabManager.Instance.SelectedBubble;
                    MaterialManager.Instance.MaterialCombo.Add(this);
                    isComboSelected = true;
                }
        }
    }
}