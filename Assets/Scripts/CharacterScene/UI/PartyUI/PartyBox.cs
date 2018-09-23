using AlchemyPlanet.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AlchemyPlanet.CharacterScene
{
    public class PartyBox : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Image[] slotImages;
        public Text slotTitle;

        float originPositionX;
        float clickedPositionX;

        private void Start()
        {
            LoadSlot();
            originPositionX = PartyUI.Instance.partyBoxPosition.transform.position.x;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (PartyUI.Instance.buttonLeft.gameObject.activeSelf == true)
            {
                clickedPositionX = Input.mousePosition.x;
                StartCoroutine("MoveCoroutine");
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (PartyUI.Instance.buttonLeft.gameObject.activeSelf == true)
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

        private void LoadSlot()
        {
            for(int i = 0; i < 3; i++)
            {
                CharacterEnum character = DataManager.Instance.CurrentPlayerData.party[GameManager.Instance.PartyIndex - 1, i];
                if (character != 0)
                {
                    slotImages[i].enabled = true;
                    slotImages[i].sprite = GameManager.Instance.characterProfiles[(int)character - 1];
                }
                else
                    slotImages[i].enabled = false;
            }

            slotTitle.text = "파티" + GameManager.Instance.PartyIndex;
        }
    }
}
