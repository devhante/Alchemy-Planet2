using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AlchemyPlanet.PrologueScene
{
    public enum ObjectKind { NPC, Object, Door }

    public class PrologueObject : MonoBehaviour
    {
        [SerializeField] private string objcect_name;
        [SerializeField] private ObjectKind kind;

        [SerializeField] private GameObject bubble;

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
                            MoveNext();
                            break;
                        }
                }
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
            bubble.transform.DOScale(1, 0.5f);
        }
        public void MoveNext()
        {

        }
    }
}
