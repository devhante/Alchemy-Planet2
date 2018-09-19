using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.Common {
    public class DialogScene : MonoBehaviour
    {
        public static DialogScene Instance;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            TownScene.UIManager.Instance.OpenMenu<TownScene.DialogUI>();
            TownScene.DialogUI.Instance.SetDialog();
        }

        public void IsOver()
        {
            //Tutorial.Instance.process++;
            //Tutorial.Instance.CheckCurrentScene();
        }
    }

}