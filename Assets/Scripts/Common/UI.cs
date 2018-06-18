using UnityEngine;

namespace AlchemyPlanet.Common
{
    public abstract class UI : MonoBehaviour
    {

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
    }
}
