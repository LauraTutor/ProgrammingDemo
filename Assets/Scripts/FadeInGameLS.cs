using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInGameLS : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    private void Start()
    {
        if (fadeImage != null)
        {
            StartCoroutine(FadeOutAndDeactivate());
        }
    }

    private IEnumerator FadeOutAndDeactivate()
    {
        fadeImage.gameObject.SetActive(true);

        float elapsed = 0f;
        Color c = fadeImage.color;
        float startAlpha = c.a;

        // Fade alpha from 1 → 0
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // Ensure it’s fully transparent
        c.a = 0f;
        fadeImage.color = c;

        // Deactivate the image
        fadeImage.gameObject.SetActive(false);
    }
}