using AlchemyPlanet.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.CharacterScene
{
    public class OrganizeUI : MonoBehaviour
    {
        public static OrganizeUI Instance { get; private set; }

        public Button leftButton;
        public Button rightButton;
        public PageLights partyBoxPageLights;
        public PageLights characterListPageLights;
        public Button okayButton;
        public Button backButton;
        public GameObject characterListCells;
        public GameObject characterListCellImage;
        public List<CharacterEnum> characterSpritesKey;
        public List<Sprite> characterSpritesValue;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            leftButton.onClick.AddListener(OnClickLeftButton);
            rightButton.onClick.AddListener(OnClickRightButton);
            okayButton.onClick.AddListener(OnClickOkayButton);
            backButton.onClick.AddListener(OnClickBackButton);

            LoadCharacterSprite(0);
        }

        private void OnClickLeftButton()
        {
            partyBoxPageLights.ChangeLightToLeft();
        }

        private void OnClickRightButton()
        {
            partyBoxPageLights.ChangeLightToRight();
        }

        private void OnClickOkayButton()
        {
            WebSocketManager.Instance.SendFindPlayerParties("0", "0");

            UIManager.Instance.DestroyUI();
            UIManager.Instance.CreateMainUI();
        }

        private void OnClickBackButton()
        {
            WebSocketManager.Instance.SendFindPlayerParties("0", "0");

            UIManager.Instance.DestroyUI();
            UIManager.Instance.CreateMainUI();
        }

        private void LoadCharacterSprite(int page)
        {
            int startIndex = page * 15;
            int endIndex = startIndex + 14;
            endIndex = Mathf.Min(endIndex, DataManager.Instance.CurrentPlayerData.characters.Count);

            for(int i = startIndex; i < endIndex; i++)
            {
                GameObject go = Instantiate(characterListCellImage, characterListCells.transform.GetChild(i));
                go.GetComponent<Image>().sprite = GetCharacterSprite(DataManager.Instance.CurrentPlayerData.characters[i].name);
            }
        }

        private Sprite GetCharacterSprite(CharacterEnum c)
        {
            int index = 0;

            for(int i = 0; i < characterSpritesKey.Count; i++)
            {
                if (characterSpritesKey[i] == c)
                {
                    index = i;
                    break;
                }
            }

            return characterSpritesValue[index];
        }
    }
}