using UnityEngine;

public class PlayManager : MonoBehaviour {
    public static PlayManager Instance { get; private set;}
    public static int Counter { get; private set; }

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void IncrementCounter()
    {
        Counter++;
        SampleUIscript.Instance.UpdatePointsText();
    }

    public void RestartGame()
    {
        PlayGamesScript.AddScoreToLeaderboard(GPGSIds.leaderboard_point, Counter);

        if(Counter > CloudVariables.Highscore)
        {
            CloudVariables.Highscore = Counter;
            PlayGamesScript.Instance.SaveData();
            SampleUIscript.Instance.UpdateHighscoreText();
        }

        Counter = 0;
    }
}
