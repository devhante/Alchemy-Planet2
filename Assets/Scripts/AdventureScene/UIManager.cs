using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace AlchemyPlanet.AdventureScene
{
    public class UIManager : MonoBehaviour
    {
        [System.Serializable]
        public struct PlanetCurveVertex
        {
            public Transform front;
            public Transform back;
            public Transform left1;
            public Transform left2;
            public Transform right1;
            public Transform right2;
        };

        public static UIManager Instance { get; private set; }

        public Sprite[] planetSprites;
        public Sprite[] groundSprites;
        public GameObject planetPrefab;
        public GameObject groundPrefab;
        public PlanetCurveVertex vertex;
        public Transform planetParent;
        public Button centerButton;
        public Image planetName;
        public Button leftButton;
        public Button rightButton;

        private Planet planet;
        private Ground ground;

        private void OnDestroy() { Instance = null; }

        private void Awake() { Instance = this; }

        private void Start()
        {
            planet = CreateNewPlanet(AdventureManager.Instance.planetNumber).GetComponent<Planet>();
            ground = CreateNewGround(AdventureManager.Instance.planetNumber, 0).GetComponent<Ground>();
            leftButton.onClick.AddListener(OnClickLeftButton);
            rightButton.onClick.AddListener(OnClickRightButton);
        }

        private void OnClickLeftButton()
        {
            AdventureManager.Instance.MoveLeft();
        }

        private void OnClickRightButton()
        {
            AdventureManager.Instance.MoveRight();
        }

        public void MoveLeft()
        {
            PlayCenterUICoroutine();

            Planet newPlanet = CreateNewPlanet(AdventureManager.Instance.planetNumber).GetComponent<Planet>();
            planet.MoveOut(false);
            newPlanet.MoveIn(false);
            planet = newPlanet;

            Ground newGround = CreateNewGround(AdventureManager.Instance.planetNumber, Ground.ANGLE).GetComponent<Ground>();
            ground.MoveRight();
            newGround.MoveRight();
            ground = newGround;
        }

        public void MoveRight()
        {
            PlayCenterUICoroutine();

            Planet newPlanet = CreateNewPlanet(AdventureManager.Instance.planetNumber).GetComponent<Planet>();
            planet.MoveOut(true);
            newPlanet.MoveIn(true);
            planet = newPlanet;

            Ground newGround = CreateNewGround(AdventureManager.Instance.planetNumber, -Ground.ANGLE).GetComponent<Ground>();
            ground.MoveLeft();
            newGround.MoveLeft();
            ground = newGround;
        }

        private void PlayCenterUICoroutine()
        {
            StartCoroutine(CenterButtonCoroutine());
            StartCoroutine(PlanetNameCoroutine());
        }

        private IEnumerator CenterButtonCoroutine()
        {
            const float disappearRatio = 0.3f;
            const float waitRatio = 0.2f;
            const float appearRatio = 0.5f;

            while (true)
            {
                float t = AdventureManager.Instance.progress;

                if (t < disappearRatio)
                {
                    float scale = t / disappearRatio;
                    scale = 1 - (Mathf.Pow(scale - 1, 5) + 1);
                    centerButton.transform.localScale = new Vector3(scale, scale);
                }
                else if (t >= disappearRatio + waitRatio)
                {
                    float scale = (t - (disappearRatio + waitRatio)) / appearRatio;
                    scale = Mathf.Pow(scale - 1, 5) + 1;
                    centerButton.transform.localScale = new Vector3(scale, scale);
                }

                if (AdventureManager.Instance.progress == 1) break;

                yield return null;
            }
        }

        private IEnumerator PlanetNameCoroutine()
        {
            const float disappearRatio = 0.3f;
            const float waitRatio = 0.2f;
            const float appearRatio = 0.5f;

            while (true)
            {
                float t = AdventureManager.Instance.progress;

                if (t < disappearRatio)
                {
                    float a = t / disappearRatio;
                    a = 1 - (Mathf.Pow(a - 1, 5) + 1);
                    planetName.color = new Color(255, 255, 255, a);
                }
                else if(t >= disappearRatio + waitRatio)
                {
                    float a = (t - (disappearRatio + waitRatio)) / appearRatio;
                    a = Mathf.Pow(a - 1, 5) + 1;
                    planetName.color = new Color(255, 255, 255, a);
                }

                if (AdventureManager.Instance.progress == 1) break;

                yield return null;
            }
        }

        private GameObject CreateNewPlanet(int planetNumber)
        {
            GameObject obj = Instantiate(planetPrefab, planetParent);
            obj.GetComponent<Image>().sprite = planetSprites[planetNumber];
            obj.GetComponent<Planet>().vertex = vertex;

            return obj;
        }

        private GameObject CreateNewGround(int planetNumber, float angle)
        {
            GameObject obj = Instantiate(groundPrefab, transform);
            obj.transform.GetChild(0).GetComponent<Image>().sprite = groundSprites[planetNumber];
            obj.transform.rotation = Quaternion.Euler(0, 0, angle);
            return obj;
        }
    }
}