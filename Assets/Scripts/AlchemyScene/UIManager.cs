using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.AlchemyScene
{
    public class UIManager : MonoBehaviour {
        public static UIManager Instance { get; private set; }

        [SerializeField] private AlchemyUI AlchemyUIPrefab;
        [SerializeField] private MakeUI MakeUIPrefab;

        public Stack<Common.UI> menuStack = new Stack<Common.UI>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
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
            GameObject.Destroy(instance.gameObject);
            if (menuStack.Count > 1)
            {
                menuStack.Peek().gameObject.SetActive(true);
            }
        }

        public T GetPrefab<T>() where T : Common.UI
        {
            if (typeof(T) == typeof(AlchemyUI))
                return AlchemyUIPrefab as T;
            else if (typeof(T) == typeof(MakeUI))
                return MakeUIPrefab as T;
            else
                throw new MissingReferenceException();
        }
    }
}

