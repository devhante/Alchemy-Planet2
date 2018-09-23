using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SpeechBubble : MonoBehaviour {

    [SerializeField] private float startDelay;
    private WaitForSeconds start_delay;

    [SerializeField] private float offset;
    private WaitForSeconds bubble_offset;

    [SerializeField] private float textOffset;
    private WaitForSeconds text_offset;

    [SerializeField] private TextMesh text;

    private void Awake()
    {
        start_delay = new WaitForSeconds(startDelay);
        bubble_offset = new WaitForSeconds(offset);
        text_offset = new WaitForSeconds(textOffset);
    }

    void Start () {
        StartCoroutine(Show());
	}

    IEnumerator Show()
    {
        yield return start_delay;

        while (true)
        {
            this.transform.DOScale(1, 1).SetEase(Ease.InBack);

            yield return text_offset;
            text.text = ".";
            yield return text_offset;
            text.text = ". .";
            yield return text_offset;
            text.text = ". . .";
            yield return text_offset;

            this.transform.DOScale(0, 1).SetEase(Ease.InBack).OnComplete(() => text.text = "");

            yield return bubble_offset;
        }
    }
}
