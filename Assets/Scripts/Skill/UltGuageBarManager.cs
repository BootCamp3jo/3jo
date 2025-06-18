using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UltGuageBarManager : MonoBehaviour
{
    [Header("UI 바")]
    [SerializeField] private UltSkillData ultSkillData;         // 스킬 데이터 (필요시)
    [SerializeField] private Image mainBar;                    // 실제 체력 바

    private float currentUltGuagePercent = 1f; // 현재 체력 상태 (0~1)

    private void Awake()
    {
        // mainBar 자동 할당 (옵션)
        if (mainBar == null)
        {
            mainBar = GetComponentInChildren<Image>();
            if (mainBar == null)
                Debug.LogError("mainBar가 설정되지 않았고 자동으로도 찾을 수 없습니다.");
        }
    }

    public void IncreaseUltGuage(float chargePercent)
    {
        chargePercent = Mathf.Clamp01(chargePercent);

        if (chargePercent < currentUltGuagePercent)
        {
            float lostPercent = currentUltGuagePercent + chargePercent;
        }

        currentUltGuagePercent = chargePercent;
        mainBar.fillAmount = currentUltGuagePercent;

        Debug.Log($"UltGuageBarManager: 현재 궁극기 게이지 상태: {currentUltGuagePercent * 100f}%");
    }
}
