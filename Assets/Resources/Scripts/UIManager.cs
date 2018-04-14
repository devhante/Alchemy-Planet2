using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Gages { Oxygen, Purify }

public class UIManager : MonoBehaviour {
    public UIManager Instance = null;

    public Canvas canvas;

    public Image OxygenGageMask;
    public Image PurifyGageMask;
    public Button PauseButton;

    public Button PauseLayoutPrefeb;
    public Button PauseLayout;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        PauseButton.onClick.AddListener(() =>
        {
            //이거 GM이나 딴데서 처리하는 게 나을지도..
            Time.timeScale = 0;
            PauseLayout = Instantiate(PauseLayoutPrefeb, canvas.transform);
            PauseLayout.onClick.AddListener(() => {
                Destroy(PauseLayout.gameObject);
                Time.timeScale = 1;
            });
        });
    }

    public void Start()
    {
        PlusGage(Gages.Purify, 60);
        //디버그를 위한 임시적인 방편
        StartCoroutine("TimeMinus");
    }

    public void MinusGage(Gages kind, float persent)
    {
        if (kind.Equals(Gages.Oxygen))
        {
            if (OxygenGageMask.transform.localScale.x <= 0)
            {
                OxygenGageMask.transform.localScale = new Vector3(0, 1, 1);
                return;
            }
            OxygenGageMask.transform.localScale -= new Vector3((persent / 100), 0, 0);
        }
        else if (kind.Equals(Gages.Purify))
        {
            if (PurifyGageMask.transform.localScale.x <= 0)
            {
                PurifyGageMask.transform.localScale = new Vector3(0, 1, 1);
                return;
            }
            PurifyGageMask.transform.localScale -= new Vector3((persent / 100), 0, 0);
        }
    }

    public void PlusGage(Gages kind, float persent)
    {
        if (kind.Equals(Gages.Oxygen))
        {
            if (OxygenGageMask.transform.localScale.x >= 1)
            {
                OxygenGageMask.transform.localScale = new Vector3(1, 1, 1);
                return;
            }
            OxygenGageMask.transform.localScale += new Vector3((persent / 100), 0, 0);
        }
        else if (kind.Equals(Gages.Purify))
        {
            if (PurifyGageMask.transform.localScale.x >= 1)
            {
                PurifyGageMask.transform.localScale = new Vector3(1, 1, 1);
                return;
            }
            PurifyGageMask.transform.localScale += new Vector3((persent / 100), 0, 0);
        }
    }

    IEnumerator TimeMinus()
    {
        while (true)
        {
            MinusGage(Gages.Oxygen, 5);
            MinusGage(Gages.Purify, 5);
            yield return new WaitForSeconds(1);
        }
    }
}
