using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlchemyPlanet.Data;

public class MapSelectUI : MonoBehaviour {
    public void SetStageIndex(int index)
    {
        DataManager.Instance.selected_stage = index;
    }
}
