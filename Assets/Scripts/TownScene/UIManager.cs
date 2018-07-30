using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.TownScene
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private TownUI TownUIPrefab;
        [SerializeField] private DialogUI DialogUIPrefab;
        [SerializeField] private DialogLogMenu DialogLogMenuPrefab;
        [SerializeField] private EmptyUI EmptyUIPrefab;
        [SerializeField] private InventoryCell InventoryPrefab;

        public Stack<Common.UI> menuStack = new Stack<Common.UI>();

        private void Awake()
        {
            Instance = this;
            OpenMenu<TownUI>();
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

        //public void OpenMenu<T>(bool disablePrev) where T : Common.UI
        //{
        //    var prefab = GetPrefab<T>();
        //    var instance = Instantiate<Common.UI>(prefab, transform);

        //    menuStack.Push(instance);
        //}

        public void CloseMenu()
        {
            var instance = menuStack.Pop();
            Destroy(instance.gameObject);
            menuStack.Peek().gameObject.SetActive(true);
        }

        public T GetPrefab<T>() where T : Common.UI
        {
            if (typeof(T) == typeof(TownUI))
                return TownUIPrefab as T;
            if (typeof(T) == typeof(DialogUI))
                return DialogUIPrefab as T;
            if (typeof(T) == typeof(DialogLogMenu))
                return DialogLogMenuPrefab as T;
            if (typeof(T) == typeof(EmptyUI))
                return EmptyUIPrefab as T;
            if (typeof(T) == typeof(InventoryCell))
                return InventoryPrefab as T;

            throw new MissingReferenceException();
        }
    }
}