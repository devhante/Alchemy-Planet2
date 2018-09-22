using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlchemyPlanet.StoryLobbyScene
{
    public class StoryManager : MonoBehaviour
    {
        public static StoryManager Instance { get; private set; }

        public int CurrentChaper { get; set; }
        public int CurrentStage { get; set; }
        public int CurrentMaxStage { get; set; }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "GameScene" && scene.name != "StoryLobbyScene" && scene.name != "LoadingScene")
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                Destroy(gameObject);
            }
        }
    }
}
