using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadUI : MonoBehaviour
{
    [SerializeField] private TLine LinePrefab;
    [SerializeField] private Sprite selected_Point;

    private Touch tempTouchs;

    void Awake()
    {
        int count = transform.childCount;
        for (int i = 0; i < count; ++i)
        {
            if (i < count - 1)
            {
                TLine line = Instantiate<TLine>(LinePrefab, this.transform);
                line.name = "Line_" + i;
                line.start = transform.GetChild(i).position;
                line.end = transform.GetChild(i + 1).position;
                line.Draw();
            }
        }
    }

    private void Update()
    {
        Touch();
    }

    void Touch()    // 터치감지
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                tempTouchs = Input.GetTouch(i);
                if (tempTouchs.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject() == false)
                {
                    Vector3 touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);
                    RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero, 1);
                    if (hit.collider.tag == "Point")
                    {
                        LoadingSceneManager.LoadScene("RunTest");
                    }
                    break;
                }
            }
        }
    }
}