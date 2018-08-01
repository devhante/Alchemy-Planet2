using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    public Button applyButton;

    protected virtual void Awake()
    {
        applyButton.onClick.AddListener(OnClickApplyButton);
    }

    protected virtual void OnClickApplyButton()
    {
        Destroy(gameObject);
    }
}
