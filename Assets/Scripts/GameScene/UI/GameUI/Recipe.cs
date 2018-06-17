using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Recipe : MonoBehaviour
    {
        public string recipeName;

        static int speed = 3;
        Vector3 destination;

        private void Awake()
        {
            transform.position = new Vector2(760, 732);
            destination = new Vector3(36, 732);
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * speed);
        }

        public void SetDestination(int index)
        {
            destination = new Vector3(36.0f + index * 50.0f, destination.y);
        }
    }
}