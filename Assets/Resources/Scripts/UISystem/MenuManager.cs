using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    public GameMenu gameMenuPrefeb;
    public PauseMenu pauseMenuPrefeb;
    public EndMenu endMenuPreFeb;

    public Stack<Menu> menuStack = new Stack<Menu>();

    private void Awake()
    {
        Instance = this;
        OpenMenu<GameMenu>();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && menuStack.Count > 0)
        {
            menuStack.Peek().OnBackPressed();
        }
    }

    public void OpenMenu<T>() where T : Menu
    {
        var prefab = GetPrefab<T>();
        var instance = Instantiate<Menu>(prefab, transform);

        menuStack.Push(instance);
    }

    public void CloseMenu()
    {
        var instance = menuStack.Pop();
        Destroy(instance.gameObject);
    }

    public T GetPrefab<T>() where T : Menu
    {
        if (typeof(T) == typeof(GameMenu))
            return gameMenuPrefeb as T;

        if (typeof(T) == typeof(PauseMenu))
            return pauseMenuPrefeb as T;

        if (typeof(T) == typeof(EndMenu))
            return endMenuPreFeb as T;

        throw new MissingReferenceException();
    }
}
