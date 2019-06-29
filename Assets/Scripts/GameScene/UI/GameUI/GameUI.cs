using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public enum Gages { OXYGEN, PURIFY }

    public class GameUI : Common.UI<GameUI>
    {
        public Image PurifyGageMask;
        public Button PauseButton;
        public Text Score;
        public Text Unicoin;
        public Text ComboText;
        public GameObject BombDestination;
        public Image DangerMask;
        public Button SkillButton;
        public Image BluePotionImage;
        public Image BlackPotionImage;

        public bool IsIncreasingPurify { get; set; }
        float purifyReduceRate = 4;

        Dictionary<Gages, float> gageValues;

        protected override void Awake()
        {
            base.Awake();

            PauseButton.onClick.AddListener(() =>
            {
                UIManager.Instance.OpenMenu<PauseUI>();
            });

            SkillButton.onClick.AddListener(() =>
            {
                Popin.Instance.Skill();
            });

            SkillButton.enabled = false;

            gageValues = new Dictionary<Gages, float>();
            gageValues.Add(Gages.OXYGEN, 100);
            gageValues.Add(Gages.PURIFY, 0);
        }

        public void Start()
        {
            IsIncreasingPurify = false;

            GameManager.Instance.StartCoroutine("GainScoreByTimeCoroutine");
            StartCoroutine("UpdatePurifyGage");
            StartCoroutine("UpdateSkillIcon");
        }

        public Image GetMask(Gages kind)
        {
            Image mask = null;

            switch (kind)
            {
                case Gages.PURIFY: mask = PurifyGageMask; break;
            }

            return mask;
        }

        public void UpdateGage(Gages kind, float percent)
        {
            Debug.Log("percent = " + percent);
            if (IsIncreasingPurify && kind == Gages.PURIFY && percent <= 0) percent = 0;

            gageValues[kind] = Mathf.Clamp(gageValues[kind] + percent, 0, 100);
            Debug.Log(gageValues[kind]);

            if (gageValues[Gages.OXYGEN] == 0)
            {
                UIManager.Instance.OpenMenu<EndUI>();
                if (StoryLobbyScene.StoryManager.Instance != null)
                {
                    SceneManager.sceneLoaded -= StoryLobbyScene.StoryManager.Instance.OnSceneLoaded;
                    Destroy(StoryLobbyScene.StoryManager.Instance.gameObject);
                }
            }
        }

        public float GetGage(Gages kind)
        {
            return gageValues[kind];
        }

        IEnumerator UpdatePurifyGage()
        {
            while (true)
            {
                float purifyGageValue = PurifyGageMask.rectTransform.offsetMin.x / 720 * 100;
                while (purifyGageValue != gageValues[Gages.PURIFY])
                {
                    purifyGageValue = Mathf.Lerp(purifyGageValue, gageValues[Gages.PURIFY], 0.1f);
                    PurifyGageMask.rectTransform.offsetMin = new Vector2(purifyGageValue / 100 * 720, PurifyGageMask.rectTransform.offsetMin.y);
                    yield return new WaitForSeconds(0.01f);
                }

                yield return null;
            }
        }

        IEnumerator UpdateSkillIcon()
        {
            bool isSkillOn = false;

            while(true)
            {
                if (isSkillOn != Popin.Instance.isSkillOn)
                {
                    isSkillOn = Popin.Instance.isSkillOn;

                    if (Popin.Instance.isSkillOn == true)
                    {
                        SkillButton.enabled = true;

                        if (Popin.Instance.color == PopinPotionColor.Blue)
                        {
                            BlackPotionImage.gameObject.SetActive(false);
                            BluePotionImage.gameObject.SetActive(true);
                        }
                        else if (Popin.Instance.color == PopinPotionColor.Black)
                        {
                            BluePotionImage.gameObject.SetActive(false);
                            BlackPotionImage.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        SkillButton.enabled = false;

                        BlackPotionImage.gameObject.SetActive(false);
                        BluePotionImage.gameObject.SetActive(false);
                    }
                }
                yield return null;
            }
        }
    }
}