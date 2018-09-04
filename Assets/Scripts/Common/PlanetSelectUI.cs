using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace AlchemyPlanet.Common {
    public class PlanetSelectUI : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;
        [SerializeField] private GameObject Planets;
        [SerializeField] private GameObject AdventurePlanetGroup;

        [SerializeField] private Image TextImage;

        private bool played;
        void Start()
        {
            mainCam.transform.DOMoveX(5,1).SetEase(Ease.OutQuart);
            AdventurePlanetGroup.transform.position = new Vector2(Planets.transform.position.x, Screen.height * 1.5f);
            Planets.transform.DOMoveX(Screen.width / 2, 1).SetEase(Ease.OutQuart);
        }

        public void MoveDown()
        {
            if (!played)
            {
                played = true;
                mainCam.transform.DOMoveY(2, 1).SetEase(Ease.OutQuart);
                Planets.transform.DOMoveY(-Screen.height / 2, 1).SetEase(Ease.OutQuart);

                TextImage.DOColor(new Color32(255, 255, 255, 255), 1).SetEase(Ease.InBack);
            }
        }

        public void ChangeSelectedStage(int stage_no)
        {
            Data.DataManager.Instance.selected_stage = stage_no;
        }
    }

}