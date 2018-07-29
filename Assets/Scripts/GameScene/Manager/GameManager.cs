using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public enum ScoreType { TimePass, TouchRightRecipe, KillMonster }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private int combo;
        public int Combo
        {
            get { return combo; }
            set { combo = value; GameUI.Instance.UpdateComboText(Combo); }
        }
        public int Score { get; set; }
        public int Coin { get; set; }
        public Dictionary<MaterialName, int> DropMaterialList { get; private set; }

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
            Combo = 0;
            Score = 0;
            Coin = 0;
            DropMaterialList = new Dictionary<MaterialName, int>();
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