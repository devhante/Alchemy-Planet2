using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace AlchemyPlanet.GameScene
{
    public class EndUI : Common.UI<EndUI>
    {
        public Button ButtonRestart;
        public Button ButtonWorldMap;
        public Text Score;
        public Text Unicoin;

        protected override void Awake()
        {
            Time.timeScale = 0;
            GameManager.Instance.EndGame();

            Score.text = GameManager.Instance.Score.ToString("#,##0");
            Unicoin.text = GameManager.Instance.UniCoin.ToString("#,##0");

            ButtonRestart.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });

            ButtonWorldMap.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                LoadingSceneManager.LoadScene("PlaySceneSelect");
            });
        }
    }
}