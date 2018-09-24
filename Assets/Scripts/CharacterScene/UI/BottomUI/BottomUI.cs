using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.CharacterScene
{
    public class BottomUI : MonoBehaviour
    {
        public static BottomUI Instance { get; private set; }

        public Button buttonOrganize;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            buttonOrganize.onClick.AddListener(OnClickButtonOrganize);
        }

        private void OnClickButtonOrganize()
        {
            //테스트
            Common.AlertManager.Instance.InstantiateAlert(transform, "준비중입니다.");

            //Destroy(GameManager.Instance.CurrentCharacters.gameObject);

            //UIManager.Instance.DestroyUI();
            //UIManager.Instance.CreateOrganizeUI();
        }
    }
}