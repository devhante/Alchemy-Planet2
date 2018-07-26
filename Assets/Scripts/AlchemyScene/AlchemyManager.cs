using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyManager : MonoBehaviour {
    Dictionary<string, Material> materials;
    List<Formula> formulas;

    private void Awake()
    {
        materials = DataManager.LoadMaterialData();
        formulas = DataManager.LoadFormulas();
    }

    private void Start()
    {
        Material material;
        materials.TryGetValue("A001", out material);

        Debug.Log(material.item_id);
        Debug.Log(formulas[0].ToString());
    }
}