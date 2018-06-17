using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSizeSpriteToCamera : MonoBehaviour
{
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        Camera.main.orthographicSize = 1280 / (2 * 100f);

        Screen.SetResolution(720, 1280, false);
    }
}
