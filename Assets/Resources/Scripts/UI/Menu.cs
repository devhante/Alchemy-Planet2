using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    public abstract void OnBackPressed();
}

public abstract class Menu<T> : Menu where T : Menu<T>
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

        MenuManager.Instance.OpenMenu<T>();
    }

    protected static void Close()
    {
        if(Instance == null)
        {
            Debug.LogErrorFormat("Not Found {0}", Instance.ToString());
            return;
        }

        MenuManager.Instance.CloseMenu();
    }
}
