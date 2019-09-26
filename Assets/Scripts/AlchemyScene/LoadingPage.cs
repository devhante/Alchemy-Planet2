using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoadingPage : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> loadingLetterList;
    [SerializeField]
    private float jumptime;
    [SerializeField]
    private float jumpPower;

    private WaitForSeconds jumpTimeSeconds;

    // Start is called before the first frame update
    void Start()
    {
        jumpTimeSeconds = new WaitForSeconds(jumptime);
        StartCoroutine(BoundSentence());
    }

    private IEnumerator BoundSentence()
    {
        yield return jumpTimeSeconds;
        foreach (var loadingLetter in loadingLetterList)
        {
            loadingLetter.transform.DOJump(loadingLetter.transform.position, jumpPower, 1, jumptime);
            yield return jumpTimeSeconds;
        };
        foreach (var loadingLetter in loadingLetterList)
        {
            loadingLetter.transform.DOJump(loadingLetter.transform.position, jumpPower, 1, jumptime);
            yield return jumpTimeSeconds;
        };
        Destroy(gameObject);
        yield break;
    }
}
