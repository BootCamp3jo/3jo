using UnityEngine;
using System.Collections;

public class BlinkEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float blinkDuration = 0.5f; 
    [SerializeField] private float totalBlinkTime = 2f;
    [SerializeField] private float minAlpha = 0.5f;

    private Coroutine blinkRoutine;

    public void StartBlink(float duration)
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkCoroutine(duration));
    }

    private IEnumerator BlinkCoroutine(float duration)
    {
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsed < duration)
        {
            float t = 0f;

            while (t < blinkDuration && elapsed < duration)
            {
                float alpha = Mathf.Lerp(1f, minAlpha, Mathf.PingPong(t * 2f / blinkDuration, 1f));
                Color newColor = originalColor;
                newColor.a = alpha;
                spriteRenderer.color = newColor;

                t += Time.deltaTime;
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        spriteRenderer.color = originalColor;
        blinkRoutine = null;
    }
}
