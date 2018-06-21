using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TLine : MonoBehaviour
{
    public Vector3 start, end;

    public void Draw()
    {
        Vector3 differenceVector = end - start;

        transform.localScale = new Vector2(3, differenceVector.magnitude / transform.lossyScale.y);
        transform.position = (start + end) / 2;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}