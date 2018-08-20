using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.TownScene
{
    public class NewPlayerUI : Common.UI
    {
        [SerializeField] private InputField PlayerNameInput;
        [SerializeField] private Button CommitButton;
        private void Awake()
        {
            CommitButton.onClick.AddListener(() => {
                Data.DataManager.Instance.CurrentPlayerData.player_name = PlayerNameInput.text;
            });
        }
    }

}