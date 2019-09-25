using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class SynthesizeMiniGame : MonoBehaviour
    {
        public float completionTime;
        public int greatProbability;

        [SerializeField]
        private Image miniGameType;
        [SerializeField]
        private GameObject miniGame1Prefab;
        [SerializeField]
        private GameObject miniGame2Prefab;
        [SerializeField]
        private Sprite miniGameTypeSprite1;
        [SerializeField]
        private Sprite miniGameTypeSprite2;
        [SerializeField]
        private Image timer;

        private void Start()
        {
            StartMiniGame1();
            StartCoroutine("MeasureTime");
            switch (Data.DataManager.Instance.itemInfo[SynthesizeManager.Instance.itemName].alchemyRating - Data.DataManager.Instance.CurrentPlayerData.alchemyRating)
            {
                case 0:
                    greatProbability = 4;
                    break;
                case 1:
                    greatProbability = 14;
                    break;
                case 2:
                    greatProbability = 29;
                    break;
                case 3:
                    greatProbability = 49;
                    break;
                default:
                    greatProbability = 74;
                    break;
            }
        }

        public void StartMiniGame1()
        {
            Instantiate(miniGame1Prefab, gameObject.transform);
            miniGameType.sprite = miniGameTypeSprite1;
        }

        public void StartMiniGame2()
        {
            Instantiate(miniGame2Prefab, gameObject.transform);
            miniGameType.sprite = miniGameTypeSprite2;
        }

        public void SendResult()
        {
            StopAllCoroutines();

            if (greatProbability < Random.Range(0, 100))
                SynthesizeManager.Instance.OpenSynthesizeResultUI(SynthesizeManager.Result.great);
            else
                SynthesizeManager.Instance.OpenSynthesizeResultUI(SynthesizeManager.Result.success);

        }

        private IEnumerator MeasureTime()
        {
            completionTime = 0;
            WaitForSeconds second = new WaitForSeconds(0.1f);
            while (completionTime < 30)
            {
                completionTime += 0.1f;
                timer.fillAmount = (30 - completionTime) / 30;
                yield return second;
            }

            SynthesizeManager.Instance.OpenSynthesizeResultUI(SynthesizeManager.Result.fail);
            Destroy(gameObject);
            yield break;
        }
    }
}