using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    [Space] 
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private GameObject credits;
    // For Start button
    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // For Scoreboard button
    public void ShowScoreBoard()
    {
        scoreboard.SetActive(true);
        gameObject.SetActive(false);
    }

    // For Credits
    public void ShowCredits()
    {
        credits.SetActive(true);
        gameObject.SetActive(false);
    }

    // For Exit button
    public void ExitGame()
    {
        Application.Quit();
    }
    
}
