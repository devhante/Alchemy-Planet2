using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public GameUI gameUIPrefab;
        public PauseUI pauseUIPrefab;
        public EndUI endUIPrefab;
        public ResumeUI ResumeUIPrefab;

        public Stack<Common.UI> menuStack = new Stack<Common.UI>();

        private void Awake()
        {
            Instance = this;
            OpenMenu<GameUI>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void OpenMenu<T>() where T : Common.UI
        {
            var prefab = GetPrefab<T>();
            var instance = Instantiate<Common.UI>(prefab, transform);

            menuStack.Push(instance);
        }

        public void CloseMenu()
        {
            var instance = menuStack.Pop();
            Destroy(instance.gameObject);
        }

        public T GetPrefab<T>() where T : Common.UI
        {
            if (typeof(T) == typeof(GameUI))
                return gameUIPrefab as T;

            if (typeof(T) == typeof(PauseUI))
                return pauseUIPrefab as T;

            if (typeof(T) == typeof(EndUI))
                return endUIPrefab as T;

            if (typeof(T) == typeof(ResumeUI))
                return ResumeUIPrefab as T;

            throw new MissingReferenceException();
        }
    }
}