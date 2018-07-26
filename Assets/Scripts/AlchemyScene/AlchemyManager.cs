using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyManager : MonoBehaviour {
    List<Formula> formulas;

    private void Awake()
    {
        formulas = DataManager.Instance.LoadFormulas();
    }

    private void Start()
    {
        Material material;
        DataManager.Instance.materials.TryGetValue("A001", out material);

        Debug.Log(material.item_name);
        Debug.Log(formulas[0].ToString());
    }
}