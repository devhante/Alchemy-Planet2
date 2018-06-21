using System.Collections;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public enum ScoreType { TimePass, TouchRightRecipe, KillMonster }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public float PlayTime { get; private set; }
        public int Score { get; private set; }
        public int UniCoin { get; set; }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            Score = 0;
            UniCoin = 0;
        }

        public void StartGame()
        {
            PlayTime = 0;
            StartCoroutine("TimeCount");
        }

        public void EndGame()
        {
            StopCoroutine("TimeCount");
        }

        private IEnumerator TimeCount()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                PlayTime++;
            }
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
                    Score += 1;
                    break;

                case ScoreType.TouchRightRecipe:
                    Score += 100;
                    break;

                case ScoreType.KillMonster:
                    Score += 1500;
                    break;
            }

            UpdateScore();
        }

        private void UpdateScore()
        {
            GameUI.Instance.Score.text = Score.ToString("#,##0");
        }
    }
}