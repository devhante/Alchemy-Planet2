using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AdvectureScene
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

        private Planet planet;
        private Ground ground;

        private void OnDestroy() { Instance = null; }

        private void Awake() { Instance = this; }

        private void Start()
        {
            planet = CreateNewPlanet(AdventureManager.Instance.planetNumber).GetComponent<Planet>();
            ground = CreateNewGround(AdventureManager.Instance.planetNumber, 0).GetComponent<Ground>();
        }

        private void MoveLeft()
        {
            Planet newPlanet = CreateNewPlanet(AdventureManager.Instance.planetNumber).GetComponent<Planet>();
            planet.MoveOut(false);
            newPlanet.MoveIn(false);
            planet = newPlanet;

            Ground newGround = CreateNewGround(AdventureManager.Instance.planetNumber, Ground.ANGLE).GetComponent<Ground>();
            ground.MoveRight();
            // ground destroy 해야 함
            newGround.MoveRight();
            ground = newGround;
        }

        private void MoveRight()
        {
            Planet newPlanet = CreateNewPlanet(AdventureManager.Instance.planetNumber).GetComponent<Planet>();
            planet.MoveOut(true);
            newPlanet.MoveIn(true);
            planet = newPlanet;

            Ground newGround = CreateNewGround(AdventureManager.Instance.planetNumber, -Ground.ANGLE).GetComponent<Ground>();
            ground.MoveLeft();
            // ground destroy 해야 함
            newGround.MoveLeft();
            ground = newGround;
        }

        private GameObject CreateNewPlanet(int planetNumber)
        {
            GameObject obj = Instantiate(planetPrefab, transform);
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