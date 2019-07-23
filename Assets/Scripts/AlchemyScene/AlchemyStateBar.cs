using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.AlchemyScene
{
    public class AlchemyStateBar : MonoBehaviour
    {
        [SerializeField]
        private Image EXPBarImage;
        [SerializeField]
        private Text EXPText;
        [SerializeField]
        private Text LevelText;
        [SerializeField]
        private Text StarPowderText;


        void Start()
        {
            SetText();
            ChangeEXPBar();
        }

        void SetText()
        {
            LevelText.text = DataManager.Instance.CurrentPlayerData.alchemyLevel.ToString();
            EXPText.text = DataManager.Instance.CurrentPlayerData.alchemyEXP.ToString() + '%';
            StarPowderText.text = DataManager.Instance.CurrentPlayerData.starPowder.ToString();
        }

        void ChangeEXPBar()
        {
            float width = DataManager.Instance.CurrentPlayerData.alchemyEXP * 3.5f;
            EXPBarImage.rectTransform.sizeDelta = new Vector2(width, EXPBarImage.rectTransform.sizeDelta.y);
        }
    }
}