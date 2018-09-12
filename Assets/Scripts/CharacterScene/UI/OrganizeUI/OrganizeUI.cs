using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.CharacterScene
{
    public class OrganizeUI : MonoBehaviour
    {
        public static OrganizeUI Instance { get; private set; }

        public Button leftButton;
        public Button rightButton;
        public PageLights partyBoxPageLights;
        public PageLights characterListPageLights;
        public Button okayButton;
        public Button backButton;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            leftButton.onClick.AddListener(OnClickLeftButton);
            rightButton.onClick.AddListener(OnClickRightButton);
            okayButton.onClick.AddListener(OnClickOkayButton);
            backButton.onClick.AddListener(OnClickBackButton);
        }

        private void OnClickLeftButton()
        {
            partyBoxPageLights.ChangeLightToLeft();
        }

        private void OnClickRightButton()
        {
            partyBoxPageLights.ChangeLightToRight();
        }

        private void OnClickOkayButton()
        {
            UIManager.Instance.DestroyUI();
            UIManager.Instance.CreateMainUI();
        }

        private void OnClickBackButton()
        {
            UIManager.Instance.DestroyUI();
            UIManager.Instance.CreateMainUI();
        }
    }
}