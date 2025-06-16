using UnityEngine;
using UnityEngine.UI;

public class HealthBarFade : MonoBehaviour
{
    [SerializeField] private Image foregroundBar;
    [SerializeField] private Image delayedBar;

    private float currentHpPercent = 1f;
    private float delayedHpPercent = 1f;

    private float delayTimer = 0f;
   [SerializeField] private float delayTime = 0.5f;
    [SerializeField] private float reduceSpeed = 0.5f;

    private bool isReducing = false;

    public void SetHp(float hpPercent)
    {
        hpPercent = Mathf.Clamp01(hpPercent);

        if (hpPercent > currentHpPercent)
        {
            // 회복: 빨간색만 즉시 상승
            currentHpPercent = hpPercent;
            foregroundBar.fillAmount = currentHpPercent;

            // 회색바가 너무 내려가지 않도록 제한
            if (delayedHpPercent < currentHpPercent)
            {
                delayedHpPercent = currentHpPercent;
                delayedBar.fillAmount = delayedHpPercent;
                isReducing = false; // 감소 중단
            }
        }
        else if (hpPercent < currentHpPercent)
        {
            // 감소: 빨간색은 즉시, 회색은 딜레이로
            currentHpPercent = hpPercent;
            foregroundBar.fillAmount = currentHpPercent;

            if (currentHpPercent < delayedHpPercent)
            {
                delayTimer = 0f;
                isReducing = true;
            }
        }
    }

    private void Update()
    {
        if (!isReducing) return;

        delayTimer += Time.deltaTime;
        if (delayTimer < delayTime) return;

        // 회색바가 빨간바보다 더 아래로 내려가면 안 됨
        delayedHpPercent = Mathf.MoveTowards(delayedHpPercent, currentHpPercent, reduceSpeed * Time.deltaTime);
        delayedHpPercent = Mathf.Max(delayedHpPercent, currentHpPercent);

        delayedBar.fillAmount = delayedHpPercent;

        if (Mathf.Approximately(delayedHpPercent, currentHpPercent))
        {
            isReducing = false;
        }
    }
}
