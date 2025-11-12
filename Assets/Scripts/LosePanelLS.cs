using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanelLS : MonoBehaviour
{
    public Button tryagainButton;
    public Button menuButton;
    public Button quitButton;
    public Image fadeImage;

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;
    public float delayBeforeLoad = 0.5f;

    private void Start()
    {
        tryagainButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(LoadScene);
        quitButton.onClick.AddListener(QuitGame);
    }

public void LoadScene()
{
    SceneManager.LoadScene("MainMenuLS");
}

public void RestartGame()
{
    SceneManager.LoadScene("Level1LS");
}

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Has Quit Game");
    }
}