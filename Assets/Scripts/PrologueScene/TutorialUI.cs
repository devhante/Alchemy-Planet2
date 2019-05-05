using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace AlchemyPlanet.PrologueScene
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] private Image message;
        [SerializeField] private Image back;

        [SerializeField] private Image finger;
        [SerializeField] private float finger_pos;

        [SerializeField] private Button button;

        private void Awake()
        {
            
        }

        void Start()
        {
            back.DOFade(1, 1).SetEase(Ease.InSine);
            message.DOFade(1, 1).SetEase(Ease.InBack);
            finger.DOFade(1, 1).SetEase(Ease.InSine).OnComplete(() => {
                finger.transform.DORotate(new Vector3(0, 0, 30), 1);
                finger.transform.DOLocalMove(new Vector3(finger_pos, -330, 0), 1).OnComplete(() =>
                {
                    button.onClick.AddListener(() => {
                        GameObject.Destroy(this.gameObject);
                    });
                });
            });
        }
    }
}