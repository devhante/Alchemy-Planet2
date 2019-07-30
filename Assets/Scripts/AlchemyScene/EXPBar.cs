using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.AlchemyScene
{
    public class EXPBar : MonoBehaviour
    {
        [SerializeField]
        private Image EXPBarImage;
        [SerializeField]
        private Text EXPText;
        [SerializeField]
        private Text LevelText;

        void Start()
        {
            SetText();
            ChangeEXPBar();
        }

        void SetText()
        {
            LevelText.text = DataManager.Instance.CurrentPlayerData.alchemyLevel.ToString();
            EXPText.text = DataManager.Instance.CurrentPlayerData.alchemyEXP.ToString() + '%';
        }

        void ChangeEXPBar()
        {
            float width = DataManager.Instance.CurrentPlayerData.alchemyEXP * 3.5f;
            EXPBarImage.rectTransform.sizeDelta = new Vector2(width, EXPBarImage.rectTransform.sizeDelta.y);
        }
    }
}