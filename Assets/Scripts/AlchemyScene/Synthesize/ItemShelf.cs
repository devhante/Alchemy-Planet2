using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class ItemShelf : MonoBehaviour
    {
        [SerializeField]
        private GameObject itemPage;
        [SerializeField]
        private List<Button> typeSelectButtonList;
        [SerializeField]
        private List<string> typeSelectNameList;

        private void Start()
        {
            AddOnClick();
        }

        private void AddOnClick()
        {
            typeSelectButtonList[0].onClick.AddListener(() => itemPage.SendMessage("ChangeType", typeSelectNameList[0]));
            typeSelectButtonList[1].onClick.AddListener(() => itemPage.SendMessage("ChangeType", typeSelectNameList[1]));
            typeSelectButtonList[2].onClick.AddListener(() => itemPage.SendMessage("ChangeType", typeSelectNameList[2]));
            typeSelectButtonList[3].onClick.AddListener(() => itemPage.SendMessage("ChangeType", typeSelectNameList[3]));
            typeSelectButtonList[4].onClick.AddListener(() => itemPage.SendMessage("ChangeType", typeSelectNameList[4]));
        }
    }
}