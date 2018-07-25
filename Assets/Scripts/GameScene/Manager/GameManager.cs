using System.Collections;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public enum ScoreType { TimePass, TouchRightRecipe, KillMonster }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [HideInInspector]
        public int score;
        [HideInInspector]
        public int coin;

        private void OnApplicationPause(bool pause)
        {
            if (pause) UIManager.Instance.OpenMenu<PauseUI>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            score = 0;
            coin = 0;
        }

        public IEnumerator GainScoreByTimeCoroutine()
        {
            while(true)
            {
                yield return new WaitForSeconds(0.1f);
                GainScore(ScoreType.TimePass);
            }
        }

        public void GainScore(ScoreType scoreType)
        {
            switch (scoreType)
            {
                case ScoreType.TimePass:
                    score += 1;
                    break;

                case ScoreType.TouchRightRecipe:
                    score += 100;
                    break;

                case ScoreType.KillMonster:
                    score += 1500;
                    break;
            }

            UpdateScore();
        }

        private void UpdateScore()
        {
            GameUI.Instance.Score.text = score.ToString("#,##0");
        }
    }
}