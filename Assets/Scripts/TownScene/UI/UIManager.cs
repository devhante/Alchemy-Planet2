using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField] private BuildingPlacement BuildingPlacementPrefab;
        [SerializeField] private BuildingManagement BuildingManagementPrefab;
        [SerializeField] private NameSetUI NameSetUIPrefab;

        [SerializeField] private GameObject TopDownUI;
        [SerializeField] private GameObject TopDownUI_S;

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

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (Common.StateBar.Instance)
                Common.StateBar.Instance.UpdateState();
            if (Common.StateBar_s.Instance)
                Common.StateBar_s.Instance.UpdateState();

            switch (scene.buildIndex)
            {
                case 0:
                case 1:
                case 7:
                case 8:
                case 10:
                    {
                        TopDownUI.SetActive(false);
                        TopDownUI_S.SetActive(false); break;
                    }
                // case 5: AlchemyScene?
                case 11:
                    {   
                        TopDownUI.SetActive(false);
                        TopDownUI_S.SetActive(true); break;
                    }
                default:
                    {
                        TopDownUI.SetActive(true);
                        TopDownUI_S.SetActive(false); break;
                    }
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
            else if (typeof(T) == typeof(BuildingPlacement))
                return BuildingPlacementPrefab as T;
            else if (typeof(T) == typeof(BuildingManagement))
                return BuildingManagementPrefab as T;
            else if (typeof(T) == typeof(NameSetUI))
                return NameSetUIPrefab as T;
            else
                throw new MissingReferenceException();
        }
    }
}