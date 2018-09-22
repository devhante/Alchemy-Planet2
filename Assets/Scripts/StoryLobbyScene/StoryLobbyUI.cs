using AlchemyPlanet.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.StoryLobbyScene
{
    public class StoryLobbyUI : MonoBehaviour
    {
        public Button starButton;
        public Button starDescriptionButton;
        public Image[] starImages;
        public Image[] starDescriptionImages;

        public Sprite starOff;
        public Sprite starOn;

        private List<StoryChallengeData> list;

        private void Awake()
        {
            starButton.onClick.AddListener(OnClickStarButton);
            starDescriptionButton.onClick.AddListener(OnClickStarDescriptionButton);

            list = DataManager.Instance.LoadStoryChallenges();
        }

        private void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                starDescriptionButton.transform.GetChild(1).GetChild(i).GetComponent<Text>().text = list[0].challenges[i];
            }
        }

        private void OnClickStarButton()
        {
            starDescriptionButton.gameObject.SetActive(!starDescriptionButton.gameObject.activeSelf);
        }

        private void OnClickStarDescriptionButton()
        {
            starDescriptionButton.gameObject.SetActive(false);
        }
    }
}
