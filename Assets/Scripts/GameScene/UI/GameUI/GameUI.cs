using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public enum Gages { OXYGEN, PURIFY }

    public class GameUI : Common.UI<GameUI>
    {
        public Image OxygenGageMask;
        public Image PurifyGageMask;
        public Button PauseButton;
        public Text Score;
        public Text Unicoin;
        public Text ComboText;

        public bool IsIncreasingPurify { get; set; }
        public bool IsNotReducingOxygen { get; set; }
        public float OxygenReduceSpeed { get; set; }

        float oxygenReduceRate = 1;
        float purifyReduceRate = 2;

        Dictionary<Gages, float> gageValues;

        protected override void Awake()
        {
            base.Awake();

            PauseButton.onClick.AddListener(() =>
            {
                UIManager.Instance.OpenMenu<PauseUI>();
            });

            gageValues = new Dictionary<Gages, float>();
            gageValues.Add(Gages.OXYGEN, 100);
            gageValues.Add(Gages.PURIFY, 50);
        }

        public void Start()
        {
            IsIncreasingPurify = false;
            IsNotReducingOxygen = false;
            OxygenReduceSpeed = 1;

            UpdateGage(Gages.PURIFY, 0);
            GameManager.Instance.StartCoroutine("GainScoreByTimeCoroutine");

            StartCoroutine("UpdateOxygenGage");
            StartCoroutine("UpdatePurifyGage");
            StartCoroutine("OxygenMinus");
            StartCoroutine("PurifyMinus");
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
            if (IsIncreasingPurify && kind == Gages.PURIFY && percent <= 0) percent = 0;
            if (Popin.Instance.PotionGreen && kind == Gages.PURIFY && percent <= 0) percent = 0;
            if (IsNotReducingOxygen && kind == Gages.OXYGEN) percent = 0;

            gageValues[kind] = Mathf.Clamp(gageValues[kind] + percent, 0, 100);

            if (gageValues[Gages.OXYGEN] == 0) UIManager.Instance.OpenMenu<EndUI>();
        }

        public float GetGage(Gages kind)
        {
            return gageValues[kind];
        }

        IEnumerator UpdateOxygenGage()
        {
            while(true)
            {
                float oxygenGageValue = OxygenGageMask.transform.localScale.x;
                while(oxygenGageValue != gageValues[Gages.OXYGEN])
                {
                    oxygenGageValue = Mathf.Lerp(oxygenGageValue, gageValues[Gages.OXYGEN], 0.1f);
                    OxygenGageMask.transform.localScale = new Vector3(oxygenGageValue / 100, 1, 1);
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }

        IEnumerator UpdatePurifyGage()
        {
            while(true)
            {
                float purifyGageValue = PurifyGageMask.transform.localScale.x;
                while(purifyGageValue != gageValues[Gages.PURIFY])
                {
                    purifyGageValue = Mathf.Lerp(purifyGageValue, gageValues[Gages.PURIFY], 0.1f);
                    PurifyGageMask.transform.localScale = new Vector3(purifyGageValue / 100, 1, 1);
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }

        IEnumerator OxygenMinus() 
        {
            float frame = 1;

            while (true)
            {
                float purifyGageSquare = (gageValues[Gages.PURIFY] * 0.01f) * (gageValues[Gages.PURIFY] * 0.01f);
                yield return new WaitForSeconds(frame * (1 + purifyGageSquare) * OxygenReduceSpeed);
                UpdateGage(Gages.OXYGEN, -oxygenReduceRate * frame);
            }
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
    }
}