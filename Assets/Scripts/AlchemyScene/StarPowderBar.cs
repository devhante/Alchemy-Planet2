using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.AlchemyScene
{
    public class StarPowderBar : MonoBehaviour
    {
        [SerializeField]
        private Text StarPowderText;

        void Start()
        {
            SetText();
        }

        void SetText()
        {
            StarPowderText.text = DataManager.Instance.CurrentPlayerData.starPowder.ToString();
        }
    }
}