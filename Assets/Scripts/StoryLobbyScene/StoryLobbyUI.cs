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
        public Text stageText;
        public GameObject stageCircles;
        public GameObject stageCircle;
        public GameObject stageBoss;
        public Button buttonLeft;
        public Button buttonRight;

        public Sprite starOff;
        public Sprite starOn;
        public Sprite stageCircleWhite;
        public Sprite stageCircleBlack;
        public Sprite stageCircleYellow;

        public int CurrentChaper { get; set; }
        public int CurrentStage { get; set; }
        public int CurrentMaxStage { get; set; }

        private List<StoryChallengeData> list;

        private void Awake()
        {
            starButton.onClick.AddListener(OnClickStarButton);
            starDescriptionButton.onClick.AddListener(OnClickStarDescriptionButton);

            CurrentChaper = 1;
            CurrentMaxStage = 7;
            list = DataManager.Instance.LoadStoryChallenges();
        }

        private void Start()
        {
            InstantiateStageCircles(CurrentMaxStage);

            for (int i = 0; i < 3; i++)
            {
                stageText.text = string.Format("STAGE {0}-{1}", CurrentChaper.ToString(), CurrentStage.ToString());
                starDescriptionButton.transform.GetChild(1).GetChild(i).GetComponent<Text>().text = list[0].challenges[i];

                if (DataManager.Instance.CurrentPlayerData.stroystar[CurrentChaper.ToString() + "-" + CurrentStage.ToString()] > i)
                {
                    starImages[i].sprite = starOn;
                    starDescriptionImages[i].sprite = starOn;
                }
                else
                {
                    starImages[i].sprite = starOff;
                    starDescriptionImages[i].sprite = starOff;
                }
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

        private void OnClickButtonLeft()
        {
            if (CurrentStage == 1) return;
            if (CurrentStage != CurrentMaxStage)
                stageCircles.transform.GetChild(CurrentStage - 1).GetComponent<Image>().sprite = stageCircleWhite;
            stageCircles.transform.GetChild(CurrentStage - 1).GetComponent<Image>().sprite = stageCircleYellow;
            CurrentStage--;
            stageText.text = string.Format("STAGE {0}-{1}", CurrentChaper.ToString(), CurrentStage.ToString());
        }

        private void OnClickButtonRight()
        {
            if (CurrentStage == CurrentMaxStage) return;
            if (stageCircles.transform.GetChild(CurrentStage).GetComponent<Image>().sprite == stageCircleBlack) return;
            if (CurrentStage != CurrentMaxStage - 1)
                stageCircles.transform.GetChild(CurrentStage).GetComponent<Image>().sprite = stageCircleYellow;
            stageCircles.transform.GetChild(CurrentStage - 1).GetComponent<Image>().sprite = stageCircleWhite;
            CurrentStage++;
            stageText.text = string.Format("STAGE {0}-{1}", CurrentChaper.ToString(), CurrentStage.ToString());
        }

        private void InstantiateStageCircles(int stageNumber)
        {
            float distance = 400.0f / (stageNumber - 1);
            for(int i = 0; i < stageNumber; i++)
            {
                GameObject instance;
                if (i == stageNumber - 1)
                    instance = Instantiate(stageBoss, stageCircles.transform);
                else
                    instance = Instantiate(stageCircle, stageCircles.transform);

                instance.transform.position = stageCircles.transform.position + new Vector3(-200 + (distance * i), 0);
            }

            bool isCleared = true;
            for(int i = 1; i < stageNumber; i++)
            {
                if (isCleared == false)
                    stageCircles.transform.GetChild(i - 1).GetComponent<Image>().sprite = stageCircleBlack;
                else if (DataManager.Instance.CurrentPlayerData.stroystar[CurrentChaper.ToString() + "-" + i.ToString()] == 0)
                {
                    stageCircles.transform.GetChild(i - 1).GetComponent<Image>().sprite = stageCircleYellow;
                    CurrentStage = i;
                    isCleared = false;
                }
            }
        }
    }
}
