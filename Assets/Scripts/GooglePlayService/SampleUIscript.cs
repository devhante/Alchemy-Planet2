using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleUIscript : MonoBehaviour
{
    public static SampleUIscript Instance;
    public Text[] ValueTextArray;

    public void Awake()
    {
        Instance = this;
    }
}