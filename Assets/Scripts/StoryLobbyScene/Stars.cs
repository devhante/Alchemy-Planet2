using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.StoryLobbyScene
{
    public class Stars : MonoBehaviour
    {
        public static Stars Instance { get; private set; }

        public Star[] stars;
        public Text[] starDescriptionTexts;
        public Image[] starDescriptionImages;
        public Button starButton;
        public Button starDescriptionButton;
        public Sprite starOn;
        public Sprite starOff;

        private List<StoryChallengeData> list;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;

            starButton.onClick.AddListener(OnClickStarButton);
            starDescriptionButton.onClick.AddListener(OnClickStarDescriptionButton);
        }

        private void Start()
        {
            list = DataManager.Instance.LoadStoryChallenges();
            LoadStarDescriptionText();
        }

        private void OnClickStarButton()
        {
            starDescriptionButton.gameObject.SetActive(!starDescriptionButton.gameObject.activeSelf);

            if (starDescriptionButton.gameObject.activeSelf == true)
            {
                starDescriptionButton.transform.localScale = Vector3.zero;
                Sequence sq = DOTween.Sequence();
                sq.Append(starDescriptionButton.transform.DOScale(Vector3.one, 0.5f))
                    .SetEase(Ease.OutQuint);
            }
        }

        private void OnClickStarDescriptionButton()
        {
            starDescriptionButton.gameObject.SetActive(false);
        }

        private void LoadStarDescriptionText()
        {
            for (int i = 0; i < 3; i++)
            {
                starDescriptionTexts[i].text = list[StoryManager.Instance.CurrentStage - 1].challenges[i];
            }
        }

        private void LoadStarDescriptionImage()
        {
            for(int i = 0; i < 3; i++)
            {
                if (stars[i].rate >= 1)
                    starDescriptionImages[i].sprite = starOn;
                else
                    starDescriptionImages[i].sprite = starOff;
            }
        }
    }
}