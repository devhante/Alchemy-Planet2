using AlchemyPlanet.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AlchemyPlanet.StoryLobbyScene
{
    public class StoryPartyBox : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Image[] slotImages;
        public Text slotTitle;

        float originPositionX;
        float clickedPositionX;

        private void Start()
        {
            LoadSlot();
            originPositionX = StoryPartyUI.Instance.partyBoxPosition.transform.position.x;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (StoryPartyUI.Instance.buttonLeft.gameObject.activeSelf == true)
            {
                clickedPositionX = Input.mousePosition.x;
                StartCoroutine(MoveCoroutine());
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (StoryPartyUI.Instance.buttonLeft.gameObject.activeSelf == true)
            {
                StopCoroutine(MoveCoroutine());

                float distance = Input.mousePosition.x - clickedPositionX;

                if (distance >= 100)
                    StoryPartyUI.Instance.OnClickButtonLeft();
                else if (distance <= -100)
                    StoryPartyUI.Instance.OnClickButtonRight();
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
                CharacterEnum character = DataManager.Instance.CurrentPlayerData.party[StoryPartyUI.Instance.PartyIndex - 1, i];
                if (character != 0)
                {
                    slotImages[i].enabled = true;
                    slotImages[i].sprite = StoryPartyUI.Instance.characterProfiles[(int)character - 1];
                }
                else
                    slotImages[i].enabled = false;
            }

            slotTitle.text = "파티" + StoryPartyUI.Instance.PartyIndex;
        }
    }
}
