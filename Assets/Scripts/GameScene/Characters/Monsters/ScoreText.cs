using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private Text text;
    private RectTransform rt;
    private float duration;
    private float upDistance;
    private int fps;

    private void Awake()
    {
        text = GetComponent<Text>();
        rt = GetComponent<RectTransform>();
        duration = 0.5f;
        upDistance = 30;
        fps = 30;
    }

    private void Start()
    {
        StartCoroutine("ScoreTextCoroutine");
    }

    IEnumerator ScoreTextCoroutine()
    {
        int count = (int)(duration * fps);
        float frame = 1.0f / fps;

        for(int i = 0; i < count; i++)
        {
            rt.position += new Vector3(0, upDistance / count, 0);
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (1.0f / count));
            yield return new WaitForSeconds(frame);
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        Destroy(gameObject);
    }
}
