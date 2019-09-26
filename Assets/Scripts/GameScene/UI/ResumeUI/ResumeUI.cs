using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public class ResumeUI : Common.UI<ResumeUI>
    {
        public Text Countdown;
        public Button PauseButton;

        protected override void Awake()
        {
            base.Awake();

            PauseButton.onClick.AddListener(OnClickPauseButton);
        }

        private void Start()
        {
            StartCoroutine(ResumeCoroutine());
        }

        private void OnClickPauseButton()
        {
            UIManager.Instance.OpenMenu<PauseUI>();
            Destroy(gameObject);
        }

        IEnumerator ResumeCoroutine()
        {
            for(int i = 3; i > 0; i--)
            {
                Countdown.text = i.ToString();
                yield return new WaitForSecondsRealtime(1);
            }

            Time.timeScale = 1;
            Destroy(gameObject);
        }
    }
}