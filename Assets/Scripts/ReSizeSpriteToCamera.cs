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

        float cameraWidthHalf = 3.6f * (1f * Screen.width / Screen.height) / (720f / 1280f);
        Camera.main.transform.position += new Vector3(cameraWidthHalf - 3.6f, 0, 0);
    }
}
