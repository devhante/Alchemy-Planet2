using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    //GM은 너무나 무능합니다. 기능을 주세요..
}
