using UnityEngine;

public class SaveLoadManager : GenericMonoSingleton<SaveLoadManager> 
{
    private string HighScoreKey = "BestWaveSavedValue";

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(HighScoreKey, score);
    }

    public int LoadHighScore()
    {
        if (PlayerPrefs.HasKey(HighScoreKey))
        {
            return PlayerPrefs.GetInt(HighScoreKey);
        }
        else
        {
            return 0;
        }
    }
}
