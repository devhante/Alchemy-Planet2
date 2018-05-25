using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleUIscript : MonoBehaviour {

    public static SampleUIscript Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Text points;

    public void GetPoint()
    {
        PlayManager.Instance.IncrementCounter();
    }

    public void Restart()
    {
        PlayManager.Instance.RestartGame();
    }

    public void Increment()
    {
        PlayGamesScript.IncerementAchievement(GPGSIds.achievement_sample, 5);
    }

    public void Unlock()
    {
        PlayGamesScript.UnloadAchievement(GPGSIds.achievement_nothing);
    }

    public void ShowAchievements()
    {
        PlayGamesScript.ShowAchievementsUI();
    }

    public void ShowLeaderboards()
    {
        PlayGamesScript.ShowLeaderboardsUI();
    }

    public void UpdatePointsText()
    {
        points.text = PlayManager.Counter.ToString();
    }
}
