using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.TownScene {
    public class TownUI : MonoBehaviour {

        [SerializeField] private Button buildingbutton;
        [SerializeField] private GameObject buildBar;

        private void Awake()
        {
            buildingbutton.onClick.AddListener(() => {
                StartCoroutine("MoveBar");
            });
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        IEnumerator MoveBar()
        {
            while (buildBar.transform.position.x > 300)
            {
                buildBar.transform.Translate(Vector2.left * 1200 * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}