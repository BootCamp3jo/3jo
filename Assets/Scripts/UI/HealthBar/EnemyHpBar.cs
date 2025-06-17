using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHpBar : MonoBehaviour
{
    [Header("UI 바")]
    [SerializeField] private Image mainBar; // 실제 체력 바
    [SerializeField] private RectTransform dropChunkContainer; // 조각이 생성될 부모
    [SerializeField] private GameObject dropChunkPrefab; // 떨어지는 조각 프리팹

    private float currentHpPercent = 1f; // 현재 체력 상태 (0~1)


    public void SetHp(float hpPercent)
    {
        hpPercent = Mathf.Clamp01(hpPercent);

        /*
        if (hpPercent < currentHpPercent)
        {
            float lostPercent = currentHpPercent - hpPercent;
            CreateDropChunk(lostPercent);
        }
        */

        currentHpPercent = hpPercent;
        mainBar.fillAmount = currentHpPercent;
    }

    private void CreateDropChunk(float lostPercent)
    {
        if (lostPercent <= 0f) return;

        GameObject chunk = Instantiate(dropChunkPrefab, dropChunkContainer);
        Image chunkImage = chunk.GetComponent<Image>();
        RectTransform chunkRect = chunk.GetComponent<RectTransform>();

        // 체력바 너비
        float totalWidth = mainBar.rectTransform.rect.width;

        float lostWidth = totalWidth * lostPercent;

        float fillEdgeX = totalWidth * currentHpPercent;
        float pivotOffset = (mainBar.rectTransform.pivot.x - 0.5f) * totalWidth;
        float posX = fillEdgeX + pivotOffset;

        chunkRect.anchoredPosition = new Vector2(posX, 0f);
        chunkRect.sizeDelta = new Vector2(lostWidth, chunkRect.sizeDelta.y);

        Color startColor = chunkImage.color;
        startColor.a = 1f;
        chunkImage.color = startColor;

        float fallDistance = 50f;
        float animDuration = 0.5f;

        chunkRect.DOAnchorPosY(chunkRect.anchoredPosition.y - fallDistance, animDuration)
            .SetEase(Ease.OutQuad);

        chunkImage.DOFade(0f, animDuration).OnComplete(() =>
        {
            Destroy(chunk);
        });
    }
}
