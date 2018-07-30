using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class AlchemyUI : Common.UI<AlchemyUI>
    {
        [SerializeField] private Button InventoryButton;

        protected override void Awake()
        {
            base.Awake();
            TownScene.UIManager.Instance.Clear();
            InventoryButton.onClick.AddListener(() => {
                TownScene.UIManager.Instance.OpenMenu<TownScene.InventoryCell>();
            });
        }
    }
}