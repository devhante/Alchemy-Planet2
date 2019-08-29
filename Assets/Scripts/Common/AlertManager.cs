using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace AlchemyPlanet.Common
{
    public class AlertManager : MonoBehaviour
    {
        public static AlertManager Instance { get; private set; }

        public GameObject alert;
        public GameObject canvas;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void InstantiateAlert(string text)
        {
            GameObject canvasObject = Instantiate(canvas);
            GameObject alertObject = Instantiate(alert, canvasObject.transform);
            alertObject.transform.GetChild(1).GetComponent<Text>().text = text;
            alertObject.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => DestroyObject(alertObject.transform.gameObject));

            alertObject.transform.localScale = new Vector3(0f, 0f, 0f);
            alertObject.transform.DOScale(1.0f, 0.5f).SetEase(Ease.OutQuint);
        }

        private void DestroyObject(GameObject obj)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(obj.transform.DOScale(0f, 0.5f).SetEase(Ease.OutQuint));
            seq.OnComplete(() => Destroy(obj));
            seq.Play();
        }

        public Button InstantiateAlertWithReturn(string text)
        {
            Button result;

            GameObject canvasObject = Instantiate(canvas);
            GameObject alertObject = Instantiate(alert, canvasObject.transform);
            alertObject.transform.GetChild(1).GetComponent<Text>().text = text;
            result = alertObject.transform.GetChild(2).GetComponent<Button>();
            result.onClick.AddListener(() => Destroy(alertObject.transform.gameObject));

            return result;
        }
    }
}