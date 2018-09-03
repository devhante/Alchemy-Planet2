using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlchemyPlanet.Common
{
    public class Tutorial : MonoBehaviour
    {
        public static Tutorial Instance;
        public int process;

        public bool loaded;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            process = 0;
            CheckCurrentScene();
        }

        private void OnLevelWasLoaded(int level)
        {
            Debug.Log("Check");
            loaded = true;
        }

        public void CheckCurrentScene()
        {
            Debug.Log(SceneManager.GetActiveScene().name);
            switch (process)
            {
                case 0:
                    {
                        //우주 행성 거래소
                        Data.DataManager.Instance.selected_dialog = new Data.NPCDAta("Prologue_00");
                        SceneChangeManager.Instance.LoadDialogScene();
                        break;
                    }
                case 1:
                    {
                        //우주선 안
                        Data.DataManager.Instance.selected_dialog = new Data.NPCDAta("Prologue_01");
                        SceneChangeManager.Instance.LoadDialogScene();
                        break;
                    }
                case 2:
                    {
                        //타운으로 불시착
                        Data.DataManager.Instance.selected_dialog = new Data.NPCDAta("Prologue_02");
                        SceneChangeManager.Instance.ChangeSceneWithLoading("TownScene");
                        StartCoroutine(OpenDialogAfterSceneLoading());
                        break;
                    }
                case 3:
                    {
                        //정화약을 구하러 자동으로 이동
                        SceneChangeManager.Instance.ChangeSceneWithLoading("GameScene");
                        break;
                    }
                case 4:
                    {

                        break;
                    }
            }
        }

        IEnumerator OpenDialogAfterSceneLoading()
        {
            while (! loaded)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(1);
            TownScene.UIManager.Instance.OpenMenu<TownScene.DialogUI>();
            TownScene.DialogUI.Instance.SetDialog();
        }
    }
}