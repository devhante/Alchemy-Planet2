using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {
    public static Tutorial Instance;
    public int proscess;

    private void Awake()
    {
        Instance = this;
        proscess = 0;
    }

    public void CheckCurrentScene()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
    }
}