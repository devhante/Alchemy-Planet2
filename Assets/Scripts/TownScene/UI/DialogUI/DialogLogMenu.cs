using UnityEngine;
using UnityEngine.UI;


namespace AlchemyPlanet.TownScene
{
    public class DialogLogMenu : Common.UI<DialogLogMenu>
    {
        public Button CloseButton;
        public GameObject LogLayout;
        //LogPrefab은 반드시 Text를 둘 포함하고 있어야 한다.
        public GameObject LogPrefab;

        [SerializeField] private Button closeButton;

        protected override void Awake()
        {
            base.Awake();

            CloseButton.onClick.AddListener(() => {
                UIManager.Instance.CloseMenu();
            });

            for (int i=0; i<DialogUI.Instance.count; ++i)
            {
                GameObject LogItem = Instantiate(LogPrefab, LogLayout.transform);
                Text[]log = LogItem.GetComponentsInChildren<Text>();

                if (DialogUI.Instance.NPC)
                {
                    log[0].text = DialogUI.Instance.NPC.data.dialogs[i].name;
                    log[1].text = DialogUI.Instance.NPC.data.dialogs[i].content;
                }
                else
                {
                    log[0].text = Data.DataManager.Instance.selected_dialog.dialogs[i].name;
                    log[1].text = Data.DataManager.Instance.selected_dialog.dialogs[i].content;
                }
            }
        }
    }
}