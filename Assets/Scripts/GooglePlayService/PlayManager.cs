using UnityEngine;

public class PlayManager : MonoBehaviour {
    public static PlayManager Instance { get; private set;}
    public static int Counter { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void IncrementCounter()
    {
        Counter++;
        SampleUIscript.Instance.UpdatePointsText();
    }

    public void RestartGame()
    {
        PlayGamesScript.AddScoreToLeaderboard(GPGSIds.leaderboard_point, Counter);
        Counter = 0;
    }
}
