using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.TownScene
{
    public class NameSetUI : Common.UI<NameSetUI>
    {
        [SerializeField] private InputField PlayerNameInput;
        [SerializeField] private Button CommitButton;

        protected override void Awake()
        {
            base.Awake();
            CommitButton.onClick.AddListener(() => {
                DataManager.Instance.CurrentPlayerData.player_name = PlayerNameInput.text;
                WebSocketManager.Instance.SendInsertName("0", DataManager.Instance.CurrentPlayerData.player_id,
                    DataManager.Instance.CurrentPlayerData.player_name);

                DataManager.Instance.CurrentPlayerData.party[0, 0] = CharacterEnum.Popin;
                WebSocketManager.Instance.SendInsertParty("0", DataManager.Instance.CurrentPlayerData.player_id, 1, 1, "0");

                DataManager.Instance.CurrentPlayerData.stroystar["1-1"] = 3;
                WebSocketManager.Instance.SendInsertStoryStar("0", DataManager.Instance.CurrentPlayerData.player_id, "1-1", 0);

                UIManager.Instance.CloseMenu();
            });
        }

    }

}