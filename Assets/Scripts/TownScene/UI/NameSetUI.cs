using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;
using System.Collections;
using BackEnd;

namespace AlchemyPlanet.TownScene
{
    public class NameSetUI : Common.UI<NameSetUI>
    {
        [SerializeField] private InputField PlayerNameInput;
        [SerializeField] private Button CommitButton;

        protected override void Awake()
        {
            base.Awake();
            CommitButton.onClick.AddListener(() =>
            {
                CreatePlayer();
                UIManager.Instance.CloseMenu();
            });
        }

        private void Start()
        {
            StartCoroutine(CheckName());
        }

        public IEnumerator CheckName()
        {
            byte[] stringByte;
            while (true)
            {
                stringByte = System.Text.Encoding.Default.GetBytes(PlayerNameInput.text);
                if (stringByte.Length <= 16 && stringByte.Length >= 2)
                {
                    CommitButton.image.color = new Color32(255, 255, 255, 255);
                }
                else
                {
                    CommitButton.image.color = new Color32(200, 200, 200, 255);
                }
                yield return new WaitForSeconds(1);
            }
        }

        public void CreatePlayer()
        {
            byte[] stringByte = System.Text.Encoding.Default.GetBytes(PlayerNameInput.text);

            if (stringByte.Length <= 16 && stringByte.Length >= 2)
            {
                StopCoroutine(CheckName());

                DataManager.Instance.CurrentPlayerData.player_id = DataManager.Instance.CreatePlayerId();
                DataManager.Instance.CurrentPlayerData.player_name = PlayerNameInput.text;
                DataManager.Instance.CurrentPlayerData.unicoin = 0;
                DataManager.Instance.CurrentPlayerData.cosmostone = 0;
                DataManager.Instance.CurrentPlayerData.oxygentank = 0;
                BackendManager.Instance.CreatePlayer(DataManager.Instance.CurrentPlayerData.player_id, DataManager.Instance.CurrentPlayerData.player_name);

                BackendManager.Instance.CreateItem(DataManager.Instance.CurrentPlayerData.player_id);
                BackendManager.Instance.CreateTown(DataManager.Instance.CurrentPlayerData.player_id);
                BackendManager.Instance.CreateCharacter(DataManager.Instance.CurrentPlayerData.player_id);

                BackendManager.Instance.CreateParty(DataManager.Instance.CurrentPlayerData.player_id);
                DataManager.Instance.CurrentPlayerData.party[0, 0] = CharacterEnum.Popin;
                BackendManager.Instance.AddParty(BackendManager.Instance.GetInDate("party"), 1, "0", "", "");

                BackendManager.Instance.CreateStage(DataManager.Instance.CurrentPlayerData.player_id);
                DataManager.Instance.CurrentPlayerData.stroystar["1-1"] = 0;
                BackendManager.Instance.AddStage(BackendManager.Instance.GetInDate("stage"), "1-1", 0);

                BackendManager.Instance.CreateRequest(DataManager.Instance.CurrentPlayerData.player_id);
            }
        }

    }

}