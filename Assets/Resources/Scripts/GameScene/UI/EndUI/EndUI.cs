using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace AlchemyPlanet.GameScene
{
    public class EndUI : UI<EndUI>
    {
        public Button Restart;
        public Button Exit;
        public Text timetext;

        protected override void Awake()
        {
            Time.timeScale = 0;
            GameManager.Instance.EndGame();
            timetext.text = string.Format("{0}:{1}", (int)GameManager.Instance.PlayTime / 60, GameManager.Instance.PlayTime % 60);

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