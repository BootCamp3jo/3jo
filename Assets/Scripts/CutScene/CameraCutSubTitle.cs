using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraCutSubTitle : MonoBehaviour
{
    public TextMeshProUGUI subtitleText;
    public float fadeTime = 3f;
    Coroutine fadeCorWrap;

    public void ChangeText(string value)
    {
        subtitleText.text = value;
        if (fadeCorWrap != null)
        {
            StopCoroutine(fadeCorWrap);
        }
        fadeCorWrap = StartCoroutine(FadeSubTitle());
    }

    IEnumerator FadeSubTitle()
    {
        if (subtitleText == null || string.IsNullOrEmpty(subtitleText.text))
        {
            yield break;
        }

        float alpha = 0f;
        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            alpha = Mathf.Lerp(0f, 1f, timer / fadeTime);
            subtitleText.GetComponent<CanvasGroup>().alpha = alpha;
            yield return null;
        }

        yield return new WaitForSeconds(1);

        timer = 0f;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            subtitleText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, timer / fadeTime);
            yield return null;
        }
        subtitleText.GetComponent<CanvasGroup>().alpha = 0f;

        this.gameObject.SetActive(false);
    }
}
