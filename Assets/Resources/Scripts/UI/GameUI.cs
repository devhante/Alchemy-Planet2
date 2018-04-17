using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Gages { Oxygen, Purify }

public class GameUI : MonoBehaviour {


    public Image OxygenGageMask;
    public Image PurifyGageMask;
    public Button PauseButton;

    //게이지가 줄어들 수록 감소량이 줄어드는 변수. 이름 정해주세요..
    public float Correction;

    public void Awake()
    {
        Correction = 1;
        PauseButton.onClick.AddListener(() =>
        {
            UIManager.Instance.OpenMenu<PauseUI>();
        });
    }

    public void Start()
    {
        UpdateGage(Gages.Purify, 0);

        GameManager.Instance.StartGame();

        StartCoroutine("TimeMinus");
    }

    public void UpdateGage(Gages kind, float persent)
    {
        if (kind.Equals(Gages.Oxygen))
        {
            OxygenGageMask.transform.localScale += new Vector3((persent / 100) * Correction, 0, 0);

            if (OxygenGageMask.transform.localScale.x <= 0)
            {
                OxygenGageMask.transform.localScale = new Vector3(0, 1, 1);
                UIManager.Instance.OpenMenu<EndUI>();
            }
            if (OxygenGageMask.transform.localScale.x >= 1)
            {
                OxygenGageMask.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if (kind.Equals(Gages.Purify))
        {
            PurifyGageMask.transform.localScale += new Vector3((persent / 100), 0, 0);

            if (PurifyGageMask.transform.localScale.x <= 0)
            {
                PurifyGageMask.transform.localScale = new Vector3(0, 1, 1);
                return;
            }
            if (PurifyGageMask.transform.localScale.x >= 1)
            {
                PurifyGageMask.transform.localScale = new Vector3(1, 1, 1);
                return;
            }

            UpdateCurrection();
        }
    }

    void UpdateCurrection()
    {
        int gage_Per = Mathf.RoundToInt(PurifyGageMask.transform.localScale.x * 100);
        switch (gage_Per / 10)
        {
            case 10:
            case 9:
            case 8: Correction = 0.1f; break;
            case 7:
            case 6:
            case 5:
            case 4: Correction = 0.5f; break;
            case 3:
            case 2:
            case 1:
            case 0: Correction = 1; break;
        }
    }

    IEnumerator TimeMinus()
    {
        while (true)
        {
			yield return new WaitForSeconds(1);
			UpdateGage(Gages.Oxygen, -10);
            UpdateGage(Gages.Purify, -10);   
        }
    }
}
