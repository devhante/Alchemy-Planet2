using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace AlchemyPlanet.CharacterScene
{
    public class PartyUI : MonoBehaviour
    {
        public Button buttonLeft;
        public Button buttonRight;
        public GameObject partyBoxPosition;
        public GameObject partyBoxPrefab;

        private GameObject partyBoxObject;
        private bool isSequencePlaying;

        private void Awake()
        {
            partyBoxObject = Instantiate(partyBoxPrefab, partyBoxPosition.transform.position, Quaternion.identity, transform);
            buttonLeft.onClick.AddListener(OnClickButtonLeft);
            buttonRight.onClick.AddListener(OnClickButtonRight);
        }

        private void OnClickButtonLeft()
        {
            buttonLeft.gameObject.SetActive(false);
            buttonRight.gameObject.SetActive(false);

            Sequence sq1 = DOTween.Sequence();
            sq1.Append(partyBoxObject.transform.DOMoveX(partyBoxObject.transform.position.x - 700, 0.5f)
                .SetEase(Ease.OutQuint));

            GameObject newObject = Instantiate(partyBoxPrefab, partyBoxPosition.transform.position + new Vector3(700, 0, 0), Quaternion.identity, transform);
            Sequence sq2 = DOTween.Sequence();
            sq2.Append(newObject.transform.DOMoveX(newObject.transform.position.x - 700, 0.5f)
                .SetEase(Ease.OutQuint)
                .OnComplete(() => {
                    Destroy(partyBoxObject);
                    partyBoxObject = newObject;
                    buttonLeft.gameObject.SetActive(true);
                    buttonRight.gameObject.SetActive(true);
                }));
        }

        private void OnClickButtonRight()
        {
            buttonLeft.gameObject.SetActive(false);
            buttonRight.gameObject.SetActive(false);

            Sequence sq = DOTween.Sequence();
            sq.Append(partyBoxObject.transform.DOMoveX(partyBoxObject.transform.position.x + 700, 0.5f)
                .SetEase(Ease.OutQuint));

            GameObject newObject = Instantiate(partyBoxPrefab, partyBoxPosition.transform.position + new Vector3(-700, 0, 0), Quaternion.identity, transform);
            Sequence sq2 = DOTween.Sequence();
            sq2.Append(newObject.transform.DOMoveX(newObject.transform.position.x + 700, 0.5f)
                .SetEase(Ease.OutQuint)
                .OnComplete(() => {
                    Destroy(partyBoxObject);
                    partyBoxObject = newObject;
                    buttonLeft.gameObject.SetActive(true);
                    buttonRight.gameObject.SetActive(true);
                }));
        }
    }
}
