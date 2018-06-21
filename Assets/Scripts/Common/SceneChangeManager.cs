using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour {
    public SceneChangeManager Instance;

    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void ChangeScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    public void ChangeSceneWithLoading(string scene_name)
    {
        LoadingSceneManager.LoadScene(scene_name);
    }
}
