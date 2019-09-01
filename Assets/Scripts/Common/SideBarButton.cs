using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SideBarButton : MonoBehaviour
{
    public Button[] sideBarButtonList;

    private bool openingMenu = true;
    private float axisY;

    // Start is called before the first frame update
    void Start()
    {
        axisY = transform.position.y;
        gameObject.AddComponent<Button>().onClick.AddListener(() =>
        {
            if (openingMenu)
                CloseSideBar();
            else
                OpenSideBar();
        });
    }

    void OpenSideBar()
    {
        openingMenu = true;
        transform.DORotate(new Vector3(0, 0, 0), 1).SetEase(Ease.OutQuint);
        for(int i = 0; i < sideBarButtonList.Length; i++)
        {
            Button button = sideBarButtonList[i];
            button.transform.DOMoveY(axisY - (i + 1) * 90, 1).SetEase(Ease.OutQuint);
        }
    }

    void CloseSideBar()
    {
        openingMenu = false;
        transform.DORotate(new Vector3(0, 0, 180), 1).SetEase(Ease.OutQuint);
        foreach (var button in sideBarButtonList)
        {
            float buttonY = button.transform.position.y;
            button.transform.DOMoveY(axisY, 1).SetEase(Ease.OutQuint);
        }
    }
}
