using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace AlchemyPlanet.PrologueScene
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private Image fade;

        private void Start()
        {
            fade.DOFade(0, 2).SetEase(Ease.InBack).OnComplete(() => fade.gameObject.SetActive(false));
        }

        void Update()
        {

        }
    }
}