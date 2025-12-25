using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BonusEngagedMessage : MonoBehaviour
{
    [SerializeField] private Text messageText;
    [SerializeField] private CanvasGroup canvasGroup;

    public void ShowMessage(string message)
    {
        messageText.text = message;
        StartCoroutine(ShowAndFade());
    }

    private IEnumerator ShowAndFade()
    {
        // Fade in
        yield return StartCoroutine(Fade(0f, 1f, 0.2f));

        // Hold
        yield return new WaitForSecondsRealtime(1.5f);

        // Fade out
        yield return StartCoroutine(Fade(1f, 0f, 0.5f));

        gameObject.SetActive(false);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }
}
