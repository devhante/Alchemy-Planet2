using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class MakeUI : Common.UI<MakeUI> {
        [SerializeField] public Button[] MateraiButtons = new Button[5];

        [SerializeField] public Button ResultButton;
        [SerializeField] private Button MakeButton;
        [SerializeField] private Button BackButton;
        [SerializeField] private Button BookmarkButton;

        public Data.FormulaData selected_formula;

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

        public void SetFormula()
        {
            ResultButton.image.sprite = Data.DataManager.Instance.itemInfo[selected_formula.result].image;
            int count = 0;
            foreach (var kv in selected_formula.formula)
            {
                MateraiButtons[count].image.sprite =
                    Data.DataManager.Instance.itemInfo[kv.Key].image;

                int inven_item_count = 0;
                Data.DataManager.Instance.CurrentPlayerData.inventory.TryGetValue(kv.Key, out inven_item_count);

                MateraiButtons[count++].GetComponentInChildren<Text>().text =
                    string.Format("{0} / {1}", kv.Value, inven_item_count);

                if (kv.Value > inven_item_count)
                {
                    MakeButton.image.color = new Color32(150, 150, 150, 255);
                    MakeButton.onClick.RemoveListener(OnClickMakeButton);
                }
            }
        }

        private void OnClickMateraiButton()
        {
            //TownScene.UIManager.Instance.OpenMenu<TownScene.InventoryCell>();
        }

        private void OnClickMakeButton()
        {
            foreach (var kv in selected_formula.formula)
            {
                Data.DataManager.Instance.CurrentPlayerData.inventory[kv.Key] -= kv.Value;
                if (Data.DataManager.Instance.CurrentPlayerData.inventory[kv.Key] <= 0)
                    Data.DataManager.Instance.CurrentPlayerData.inventory.Remove(kv.Key);
            }
            Data.DataManager.Instance.CurrentPlayerData.inventory.Add(selected_formula.result, 1);
            UIManager.Instance.CloseMenu();
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

