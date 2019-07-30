using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public enum ScoreType { TimePass, TouchRightRecipe, KillMonster }
    public enum ComboStatus { Combo0, Combo10, Combo30, Combo50, Combo100 }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public int Second { get; set; }

        private int combo;
        private int score;
        private int coin;

        public int Combo
        {
            get { return combo; }
            set
            {
                combo = value;
                UpdateCombo();
                UpdateComboStatus();
            }
        }
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                UpdateScore();
            }
        }
        public int Coin
        {
            get { return coin; }
            set
            {
                coin = value;
                UpdateCoin();
            }
        }

        public float MoveSpeed { get; set; }
        public int Position { get; set; }
        public ComboStatus ComboStatus { get; private set; }
        public Dictionary<string, int> dropMaterialList;

        private void OnApplicationPause(bool pause)
        {
            if (pause && PauseUI.Instance == null)
            {
                UIManager.Instance.OpenMenu<PauseUI>();

                if (ResumeUI.Instance != null)
                    Destroy(ResumeUI.Instance.gameObject);
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public GameObject popinPrefab;

        private void Awake()
        {
            Instance = this;
            Second = 0;
            Combo = 0;
            Score = 0;
            Coin = 0;
            MoveSpeed = 1;
            Position = 0;
            dropMaterialList = new Dictionary<string, int>();
            Instantiate(popinPrefab, GameObject.Find("Character").transform);
            
        }

        private void Start()
        {
            StartCoroutine("SecondCoroutine");
            StartCoroutine("GameCoroutine");
        }

        IEnumerator SecondCoroutine()
        {
            yield return new WaitForSeconds(1);
            Second++;
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
            int value = 0;
            float scoreRate = 1;

            switch (scoreType)
            {
                case ScoreType.TimePass:            value += 1; break;
                case ScoreType.TouchRightRecipe:    value += 100; break;
                case ScoreType.KillMonster:         value += 1500; break;
            }

            switch(ComboStatus)
            {
                case ComboStatus.Combo10:   scoreRate += 0.05f; break;
                case ComboStatus.Combo30:   scoreRate += 0.08f; break;
                case ComboStatus.Combo50:   scoreRate += 0.1f; break;
                case ComboStatus.Combo100:  scoreRate += 0.12f; break;
            }

            Score += (int)(value * scoreRate);
        }

        public void GainScore(ScoreType scoreType, out int result)
        {
            int value = 0;
            float scoreRate = 1;

            switch (scoreType)
            {
                case ScoreType.TimePass: value += 1; break;
                case ScoreType.TouchRightRecipe: value += 100; break;
                case ScoreType.KillMonster: value += 1500; break;
            }

            //switch (ComboStatus)
            //{
            //    case ComboStatus.Combo10: scoreRate += 0.05f; break;
            //    case ComboStatus.Combo30: scoreRate += 0.08f; break;
            //    case ComboStatus.Combo50: scoreRate += 0.1f; break;
            //    case ComboStatus.Combo100: scoreRate += 0.12f; break;
            //}

            scoreRate += GameUI.Instance.GetGage(Gages.PURIFY) * 2;

            result = (int)(value * scoreRate);
            Score += result;
        }

        private void UpdateScore()
        {
            GameUI.Instance.Score.text = Score.ToString("#,##0");
        }

        public void GainCoin(int value)
        {
            float coinRate = 1;

            switch (ComboStatus)
            {
                case ComboStatus.Combo50: coinRate += 0.1f; break;
                case ComboStatus.Combo100: coinRate += 0.15f; break;
            }

            Coin += (int)(value * coinRate);
        }

        private void UpdateCoin()
        {
            GameUI.Instance.Unicoin.text = Coin.ToString("#,##0");
        }

        public void UpdateCombo()
        {
            if (Combo == 0)
                ComboText.Instance.SetOpacity(0);
            else
            {
                ComboText.Instance.SetOpacity(1);
                ComboText.Instance.FadeOut();
            }
            GameUI.Instance.ComboText.text = string.Format("{0} Combo!", combo);
        }

        public void UpdateComboStatus()
        {
            if (Combo < 10) ComboStatus = ComboStatus.Combo0;
            else if (Combo < 30) ComboStatus = ComboStatus.Combo10;
            else if (Combo < 50) ComboStatus = ComboStatus.Combo30;
            else if (Combo < 100) ComboStatus = ComboStatus.Combo50;
            else ComboStatus = ComboStatus.Combo100;
        }

        IEnumerator GameCoroutine()
        {
            while(true)
            {
                yield return new WaitForSeconds(2);

                MoveSpeed = 0;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Idle();
                MonsterManager.Instance.SpawnMonster();

                while(MonsterManager.Instance.Monsters.Count > 0)
                {
                    yield return new WaitForEndOfFrame();
                }

                MoveSpeed = 1;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Run();
            }
        }
    }
}