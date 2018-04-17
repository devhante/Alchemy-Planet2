using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager Instance { get; private set; }

    public GameUI GameUIPrefeb;
    public PauseUI PauseUIPrefeb;
    public EndUI EndUIPreFeb;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OpenMenu<GameUI>();
    }

    public void OpenMenu<T>()
    {
        if(typeof(T) == typeof(GameUI))
        {
            Instantiate(GameUIPrefeb);
        }
        if(typeof(T) == typeof(PauseUI))
        {
            Instantiate(PauseUIPrefeb);
        }
        if(typeof(T) == typeof(EndUI))
        {
            Instantiate(EndUIPreFeb);
        }
    }
}
