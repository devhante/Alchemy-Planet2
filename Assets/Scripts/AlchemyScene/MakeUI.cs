using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class MakeUI : Common.UI<MakeUI> {
        [SerializeField] private Button[] MateraiButtons = new Button[5];

        [SerializeField] private Button ResultButton;
        [SerializeField] private Button MakeButton;
        [SerializeField] private Button BackButton;
        [SerializeField] private Button BookmarkButton;

        protected override void Awake()
        {
            base.Awake();

            foreach (var item in MateraiButtons)
                item.onClick.AddListener(OnClickMateraiButton);

            MakeButton.onClick.AddListener(OnClickMakeButton);
            ResultButton.onClick.AddListener(OnClickResultButton);
            BackButton.onClick.AddListener(OnClickBackButton);
            BookmarkButton.onClick.AddListener(OnClickBookmarkButton);
        }

        private void OnClickMateraiButton()
        {
            TownScene.UIManager.Instance.OpenMenu<TownScene.InventoryCell>();
        }

        private void OnClickMakeButton()
        {

        }

        private void OnClickResultButton()
        {

        }

        private void OnClickBackButton()
        {
            UIManager.Instance.CloseMenu();
        }

        private void OnClickBookmarkButton()
        {

        }
    }
}

