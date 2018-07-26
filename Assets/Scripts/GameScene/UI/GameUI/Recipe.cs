using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Recipe : MonoBehaviour
    {
        public MaterialName recipeName;

        static int speed = 3;
        Vector3 destination;
        RectTransform rt;

        private void Awake()
        {
            rt = GetComponent<RectTransform>();

            rt.position = RecipeManager.Instance.startPoint.position;
            destination = RecipeManager.Instance.endPoint.position;
        }

        private void Update()
        {
            rt.position = Vector3.Lerp(rt.position, destination, Time.deltaTime * speed);
        }

        public void SetDestination(int index)
        {
            destination = RecipeManager.Instance.endPoint.position + new Vector3(index * (Screen.width / 14.5f), 0, 0);
        }
    }
}