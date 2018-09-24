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

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public GameObject InstantiateAlert(Transform parent, string text)
        {
            GameObject instance = Instantiate(alert, parent);
            instance.transform.GetChild(1).GetComponent<Text>().text = text;
            instance.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => Destroy(instance.transform.gameObject));
            return instance;
        }
    }
}