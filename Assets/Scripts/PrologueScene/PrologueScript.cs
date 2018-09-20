using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.PrologueScene
{
    public class PrologueScript : MonoBehaviour
    {
        [SerializeField] private MainCamera mainCamera;

        void Start()
        {
            //StartCoroutine(LateStart(1));
        }

        private IEnumerator LateStart(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            DataManager.Instance.selected_dialog = new NPCDAta("Prologue_00");
            TownScene.UIManager.Instance.OpenMenu<TownScene.DialogUI>();
            TownScene.DialogUI.Instance.SetDialog();

            while (TownScene.DialogUI.Instance != null)
            {
                yield return new WaitForEndOfFrame();
            }

            mainCamera.FadeIn();

            yield return new WaitForSeconds(3);

            DataManager.Instance.selected_dialog = new NPCDAta("Prologue_01");
            TownScene.UIManager.Instance.OpenMenu<TownScene.DialogUI>();
            TownScene.DialogUI.Instance.SetDialog();

            while (TownScene.DialogUI.Instance != null)
            {
                yield return new WaitForEndOfFrame();
            }

            //mainCamera.FadeOut();

            yield return new WaitForSeconds(3);

            DataManager.Instance.selected_dialog = new NPCDAta("Prologue_Bell");
            TownScene.UIManager.Instance.OpenMenu<TownScene.DialogUI>();
            TownScene.DialogUI.Instance.SetDialog();
        }
    }
}