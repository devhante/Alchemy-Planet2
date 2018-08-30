using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.Common {
    public class DialogScene : MonoBehaviour
    {
        void Start()
        {
            TownScene.DialogUI.Instance.SetDialog();
        }
    }

}