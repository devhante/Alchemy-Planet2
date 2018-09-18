using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class RequestUI : Common.UI<RequestUI>
    {
        [SerializeField] private Button[] RequestButtons;
        //의뢰 리스트

        [SerializeField] private Button BackButton;
        [SerializeField] private Button UpdateRequestButton;

        protected override void Awake()
        {
            base.Awake();

            UpdateRequest();

            BackButton.onClick.AddListener(OnClickBackButton);

            UpdateRequestButton.onClick.AddListener(OnClickUpdateRequestButton);
            CheckUnicoin();
        }

        private void UpdateRequest()
        {
            for (int i = 0; i < RequestButtons.Length; ++i)
            {
                Image[] requiers = RequestButtons[i].transform.GetComponentsInChildren<Image>();
                Text[] texts = RequestButtons[i].transform.GetComponentsInChildren<Text>();

                var req = Data.DataManager.Instance.CurrentPlayerData.request[i];

                int index = 1;
                foreach (var kv in req.requires)
                {
                    requiers[index].sprite = Data.DataManager.Instance.itemInfo[kv.Key].image;
                    //개수 표현 필요
                    ++index;
                }

                texts[0].text = req.request_name;
                texts[1].text = req.unicoin.ToString();
                texts[2].text = req.cosmoston.ToString();
                texts[3].text = req.exp.ToString();

                RequestButtons[i].onClick.AddListener(OnClickRequestButton);
            }
        }

        private void OnClickRequestButton()
        {

        }

        private void OnClickBackButton()
        {
            UIManager.Instance.CloseMenu();
        }

        private void OnClickUpdateRequestButton()
        {
            if (CheckUnicoin())
            {
                Data.DataManager.Instance.CurrentPlayerData.unicoin -= 1000;
                Data.DataManager.Instance.CurrentPlayerData.UpdateRequest();
                UpdateRequest();
                Common.StateBar.Instance.UpdateState();
            }
        }

        private bool CheckUnicoin()
        {
            if (Data.DataManager.Instance.CurrentPlayerData.unicoin < 1000){
                UpdateRequestButton.image.color = new Color32(150, 150, 150, 255);
                UpdateRequestButton.onClick.RemoveAllListeners();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
