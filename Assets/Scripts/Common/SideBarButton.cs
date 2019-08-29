using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SideBarButton : MonoBehaviour
{
    private Image sideBarIcon;
    private Button[] sidaBarIconList;
    private Button button;
    private bool openingMenu = true;

    // Start is called before the first frame update
    void Start()
    {
        sideBarIcon = GetComponent<Image>();
        sidaBarIconList = GetComponentsInChildren<Button>();
        button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if (openingMenu)
                CloseSideBar();
            else
                OpenSideBar();
        });
    }

    void OpenSideBar()
    {
        float iconY = 0;
        openingMenu = true;
        foreach (var icon in sidaBarIconList)
        {
            iconY = icon.transform.position.y;
            icon.transform.DOMoveY(iconY - 200, 1);
        }
    }

    void CloseSideBar()
    {
        openingMenu = false;
        float iconY = 0;
        foreach (var icon in sidaBarIconList)
        {
            iconY = icon.transform.position.y;
            icon.transform.DOMoveY(iconY + 200, 1);
        }
    }
}
