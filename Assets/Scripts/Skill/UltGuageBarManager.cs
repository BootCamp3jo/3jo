using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UltGuageBarManager : MonoBehaviour
{
    [Header("UI 바")]
    [SerializeField] private UltSkillData ultSkillData;         // 스킬 데이터 (필요시)
    [SerializeField] private Image mainBar;                    // 실제 체력 바

    private float currentUltGuagePercent = 0f; // 현재 궁극기 차오름 상태 (0~1)

    private void Awake()
    {
        // mainBar 자동 할당 (옵션)
        if (mainBar == null)
        {
            mainBar = gameObject.transform.GetChild(1).GetComponent<Image>();
            if (mainBar == null)
                Debug.LogError("mainBar가 설정되지 않았고 자동으로도 찾을 수 없습니다.");
        }
    }

    private void Start()
    {
        mainBar.fillAmount = currentUltGuagePercent;
    }

    public void IncreaseUltGuage(float chargePercent)
    {
        chargePercent = Mathf.Clamp01(chargePercent);

        // 현재 궁극기 게이지가 가득 찼다면 증가하지 않음
        if (currentUltGuagePercent == 1f) return;

        currentUltGuagePercent += chargePercent;
        mainBar.fillAmount = currentUltGuagePercent;

        Debug.Log($"UltGuageBarManager: 현재 궁극기 게이지 상태: {currentUltGuagePercent * 100f}%");
    }

    public bool IsUltGuageActive()
    {
        // 궁극기 게이지가 하이어라키 내에 활성화 상태인지 확인하는 로직
        return gameObject.activeSelf;
    }

    public void DrainUltGuage()
    {
        // 궁극기 게이지를 소모하는 로직
        if (currentUltGuagePercent == 1f)
        {
            currentUltGuagePercent = 0f;
            mainBar.fillAmount = currentUltGuagePercent;
            Debug.Log("UltGuageBarManager: 궁극기 게이지가 소모되었습니다.");
        }
        else
        {
            Debug.LogWarning("UltGuageBarManager: 궁극기 게이지가 이미 비어 있습니다.");
        }
    }
}
