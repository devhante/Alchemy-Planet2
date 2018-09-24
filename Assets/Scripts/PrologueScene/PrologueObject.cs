using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AlchemyPlanet.PrologueScene
{
    public enum ObjectKind { NPC, Object, Door, Zoom, Switch}
    public enum ObjectSwitch { NPCTutorial, End }

    public class PrologueObject : MonoBehaviour
    {
        [SerializeField] private string objcect_name;
        [SerializeField] private ObjectKind kind;

        //Object일 떄 필요한 말풍선
        [SerializeField] private GameObject bubble;

        [SerializeField] private MainCamera mainCamera;

        //Nexxt Stage
        [SerializeField] private GameObject currentStages;
        [SerializeField] private GameObject nextStage;

        //스위치
        [SerializeField] private ObjectSwitch switchKind;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("On");
                switch (kind)
                {
                    case ObjectKind.NPC:
                        {
                            OpenDialog();
                            break;
                        }
                    case ObjectKind.Object:
                        {
                            ShowBubble();
                            break;
                        }
                    case ObjectKind.Door:
                        {
                            StartCoroutine(MoveNext());
                            break;
                        }
                    case ObjectKind.Zoom:
                        {
                            mainCamera.ZoomIn(5.5f);
                            break;
                        }
                    case ObjectKind.Switch:
                        {
                            switch (switchKind)
                            {
                                case ObjectSwitch.NPCTutorial:
                                    PrologueScript.Instance.isOnNPCPos = true; break;
                                case ObjectSwitch.End:
                                    PrologueScript.Instance.isEnd = true; break;
                            }
                            break;
                        }
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && kind == ObjectKind.Object)
            {
                Debug.Log("Off");
                CloseBubble();
            }
        }

        public void OpenDialog()
        {
            Data.DataManager.Instance.selected_dialog = new Data.NPCDAta(objcect_name);
            TownScene.UIManager.Instance.OpenMenu<TownScene.DialogUI>();
            TownScene.DialogUI.Instance.SetDialog();
        }

        public void ShowBubble()
        {
            bubble.SetActive(true);
            bubble.transform.DOScaleY(0.4f, 0.3f);
        }
        public void CloseBubble()
        {
            bubble.transform.DOScaleY(0, 0.5f).OnComplete(() => bubble.SetActive(false));
        }

        public IEnumerator MoveNext()
        {
            mainCamera.FadeOut();
            yield return new WaitForSeconds(2);

            currentStages.SetActive(false);
            nextStage.SetActive(true);
            mainCamera.FadeIn();
        }
    }
}
