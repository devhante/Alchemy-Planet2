using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSelectUI : SelectUI
{
    [SerializeField] private List<GameObject> itemButtonList;

    [SerializeField] private Button CloseButton;
    public GameObject backGroundImage;

    protected override void Awake()
    {
        base.Awake();
    }
}