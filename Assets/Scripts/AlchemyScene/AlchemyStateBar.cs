using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        private int EXP;
        private int Level;

        // Use this for initialization
        void Start()
        {
            EXP = 0;
            Level = 0;
            SetText();
            ChangeEXPBar();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void SetText()
        {
            LevelText.text = Level.ToString();
            EXPText.text = EXP.ToString();
        }

        void ChangeEXPBar()
        {
            EXPBarImage.rectTransform.sizeDelta = new Vector2( EXP, EXPBarImage.rectTransform.sizeDelta.y );
        }
    }
}