using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public enum Gages { OXYGEN, PURIFY }

    public class GameUI : Common.UI<GameUI>
    {
        public static new GameUI Instance;

        public Text Countdown;
        public Image OxygenGageMask;
        public Image PurifyGageMask;
        public Button PauseButton;
        public Text Score;
        public Text Unicoin;
        public Text ChickenCount;

        public bool IsIncreasingPurify { get; set; }
        public bool IsNotReducingOxygen { get; set; }
        public float OxygenReduceSpeed { get; set; }

        float oxygenReduceRate = 1;
        float purifyReduceRate = 2;

        protected override void Awake()
        {
            Instance = this;
            PauseButton.onClick.AddListener(() =>
            {
                UIManager.Instance.OpenMenu<PauseUI>();
            });
        }

        public void Start()
        {
            IsIncreasingPurify = false;
            IsNotReducingOxygen = false;
            OxygenReduceSpeed = 1;

            UpdateGage(Gages.PURIFY, 0);
            GameManager.Instance.StartCoroutine("GainScoreByTimeCoroutine");

            StartCoroutine("PurifyMinus");
            StartCoroutine("OxygenMinus");
        }

        public Image GetMask(Gages kind)
        {
            Image mask = null;

            switch (kind)
            {
                case Gages.OXYGEN: mask = OxygenGageMask; break;
                case Gages.PURIFY: mask = PurifyGageMask; break;
            }

            return mask;
        }

        public void UpdateGage(Gages kind, float percent)
        {
            if (IsIncreasingPurify && kind == Gages.PURIFY) percent = 0;
            if (IsNotReducingOxygen && kind == Gages.OXYGEN) percent = 0;

            Image mask = GetMask(kind);
            float x = Mathf.Clamp(mask.transform.localScale.x + (percent / 100), 0, 1);
            mask.transform.localScale = new Vector3(x, 1, 1);

            if (kind == Gages.OXYGEN && mask.transform.localScale.x == 0) UIManager.Instance.OpenMenu<EndUI>();
        }

        public void IncreasePurifyForItem(float percent)
        {
            Image mask = GetMask(Gages.PURIFY);
            float x = Mathf.Clamp(mask.transform.localScale.x + (percent / 100), 0, 1);
            mask.transform.localScale = new Vector3(x, 1, 1);
        }

        public float GetGage(Gages kind)
        {
            return GetMask(kind).transform.localScale.x;
        }

        IEnumerator PurifyMinus()
        {
            float frame = 0.1f;
            WaitForSeconds wait = new WaitForSeconds(frame);

            while (true)
            {
                yield return wait;
                UpdateGage(Gages.PURIFY, -purifyReduceRate * frame);
            }
        }

        IEnumerator OxygenMinus()
        {
            float frame = 1;

            while (true)
            {
                yield return new WaitForSeconds(frame * (1 + GetGage(Gages.PURIFY) * GetGage(Gages.PURIFY)) * OxygenReduceSpeed);
                UpdateGage(Gages.OXYGEN, -oxygenReduceRate * frame);
            }
        }

        IEnumerator ResumeCoroutine()
        {
            Countdown.gameObject.SetActive(true);

            for (int i = 3; i > 0; i--)
            {
                Countdown.text = i.ToString();
                yield return new WaitForSecondsRealtime(1);
            }

            Time.timeScale = 1;
            Countdown.gameObject.SetActive(false);
            yield return null;
        }
    }
}