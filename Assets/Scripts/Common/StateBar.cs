using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.Common
{
    public class StateBar : MonoBehaviour
    {
        public static StateBar Instance;

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
            Unicoin.text = Data.DataManager.Instance.CurrentPlayerData.unicoin.ToString();
            Cosmoston.text = Data.DataManager.Instance.CurrentPlayerData.cosmoston.ToString();
            OxygenTank.text =
               Data.DataManager.Instance.CurrentPlayerData.oxygentank.ToString() + "/" +
               Data.DataManager.Instance.CurrentPlayerData.Max_oxygentank.ToString();
        }
    }
}