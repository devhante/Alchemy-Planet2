using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour {
    public Button Resume;
    public Button Restart;
    public Button Exit;

    public void Awake()
    {
        Time.timeScale = 0;

        Resume.onClick.AddListener(() => {
            Time.timeScale = 1;
            Destroy(this.gameObject);
        });

        Restart.onClick.AddListener(() => {
			Time.timeScale = 1;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		});

        Exit.onClick.AddListener(() => {
			Application.Quit();
		});
    }
}
