using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public class PauseUI : Common.UI<PauseUI>
    {
        public Button Resume;
        public Button Restart;
        public Button Exit;

        protected override void Awake()
        {
            base.Awake();

            Time.timeScale = 0;

            Resume.onClick.AddListener(() =>
            {
                GameUI.Instance.StartCoroutine("ResumeCoroutine");
                Destroy(gameObject);
            });

            Restart.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });

            Exit.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }
    }
}