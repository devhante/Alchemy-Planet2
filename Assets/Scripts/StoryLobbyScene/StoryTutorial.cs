using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTutorial : MonoBehaviour
{
    public static StoryTutorial Instance { get; private set; }

    public GameObject tutorialCanvas;

    [HideInInspector]
    public bool isTutorialFinished = false;

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Awake()
    {
        Instance = this;
        GameObject tutorialCanvasObject = Instantiate(tutorialCanvas);

        for(int i = 0; i < tutorialCanvasObject.transform.childCount - 1; i++)
        {
            Button buttonCurrent = tutorialCanvasObject.transform.GetChild(i).GetComponent<Button>();
            Button buttonNext = tutorialCanvasObject.transform.GetChild(i + 1).GetComponent<Button>();
            buttonCurrent.onClick.AddListener(() =>
            {
                Debug.Log("Click");
                buttonCurrent.gameObject.SetActive(false);
                buttonNext.gameObject.SetActive(true);
            });
        }

        Button button = tutorialCanvasObject.transform.GetChild(tutorialCanvasObject.transform.childCount - 1).GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            button.gameObject.SetActive(false);
            isTutorialFinished = true;
        });
    }
}
