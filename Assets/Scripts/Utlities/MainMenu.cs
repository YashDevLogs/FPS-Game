using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    string NewGameScene = "LEVEL";

    public TextMeshProUGUI HighScoreUI;

    public AudioClip BgMusic;
    public AudioSource MainMenuChannel;

    private void Start()
    {
        MainMenuChannel.PlayOneShot(BgMusic);
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        HighScoreUI.text = $"Top Wave Survived: {highScore}";
    }

    public void StartGame()
    {
        MainMenuChannel.Stop();
        SceneManager.LoadScene(NewGameScene); 
    }

    public void Exit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
     Application.Quit();
#endif

    }
}
