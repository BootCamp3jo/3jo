using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePanel : MonoBehaviour
{
    public Image image;
    public float fadeTime = 3f;
    Coroutine fadeCorWrap;

    public void Init(Sprite image)
    {
        this.image.sprite = image;

        if (fadeCorWrap != null)
        {
            StopCoroutine(fadeCorWrap);
        }
        fadeCorWrap = StartCoroutine(FadeCor());
    }

    IEnumerator FadeCor()
    {
        float alpha = 0f;
        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            alpha = Mathf.Lerp(0f, 1f, timer / fadeTime);
            image.GetComponent<CanvasGroup>().alpha = alpha;
            yield return null;
        }
    }
}
