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

    [Header("Scene Settings")]
    public string MainMenu = "MainMenu";

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;
    public float delayBeforeLoad = 0.5f;

    private void Start()
    {
        tryagainButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener (LoadScene);
        quitButton.onClick.AddListener(QuitGame);

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.gameObject.SetActive(false);
        }
    }

    private IEnumerator LoadSceneRoutine(string scene)
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);

            float elapsed = 0f;
            Color c = fadeImage.color;

            // Fade to black
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                c.a = Mathf.Clamp01(elapsed / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }

            yield return new WaitForSeconds(delayBeforeLoad);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    private void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        StartCoroutine(LoadSceneRoutine(currentScene.name)); // use coroutine for fade
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Has Quit Game");
    }
}
