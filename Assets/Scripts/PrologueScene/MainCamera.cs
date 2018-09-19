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

        private Vector3 offset;

        private WaitForEndOfFrame frame = new WaitForEndOfFrame();

        private void Awake()
        {
            offset = this.transform.position - player.transform.position;
        }

        private void Start()
        {
            StartCoroutine(Follow());
        }

        public void FadeIn()
        {
            fade.DOFade(0, 2).SetEase(Ease.InBack).OnComplete(() => fade.gameObject.SetActive(false));
        }

        public void FadeOut()
        {
            fade.gameObject.SetActive(true);
            fade.DOFade(1, 2).SetEase(Ease.InBack);
        }

        IEnumerator Follow()
        {
            while (true)
            {
                this.gameObject.transform.position = player.transform.position + offset;
                yield return frame;
            }
        }
    }
}