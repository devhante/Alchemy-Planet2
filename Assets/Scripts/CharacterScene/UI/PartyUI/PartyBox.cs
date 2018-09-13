using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AlchemyPlanet.CharacterScene
{
    public class PartyBox : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        float originPositionX;
        float clickedPositionX;

        private void Start()
        {
            originPositionX = PartyUI.Instance.partyBoxPosition.transform.position.x;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            clickedPositionX = Input.mousePosition.x;
            StartCoroutine("MoveCoroutine");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            StopCoroutine("MoveCoroutine");

            float distance = Input.mousePosition.x - clickedPositionX;

            if (distance >= 100)
                PartyUI.Instance.OnClickButtonRight();
            else if (distance <= -100)
                PartyUI.Instance.OnClickButtonLeft();
            else
                transform.position = new Vector3(originPositionX, transform.position.y, transform.position.z);
        }

        IEnumerator MoveCoroutine()
        {
            while(true)
            {
                float posX = originPositionX + (Input.mousePosition.x - clickedPositionX);
                transform.position = new Vector3(posX, transform.position.y, transform.position.z);
                yield return null;
            }
        }
    }
}
