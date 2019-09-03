using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AlchemyPlanet.Common
{
    public class TopDownUI : MonoBehaviour
    {
        [SerializeField] private GameObject stateBar;
        [SerializeField] private GameObject bottomMenu;
        [SerializeField] private GameObject sideBar;

        [SerializeField] private GameObject stateBarStartPos;
        [SerializeField] private GameObject stateBarEndPos;
        [SerializeField] private GameObject bottomMenuStartPos;
        [SerializeField] private GameObject bottomMenuEndPos;
        [SerializeField] private GameObject sideBarStartPos;
        [SerializeField] private GameObject sideBarEndPos;

        public void Appear()
        {
            stateBar.transform.position = stateBarStartPos.transform.position;
            bottomMenu.transform.position = bottomMenuStartPos.transform.position;
            sideBar.transform.position = sideBarStartPos.transform.position;

            stateBar.transform.DOMove(stateBarEndPos.transform.position, 1).SetEase(Ease.OutQuint);
            bottomMenu.transform.DOMove(bottomMenuEndPos.transform.position, 1).SetEase(Ease.OutQuint);
            sideBar.transform.DOMove(sideBarEndPos.transform.position, 1).SetEase(Ease.OutQuint);
        }

        public void Disappear()
        {
            stateBar.transform.position = stateBarEndPos.transform.position;
            bottomMenu.transform.position = bottomMenuEndPos.transform.position;
            sideBar.transform.position = sideBarEndPos.transform.position;

            stateBar.transform.DOMove(stateBarStartPos.transform.position, 1).SetEase(Ease.OutQuint);
            bottomMenu.transform.DOMove(bottomMenuStartPos.transform.position, 1).SetEase(Ease.OutQuint);
            sideBar.transform.DOMove(sideBarStartPos.transform.position, 1).SetEase(Ease.OutQuint);
        }
    }
}