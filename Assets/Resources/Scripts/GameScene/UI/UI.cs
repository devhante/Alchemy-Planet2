using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public abstract class UI : MonoBehaviour
    {
        public abstract void OnBackPressed();
    }

    public abstract class UI<T> : UI where T : UI<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }

        public override void OnBackPressed()
        {
            Close();
        }

        protected static void Open()
        {
            if (Instance != null)
                return;

            UIManager.Instance.OpenMenu<T>();
        }

        protected static void Close()
        {
            if (Instance == null)
            {
                Debug.LogErrorFormat("Not Found {0}", Instance.ToString());
                return;
            }

            UIManager.Instance.CloseMenu();
        }
    }
}
