using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectUI : MonoBehaviour {
    public void SetStageIndex(int index)
    {
        DataManager.Instance.selected_stage = index;
    }
}
