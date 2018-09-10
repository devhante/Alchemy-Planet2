using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject backgroundImage;


    private Camera mainCamera;
    private float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier; 
    private Vector2 firstTouchPrevPos, secondTouchPrevPos;

    float zoomModifierSpeed = 0.1f;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        Zoom();
        Follow();
    }

    void Zoom()
    {
        if (Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed/10;

            if (touchesPrevPosDifference > touchesCurPosDifference)
                mainCamera.orthographicSize += zoomModifier;
            if (touchesPrevPosDifference < touchesCurPosDifference)
                mainCamera.orthographicSize -= zoomModifier;
        }
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 7, 12);
        backgroundImage.transform.localScale = new Vector3(1.2f + (mainCamera.orthographicSize - 7) / 4, 1.2f + (mainCamera.orthographicSize - 7) / 4);
        backgroundImage.transform.position = new Vector3(backgroundImage.transform.position.x, 2 + 2.4f * ((mainCamera.orthographicSize - 7) / 5));
    }

    IEnumerator ZoomOut()
    {
        while (mainCamera.orthographicSize < 12)
        {
            mainCamera.orthographicSize += 0.5f;
            yield return null;
        }
    }

    void Follow()
    {
        if(player.activeSelf)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2, transform.position.z);
    }
}
