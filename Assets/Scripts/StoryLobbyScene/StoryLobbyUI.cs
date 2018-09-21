using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryLobbyUI : MonoBehaviour
{
    public Button starButton;
    public Button starDescriptionButton;

    private void Awake()
    {
        starButton.onClick.AddListener(OnClickStarButton);
        starDescriptionButton.onClick.AddListener(OnClickStarDescriptionButton);
    }

    private void OnClickStarButton()
    {
        starDescriptionButton.gameObject.SetActive(!starDescriptionButton.gameObject.activeSelf);
        Debug.Log("누름");
    }

    private void OnClickStarDescriptionButton()
    {
        starDescriptionButton.gameObject.SetActive(false);
    }
}
