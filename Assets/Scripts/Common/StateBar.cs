using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.Common
{
    public class StateBar : MonoBehaviour
    {
        public static StateBar Instance;

        [SerializeField] private Text playerName;
        [SerializeField] private Text playerLevel;
        [SerializeField] private Image Expbar;

        [SerializeField] private Text Unicoin;
        [SerializeField] private Text Cosmoston;
        [SerializeField] private Text OxygenTank;

        private void Awake()
        {
            Instance = this;
            StartCoroutine(LateAwake());
        }

        private IEnumerator LateAwake()
        {
            while(!Data.DataManager.Instance)
            {
                yield return new WaitForEndOfFrame();
            }
            UpdateState();
        }

        public void UpdateState()
        {
            playerName.text = Data.DataManager.Instance.CurrentPlayerData.player_name.ToString();
            playerLevel.text = Data.DataManager.Instance.CurrentPlayerData.level.ToString();

            Expbar.fillAmount = Data.DataManager.Instance.CurrentPlayerData.exp / System.Int32.Parse(playerLevel.text);

            Unicoin.text = Data.DataManager.Instance.CurrentPlayerData.unicoin.ToString();
            Cosmoston.text = Data.DataManager.Instance.CurrentPlayerData.cosmostone.ToString();
            OxygenTank.text =
               Data.DataManager.Instance.CurrentPlayerData.oxygentank.ToString() + "/" +
               Data.DataManager.Instance.CurrentPlayerData.Max_oxygentank.ToString();
        }
    }
}