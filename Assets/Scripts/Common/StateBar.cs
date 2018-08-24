using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.Common
{
    public class StateBar : MonoBehaviour
    {
        [SerializeField] private Text Unicoin;
        [SerializeField] private Text Cosmoston;
        [SerializeField] private Text OxygenTank;

        private void Awake()
        {
            Unicoin.text = Data.DataManager.Instance.CurrentPlayerData.unicoin.ToString();
            Cosmoston.text = Data.DataManager.Instance.CurrentPlayerData.cosmoston.ToString();
            OxygenTank.text =
               Data.DataManager.Instance.CurrentPlayerData.oxygentank.ToString() + "/" +
               Data.DataManager.Instance.CurrentPlayerData.Max_oxygentank.ToString();
        }

        public void UpdateState()
        {

        }
    }
}