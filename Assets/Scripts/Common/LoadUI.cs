using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUI : MonoBehaviour
{
    [SerializeField] private TLine LinePrefab;
    [SerializeField] private Sprite selected_Point;

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
}