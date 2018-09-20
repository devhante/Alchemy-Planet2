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
        public int PartyIndex { get; set; }

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
            PartyIndex = 1;
            WebSocketManager.Instance.SendFindParties("0", "0");
        }

        public void InitParty(CollectionParty[] data)
        {
            foreach (var item in data)
                Parties[item.partyIndex - 1, item.slotIndex - 1] = (CharacterEnum)int.Parse(item.characterId);

            CurrentCharacters = InstantiateCharacters();
        }

        public Characters InstantiateCharacters()
        {
            var temp = Instantiate(characters).GetComponent<Characters>();
            temp.ChangeCharacter(Parties[PartyIndex - 1, 0], Parties[PartyIndex - 1, 1], Parties[PartyIndex - 1, 2]);
            return temp;
        }
    }   
}