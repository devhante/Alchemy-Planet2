using AlchemyPlanet.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.CharacterScene
{
    public class Characters : MonoBehaviour
    {
        private GameObject[] Slots { get; set; }

        private void Awake()
        {
            Slots = new GameObject[3];

            for (int i = 0; i < Slots.Length; i++)
                Slots[i] = transform.GetChild(i).gameObject;
        }

        public void ChangeCharacter(CharacterEnum character1, CharacterEnum character2, CharacterEnum character3)
        {
            if (character1 != 0)
                Instantiate(GameManager.Instance.characterPrefabs[(int)character1 - 1], Slots[0].transform);
            if (character2 != 0)
                Instantiate(GameManager.Instance.characterPrefabs[(int)character2 - 1], Slots[1].transform);
            if (character3 != 0)
                Instantiate(GameManager.Instance.characterPrefabs[(int)character3 - 1], Slots[2].transform);
        }

        public void MoveLeft()
        {
            StartCoroutine("MoveLeftCoroutine");
        }

        public void MoveRight()
        {
            StartCoroutine("MoveRightCoroutine");
        }

        IEnumerator MoveLeftCoroutine()
        {
            float speed = 10;
            float originPosX = transform.position.x;

            while (originPosX - transform.position.x <= 5)
            {
                transform.position += Vector3.left * Time.deltaTime * speed;
                yield return null;
            }

            transform.position = new Vector3(originPosX - 5, transform.position.y, transform.position.z);
 ;        }

        IEnumerator MoveRightCoroutine()
        {
            float speed = 10;
            float originPosX = transform.position.x;

            while (originPosX - transform.position.x >= -5)
            {
                transform.position += Vector3.right * Time.deltaTime * speed;
                yield return null;
            }

            transform.position = new Vector3(originPosX + 5, transform.position.y, transform.position.z);
        }
    }
}