using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.CharacterScene
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance = null;

        public GameObject partyUI;
        public GameObject bottomUI;
        public GameObject organizeUI;

        private List<GameObject> uiList = new List<GameObject>();

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            CreateMainUI();
        }

        public void DestroyUI()
        {
            foreach (var item in uiList)
                Destroy(item);
        }

        public void CreateMainUI()
        {
            uiList.Add(Instantiate(partyUI, transform));
            uiList.Add(Instantiate(bottomUI, transform));
        }

        public void CreateOrganizeUI()
        {
            uiList.Add(Instantiate(organizeUI, transform));
        }
    }
}
