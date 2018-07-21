using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    [SerializeField] private GameObject player;

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

            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

            if (touchesPrevPosDifference > touchesCurPosDifference)
                mainCamera.orthographicSize += zoomModifier;
            if (touchesPrevPosDifference < touchesCurPosDifference)
                mainCamera.orthographicSize -= zoomModifier;
        }
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 6f, 12f);
    }

    void Follow() { transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3, transform.position.z); }
}
