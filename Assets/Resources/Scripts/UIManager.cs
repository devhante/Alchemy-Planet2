using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Gages { Oxygen, Purify }

public class UIManager : MonoBehaviour {
    public UIManager Instance = null;

    public Image OxygenGageMask;
    public Image PurifyGageMask;
    public Button PauseButton;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        PauseButton.onClick.AddListener(() => {
            Time.timeScale = 0;
        });
    }

    public void Start()
    {
        StartCoroutine("A");
    }

    public void MinusGage(Gages kind, float persent)
    {
        if (kind.Equals(Gages.Oxygen))
        {
            OxygenGageMask.transform.localScale -= new Vector3((persent / 100), 0, 0);
        }
        else if (kind.Equals(Gages.Purify))
        {
            PurifyGageMask.transform.localScale -= new Vector3((persent / 100), 0, 0);
        }
    }

    IEnumerator A()
    {
        while (true)
        {
            MinusGage(Gages.Oxygen, 5);
            MinusGage(Gages.Purify, 10);
            yield return new WaitForSeconds(1);
        }
    }
}
