using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public Button menuButton;
    public Button quitButton;
    public Image fadeImage; // Assign this in the Inspector

    public float fadeDuration = 1.5f; // Time it takes to fade in
    public float delayBeforeLoad = 0.5f; // Delay after fade before loading

    private void Start()
    {
        menuButton.onClick.AddListener(() => StartCoroutine(StartGameRoutine(1)));
        quitButton.onClick.AddListener(QuitGame);

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.gameObject.SetActive(false);
        }
    }

    private IEnumerator StartGameRoutine(int sceneIndex)
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);

            float elapsed = 0f;
            Color c = fadeImage.color;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                c.a = Mathf.Clamp01(elapsed / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }

            yield return new WaitForSeconds(delayBeforeLoad);
        }

        int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (previousSceneIndex >= 0)
        {
            StartCoroutine(StartGameRoutine(previousSceneIndex));
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Has Quit Game");
    }
}