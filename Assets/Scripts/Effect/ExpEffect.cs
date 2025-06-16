using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpEffect : MonoBehaviour
{
    [SerializeField] private Image expBarImg;
    [SerializeField] private ParticleSystem expParticlePrefab;
    [SerializeField] private RectTransform backgroundRect;

    private Vector3 expBarOriginalScale;
    private Vector3 bgOriginalScale;

    private void Awake()
    {
        expBarOriginalScale = expBarImg.rectTransform.localScale;

        if (backgroundRect != null)
            bgOriginalScale = backgroundRect.localScale;
        else
            bgOriginalScale = Vector3.one;
    }
    public void AnimateBarPop()
    {
        RectTransform rt = expBarImg.rectTransform;
        rt.DOKill();
        rt.localScale = expBarOriginalScale;

        // expBar 애니메이션
        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOScale(expBarOriginalScale * 1.05f, 0.1f).SetEase(Ease.OutQuad));

        if (backgroundRect != null)
        {
            backgroundRect.DOKill();
            backgroundRect.localScale = bgOriginalScale;

            // 배경과 동시에 커졌다 작아지기 (동기화)
            seq.Join(backgroundRect.DOScale(bgOriginalScale * 1.05f, 0.1f).SetEase(Ease.OutQuad));
            seq.Append(rt.DOScale(expBarOriginalScale, 0.15f).SetEase(Ease.InQuad));
            seq.Join(backgroundRect.DOScale(bgOriginalScale, 0.15f).SetEase(Ease.InQuad));
        }
        else
        {
            seq.Append(rt.DOScale(expBarOriginalScale, 0.15f).SetEase(Ease.InQuad));
        }
    }

    public void SpawnParticleAt()
    {
        RectTransform rect = expBarImg.rectTransform;
        float fill = expBarImg.fillAmount;
        Vector2 barSize = rect.rect.size;

        Vector2 localPos = new Vector2(
            barSize.x * (fill - rect.pivot.x),
            barSize.y * (0.5f - rect.pivot.y)
        );

        Canvas canvas = rect.GetComponentInParent<Canvas>();
        Camera uiCamera = canvas.worldCamera;

        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, rect.TransformPoint(localPos));
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;

        ParticleSystem particle = Instantiate(expParticlePrefab, worldPos, Quaternion.identity);
        particle.transform.SetParent(expBarImg.transform);
    }
}
