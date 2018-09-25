using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            alertObject.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => Destroy(alertObject.transform.gameObject));
        }
    }
}