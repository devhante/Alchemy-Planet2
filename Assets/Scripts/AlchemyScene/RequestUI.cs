using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class RequestUI : Common.UI<RequestUI>
    {
        [SerializeField] private Button[] RequestButtons = new Button[4];
        //의뢰 리스트

        [SerializeField] private Button BackButton;
        [SerializeField] private Button UpdateRequestButton;

        protected override void Awake()
        {
            base.Awake();

            foreach (var item in RequestButtons)
                item.onClick.AddListener(OnClickRequestButton);

            BackButton.onClick.AddListener(OnClickBackButton);

            UpdateRequestButton.onClick.AddListener(OnClickUpdateRequestButton);
        }

        private void OnClickRequestButton()
        {

        }

        private void OnClickBackButton()
        {

        }

        private void OnClickUpdateRequestButton()
        {

        }
    }
}
