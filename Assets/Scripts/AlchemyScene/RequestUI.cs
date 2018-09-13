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

            foreach (var item in RequestButtons)
            {
                Image[] requiers = item.transform.GetComponentsInChildren<Image>();
                Text[] texts = item.transform.GetComponentsInChildren<Text>();
                int rand = Random.Range(0, 4);
                var req = AlchemyManager.Instance.requests[rand];

                int index = 1;
                //여기 키, 값 전부 받도록 수정
                foreach (string n in req.requires.Keys)
                {
                    requiers[index].sprite = Data.DataManager.Instance.materials[n].image;
                    ++index;
                }

                texts[0].text = req.request_name;
                texts[1].text = req.unicoin.ToString();
                texts[2].text = req.cosmoston.ToString();
                texts[3].text = req.exp.ToString();

                item.onClick.AddListener(OnClickRequestButton);
            }

            BackButton.onClick.AddListener(OnClickBackButton);

            UpdateRequestButton.onClick.AddListener(OnClickUpdateRequestButton);
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

        }
    }
}
