using AlchemyPlanet.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.StoryLobbyScene
{
    public class PartyBoxStory : MonoBehaviour
    {
        public Sprite[] characterProfiles;
        public Image[] slotImages;
        public Text slotTitle;

        private int partyIndex = 1;

        private void Start()
        {
            LoadSlot();
        }

        private void LoadSlot()
        {
            for (int i = 0; i < 3; i++)
            {
                CharacterEnum character = DataManager.Instance.CurrentPlayerData.party[partyIndex - 1, i];
                if (character != 0)
                {
                    slotImages[i].enabled = true;
                    slotImages[i].sprite = characterProfiles[(int)character - 1];
                }
                else
                    slotImages[i].enabled = false;
            }

            slotTitle.text = "파티" + partyIndex;
        }
    }
}