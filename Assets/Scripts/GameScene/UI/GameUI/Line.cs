using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Line : MonoBehaviour
{
    RectTransform rt;
    public Vector3 start, end;
    bool isMouseButtonDown;
    float width;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        isMouseButtonDown = true;
        width = 20;
    }

    private void Start()
    {
        StartCoroutine("DrawCoroutine");
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            isMouseButtonDown = false;
    }

    IEnumerator DrawCoroutine()
    {
        while(isMouseButtonDown)
        {
            end = Input.mousePosition;
            Draw();
            yield return null;
        }
    }

    public void Draw()
    {
        Vector3 differenceVector = end - start;

        rt.sizeDelta = new Vector2(differenceVector.magnitude, width);
        rt.pivot = new Vector2(0, 0.5f);
        rt.position = start;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        rt.rotation = Quaternion.Euler(0, 0, angle);
    }
}