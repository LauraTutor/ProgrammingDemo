using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManagerMM : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    private void Start()
    {
        startButton.onClick.AddListener(() => StartGame(1));
        quitButton.onClick.AddListener(QuitGame);
    }
    public void StartGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
        Debug.Log("Has Quit Game");
    }
}