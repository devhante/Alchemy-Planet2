using UnityEngine;
using DG.Tweening;

namespace AlchemyPlanet.Common {
    public class PlanetSelectUI : MonoBehaviour
    {
        [SerializeField] private GameObject Planets;
        void Start()
        {
            Planets.transform.DOMoveX(Screen.width / 2, 1).SetEase(Ease.InSine);
        }

        public void MoveDown()
        {
            Planets.transform.DOMoveY(-Screen.height / 2, 1).SetEase(Ease.InSine);
        }

        public void ChangeSelectedStage(int stage_no)
        {
            Data.DataManager.Instance.selected_stage = stage_no;
        }
    }

}