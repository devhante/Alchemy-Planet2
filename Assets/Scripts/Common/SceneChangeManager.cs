using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour {
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
    
    public void ChangeScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
        AlchemyPlanet.TownScene.UIManager.Instance.Clear();
    }

    public void ChangeSceneWithLoading(string scene_name)
    {
        LoadingSceneManager.LoadScene(scene_name);
        AlchemyPlanet.TownScene.UIManager.Instance.Clear();
    }
}
