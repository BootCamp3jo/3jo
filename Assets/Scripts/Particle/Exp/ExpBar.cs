using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private Image expBarImg;
    [SerializeField] private ParticleSystem expParticlePrefab;

    // 새로 추가: 배경 이미지 (또는 배경 오브젝트)
    [SerializeField] private RectTransform backgroundRect;

    private int maxExp = 1000;
    private int nowExp = 0;

    private Vector3 expBarOriginalScale;
    private Vector3 bgOriginalScale;

    private void Awake()
    {
        expBarOriginalScale = expBarImg.rectTransform.localScale;

        if (backgroundRect != null)
            bgOriginalScale = backgroundRect.localScale;
        else
            bgOriginalScale = Vector3.one;

        UpdateBar();
    }
    private void Start()
    {
        
    }

    void OnEnable()
    {
        ExpManager.OnExpGained += AddExp;
    }

    void OnDisable()
    {
        ExpManager.OnExpGained -= AddExp;
    }

    void AddExp(int amount)
    {
        nowExp += amount;
        nowExp = Mathf.Min(nowExp, maxExp);
        UpdateBar();
        SpawnParticleAt();
        AnimateBarPop();
    }

    void UpdateBar()
    {
        expBarImg.fillAmount = (float)nowExp / maxExp;
    }


    // 애니메이션 바 효과
    void AnimateBarPop()
    {
        RectTransform rt = expBarImg.rectTransform;
        rt.DOKill();
        rt.localScale = expBarOriginalScale;

        // expBar 애니메이션
        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOScale(expBarOriginalScale * 1.1f, 0.1f).SetEase(Ease.OutQuad));

        if (backgroundRect != null)
        {
            backgroundRect.DOKill();
            backgroundRect.localScale = bgOriginalScale;

            // 배경과 동시에 커졌다 작아지기 (동기화)
            seq.Join(backgroundRect.DOScale(bgOriginalScale * 1.1f, 0.1f).SetEase(Ease.OutQuad));
            seq.Append(rt.DOScale(expBarOriginalScale, 0.15f).SetEase(Ease.InQuad));
            seq.Join(backgroundRect.DOScale(bgOriginalScale, 0.15f).SetEase(Ease.InQuad));
        }
        else
        {
            seq.Append(rt.DOScale(expBarOriginalScale, 0.15f).SetEase(Ease.InQuad));
        }
    }

    // 경험치 파티클 생성
    void SpawnParticleAt()
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

        Instantiate(expParticlePrefab, worldPos, Quaternion.identity);
    }
}
