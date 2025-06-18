using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UltGuageBarManager : MonoBehaviour
{
    [Header("UI 바")]
    [SerializeField] private UltSkillData ultSkillData;         // 스킬 데이터 (필요시)
    [SerializeField] private Image mainBar;                    // 실제 체력 바
    [SerializeField] private GameObject dropChunkPrefab;       // 떨어지는 조각 프리팹

    private float currentUltGuagePercent = 1f; // 현재 체력 상태 (0~1)

    private void Awake()
    {
        // dropChunkPrefab이 연결되지 않았으면 Resources에서 자동 로드
        if (dropChunkPrefab == null)
        {
            dropChunkPrefab = Resources.Load<GameObject>("DropChunkPrefab");
            if (dropChunkPrefab == null)
            {
                Debug.LogError("DropChunkPrefab이 Resources 폴더에 없습니다. 경로: Resources/DropChunkPrefab");
            }
        }

        // mainBar 자동 할당 (옵션)
        if (mainBar == null)
        {
            mainBar = GetComponentInChildren<Image>();
            if (mainBar == null)
                Debug.LogError("mainBar가 설정되지 않았고 자동으로도 찾을 수 없습니다.");
        }
    }

    public void SetCharge(float charge)
    {
        charge = Mathf.Clamp01(charge);

        if (charge < currentUltGuagePercent)
        {
            float lostPercent = currentUltGuagePercent - charge;
            CreateDropChunk(lostPercent);
        }

        currentUltGuagePercent = charge;
        mainBar.fillAmount = currentUltGuagePercent;
    }

    private void CreateDropChunk(float lostPercent)
    {
        if (dropChunkPrefab == null || lostPercent <= 0f) return;

        GameObject chunk = Instantiate(dropChunkPrefab);
        chunk.transform.SetParent(mainBar.rectTransform, false); // 부모를 mainBar로 설정

        Image chunkImage = chunk.GetComponent<Image>();
        RectTransform chunkRect = chunk.GetComponent<RectTransform>();

        float totalWidth = mainBar.rectTransform.rect.width;
        float lostWidth = totalWidth * lostPercent;
        float fillX = totalWidth * currentUltGuagePercent;

        // 좌측 기준 앵커/피벗 고정
        chunkRect.anchorMin = new Vector2(0f, 0.5f);
        chunkRect.anchorMax = new Vector2(0f, 0.5f);
        chunkRect.pivot = new Vector2(0f, 0.5f);

        // ✅ 수정: 생성 위치를 lostWidth 만큼 왼쪽으로 이동
        chunkRect.anchoredPosition = new Vector2(fillX - lostWidth, 0f);
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
