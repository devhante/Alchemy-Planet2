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
        [SerializeField] private TownManagement TownManagerUIPrefab;
        [SerializeField] private TownUpgrade TownUpgradeUIPrefab;
        [SerializeField] private NameSetUI NameSetUIPrefab;

        [SerializeField] private GameObject TopDownUI;

        public Stack<Common.UI> menuStack = new Stack<Common.UI>();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        private void OnLevelWasLoaded(int level)
        {
            switch (level)
            {
                case 0: case 1: case 7: case 8:
                    TopDownUI.SetActive(false); break;
                default:
                    TopDownUI.SetActive(true); break;
            }
        }

        public void Clear()
        {
            while (menuStack.Count > 0)
            {
                CloseMenu();
            }
        }

        public void TownUIOff()
        {
            TopDownUI.SetActive(false);
            menuStack.Peek().gameObject.SetActive(false);
        }

        public void TownUIOn()
        {
            TopDownUI.SetActive(true);
            menuStack.Peek().gameObject.SetActive(true);
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
            GameObject.Destroy(instance.gameObject);
            if (menuStack.Count > 1)
            {
                menuStack.Peek().gameObject.SetActive(true);
            }
        }

        public T GetPrefab<T>() where T : Common.UI
        {
            if (typeof(T) == typeof(TownUI))
                return TownUIPrefab as T;
            else if (typeof(T) == typeof(DialogUI))
                return DialogUIPrefab as T;
            else if (typeof(T) == typeof(DialogLogMenu))
                return DialogLogMenuPrefab as T;
            else if (typeof(T) == typeof(EmptyUI))
                return EmptyUIPrefab as T;
            else if (typeof(T) == typeof(InventoryCell))
                return InventoryPrefab as T;
            else if (typeof(T) == typeof(TownManagement))
                return TownManagerUIPrefab as T;
            else if (typeof(T) == typeof(TownUpgrade))
                return TownUpgradeUIPrefab as T;
            else if (typeof(T) == typeof(NameSetUI))
                return NameSetUIPrefab as T;
            else
                throw new MissingReferenceException();
        }
    }
}