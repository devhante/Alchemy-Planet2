using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;
using DG.Tweening;

namespace AlchemyPlanet.PrologueScene
{
    public class PrologueScript : MonoBehaviour
    {
        public static PrologueScript Instance;

        [SerializeField] private MainCamera mainCamera;

        [SerializeField] private GameObject moveTutorial;
        [SerializeField] private GameObject NPCTutorial;

        [SerializeField] private Image SmarteenApp;
        [SerializeField] private Image TeamFoxin;
        [SerializeField] private Image AlchemyPlanet;
        [SerializeField] private Image Back;

        public bool isOnNPCPos = false;
        public bool isEnd = false;

        void Start()
        {
            Instance = this;
            StartCoroutine(LateStart(1));
        }

        private IEnumerator LateStart(int seconds)
        {
            //첫 대사
            yield return new WaitForSeconds(seconds);
            DataManager.Instance.selected_dialog = new NPCDAta("Prologue_00");
            TownScene.UIManager.Instance.OpenMenu<TownScene.DialogUI>();
            TownScene.DialogUI.Instance.SetDialog();

            while (TownScene.DialogUI.Instance != null)
            {
                yield return new WaitForEndOfFrame();
            }


            //카메라 이동
            mainCamera.FadeIn();

            mainCamera.gameObject.transform.DOMoveY(4.4f, 3).OnComplete(() => {
                mainCamera.SetOffset();
            });
            yield return new WaitForSeconds(3);
            
            //다음대사
            DataManager.Instance.selected_dialog = new NPCDAta("Prologue_01");
            TownScene.UIManager.Instance.OpenMenu<TownScene.DialogUI>();
            TownScene.DialogUI.Instance.SetDialog();

            while (TownScene.DialogUI.Instance != null)
            {
                yield return new WaitForEndOfFrame();
            }

            //이동 튜토리얼을 연다.
            moveTutorial.SetActive(true);

            while (moveTutorial != null)
            {
                yield return new WaitForEndOfFrame();
            }

            mainCamera.gameObject.transform.DOMoveY(2.4f, 3).OnComplete(() => {
                mainCamera.SetOffset();
            });

            //다음 스테이지의 NPC 옆에 도착한다.
            while (! isOnNPCPos)
            {
                yield return new WaitForEndOfFrame();
            }
            //NPC 튜토리얼을 연다
            NPCTutorial.SetActive(true);

            //이동을 끝낸다
            while (! isEnd)
            {
                yield return new WaitForEndOfFrame();
            }
            
            mainCamera.FadeOut();
            yield return new WaitForSeconds(2);

            SmarteenApp.gameObject.SetActive(true);
            SmarteenApp.DOFade(1, 1.5f);
            SmarteenApp.DOFade(0, 1.5f);
            yield return new WaitForSeconds(3);
            SmarteenApp.gameObject.SetActive(false);

            TeamFoxin.gameObject.SetActive(true);
            TeamFoxin.DOFade(1, 1.5f);
            SmarteenApp.DOFade(0, 1.5f);
            yield return new WaitForSeconds(3);
            TeamFoxin.gameObject.SetActive(false);

            AlchemyPlanet.gameObject.SetActive(true);
            AlchemyPlanet.DOFade(1, 1.5f);
            SmarteenApp.DOFade(0, 1.5f);
            yield return new WaitForSeconds(3);
            AlchemyPlanet.gameObject.SetActive(false);

            yield return new WaitForSeconds(3);

            Back.gameObject.SetActive(true);
            mainCamera.FadeIn();

            DataManager.Instance.selected_dialog = new NPCDAta("Prologue_Bell");
            TownScene.UIManager.Instance.OpenMenu<TownScene.DialogUI>();
            TownScene.DialogUI.Instance.SetDialog();

            while (TownScene.DialogUI.Instance != null)
            {
                yield return new WaitForEndOfFrame();
            }

            mainCamera.FadeOut();
            yield return new WaitForSeconds(3);

            SceneChangeManager.Instance.ChangeSceneWithLoading("TownScene");
        }
    }
}