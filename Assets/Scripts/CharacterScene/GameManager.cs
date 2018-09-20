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

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            PartyIndex = 1;
        }

        public Characters InstantiateCharacters()
        {
            var temp = Instantiate(characters).GetComponent<Characters>();
            temp.ChangeCharacter(DataManager.Instance.CurrentPlayerData.party[PartyIndex - 1, 0], DataManager.Instance.CurrentPlayerData.party[PartyIndex - 1, 1], DataManager.Instance.CurrentPlayerData.party[PartyIndex - 1, 2]);
            return temp;
        }
    }   
}