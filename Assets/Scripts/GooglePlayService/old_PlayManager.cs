using UnityEngine;

public class old_PlayManager : MonoBehaviour {
    public static old_PlayManager Instance { get; private set;}
    public static int Counter { get; private set; }

    private void Awake()
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
        old_PlayGamesScript.AddScoreToLeaderboard(GPGSIds.leaderboard_point, Counter);
        Counter = 0;
    }
}
