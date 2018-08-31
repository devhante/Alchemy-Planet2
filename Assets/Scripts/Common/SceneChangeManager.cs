using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlchemyPlanet
{
    public class SceneChangeManager : MonoBehaviour
    {
        public static SceneChangeManager Instance;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        private void Update()
        {
            if (Screen.fullScreen == false)
                Screen.fullScreen = true;
        }

        public void ChangeScene(string scene_name)
        {
            SceneManager.LoadScene(scene_name);
            AlchemyPlanet.TownScene.UIManager.Instance.Clear();
        }

        public void ChangeSceneWithLoading(string scene_name)
        {
            LoadingSceneManager.LoadScene(scene_name);
            if (AlchemyPlanet.TownScene.UIManager.Instance)
            {
                AlchemyPlanet.TownScene.UIManager.Instance.Clear();
            }
        }

        public void LoadDialogScene()
        {
            AlchemyPlanet.TownScene.UIManager.Instance.Clear();
            LoadingSceneManager.LoadScene("DialogScene");
        }
    }

}