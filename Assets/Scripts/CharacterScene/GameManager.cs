using AlchemyPlanet.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.CharacterScene
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; set; }

        public GameObject characters;
        public GameObject[] characterPrefabs;
        public Sprite[] chracterProfiles;

        public Characters CurrentCharacters { get; set; }

        private int partyIndex = 1;

        [HideInInspector]
        public CharacterEnum[,] Parties { get; set; } 

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            Parties = new CharacterEnum[9, 3];
            WebSocketManager.Instance.SendFindPlayerParties("0", "0");
        }

        public void GetPlayerParty(PlayerParty[] data)
        {
            foreach (var item in data)
                Parties[item.partyIndex - 1, item.slotIndex - 1] = (CharacterEnum)int.Parse(item.characterId);

            CurrentCharacters = Instantiate(characters).GetComponent<Characters>();
            CurrentCharacters.ChangeCharacter(Parties[partyIndex - 1, 0], Parties[partyIndex - 1, 1], Parties[partyIndex - 1, 2]);
        }
    }   
}