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

        public void SetOffset()
        {
            offset = this.transform.position - player.transform.position;
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

        public void ZoomIn(float zoom)
        {
            StartCoroutine(ZoomInCoroutine(zoom));
        }

        IEnumerator Follow()
        {
            while (true)
            {
                this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, player.transform.position + offset, 0.2f);
                yield return frame;
            }
        }

        IEnumerator ZoomInCoroutine(float zoom)
        {
            while (Camera.main.orthographicSize > zoom)
            {
                Camera.main.orthographicSize -= 0.1f;
                yield return frame;
            }
        }
    }
}