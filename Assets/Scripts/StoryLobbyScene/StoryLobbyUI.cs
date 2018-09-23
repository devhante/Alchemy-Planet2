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

        

        private List<StoryChallengeData> list;

        private void Awake()
        {
            starButton.onClick.AddListener(OnClickStarButton);
            starDescriptionButton.onClick.AddListener(OnClickStarDescriptionButton);
            buttonLeft.onClick.AddListener(OnClickButtonLeft);
            buttonRight.onClick.AddListener(OnClickButtonRight);
            list = DataManager.Instance.LoadStoryChallenges();
        }

        private void Start()
        {
            StoryManager.Instance.CurrentChaper = 1;
            StoryManager.Instance.CurrentMaxStage = 7;
            InstantiateStageCircles(StoryManager.Instance.CurrentMaxStage);

            for (int i = 0; i < 3; i++)
            {
                stageText.text = string.Format("STAGE {0}-{1}", StoryManager.Instance.CurrentChaper.ToString(), StoryManager.Instance.CurrentStage.ToString());
                starDescriptionButton.transform.GetChild(1).GetChild(i).GetComponent<Text>().text = list[StoryManager.Instance.CurrentStage - 1].challenges[i];

                if (DataManager.Instance.CurrentPlayerData.stroystar[StoryManager.Instance.CurrentChaper.ToString() + "-" + StoryManager.Instance.CurrentStage.ToString()] > i)
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
            if (StoryManager.Instance.CurrentStage == 1) return;
            if (StoryManager.Instance.CurrentStage != StoryManager.Instance.CurrentMaxStage)
                stageCircles.transform.GetChild(StoryManager.Instance.CurrentStage - 1).GetComponent<Image>().sprite = stageCircleWhite;
            stageCircles.transform.GetChild(StoryManager.Instance.CurrentStage - 2).GetComponent<Image>().sprite = stageCircleYellow;
            StoryManager.Instance.CurrentStage--;
            stageText.text = string.Format("STAGE {0}-{1}", StoryManager.Instance.CurrentChaper.ToString(), StoryManager.Instance.CurrentStage.ToString());
        }

        private void OnClickButtonRight()
        {
            if (StoryManager.Instance.CurrentStage == StoryManager.Instance.CurrentMaxStage) return;
            if (stageCircles.transform.GetChild(StoryManager.Instance.CurrentStage).GetComponent<Image>().sprite == stageCircleBlack) return;
            if (StoryManager.Instance.CurrentStage != StoryManager.Instance.CurrentMaxStage - 1)
                stageCircles.transform.GetChild(StoryManager.Instance.CurrentStage).GetComponent<Image>().sprite = stageCircleYellow;
            stageCircles.transform.GetChild(StoryManager.Instance.CurrentStage - 1).GetComponent<Image>().sprite = stageCircleWhite;
            StoryManager.Instance.CurrentStage++;
            stageText.text = string.Format("STAGE {0}-{1}", StoryManager.Instance.CurrentChaper.ToString(), StoryManager.Instance.CurrentStage.ToString());
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
                else if (DataManager.Instance.CurrentPlayerData.stroystar[StoryManager.Instance.CurrentChaper.ToString() + "-" + i.ToString()] == 0)
                {
                    stageCircles.transform.GetChild(i - 1).GetComponent<Image>().sprite = stageCircleYellow;
                    StoryManager.Instance.CurrentStage = i;
                    isCleared = false;
                }
            }
        }
    }
}
