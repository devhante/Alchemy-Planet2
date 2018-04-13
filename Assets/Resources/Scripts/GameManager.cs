using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameManager Instance { get; private set; }

    public float gameScore;
    public float gameTime;

    private void Awake()
    {
        Instance = this;
    }

    IEnumerator TimeCOunt(float StartTIme)
    {
        gameTime = StartTIme;
        while (gameTime < 0)
        {
            yield return new WaitForSeconds(1);
            gameTime -= Time.deltaTime;
        }
    }
}
