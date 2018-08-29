using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlchemyPlanet.Common
{
    public class Tutorial : MonoBehaviour
    {
        public static Tutorial Instance;
        public int process;

        private void Awake()
        {
            Instance = this;
            process = 0;
        }

        public void CheckCurrentScene()
        {
            Debug.Log(SceneManager.GetActiveScene().name);

            switch (process)
            {
                case 0:
                    {
                        SceneChangeManager.Instance.LoadDialogScene("Prologue");
                        break;
                    }
            }
        }
    }
}