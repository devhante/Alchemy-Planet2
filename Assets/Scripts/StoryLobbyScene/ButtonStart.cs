using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.StoryLobbyScene
{
    public class ButtonStart : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClickButtonStart);
        }

        private void OnClickButtonStart()
        {
            if(StoryManager.Instance.CurrentChaper == 1)
            {
                if (StoryManager.Instance.CurrentStage == 1)
                    Common.AlertManager.Instance.InstantiateAlert("준비중입니다.");
                else if (StoryManager.Instance.CurrentStage == 7)
                    Common.AlertManager.Instance.InstantiateAlert("준비중입니다.");
                else
                    SceneChangeManager.Instance.ChangeSceneWithLoading("GameScene");
            }
        }
    }
}