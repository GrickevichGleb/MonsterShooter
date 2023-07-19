using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{

    public void PlayAgainButton()
    {
        gameObject.SetActive(false);
        // Reloads current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0); //Main menu scene has index 0
    }
}
