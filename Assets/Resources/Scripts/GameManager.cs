using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public float PlayTime;

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        PlayTime = 0;
        StartCoroutine("TimeCount");
    }

    public void EndGame()
    {
        StopCoroutine("TimeCount");
    }

    IEnumerator TimeCount()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            ++PlayTime;
        }
    }
    //GM은 너무나 무능합니다. 기능을 주세요..

}
