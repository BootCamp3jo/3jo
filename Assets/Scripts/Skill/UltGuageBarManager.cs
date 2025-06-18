using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UltGuageBarManager : MonoBehaviour
{
    [Header("UI 바")]
    [SerializeField] private UltSkillData ultSkillData;        // 스킬 데이터 (필요시)
    [SerializeField] private GameObject ultGuageBarWrapper;    // 궁극기 게이지 바 프리팹 (필요시)
    [SerializeField] private Image mainBar;                    // 실제 체력 바
    [SerializeField] public Sprite ultNotAvailable;            // 궁극기 바가 다 차지 않았을 때 표시할 이미지

    private float currentUltGuagePercent = 0f; // 현재 궁극기 차오름 상태 (0~1)

    private void Awake()
    {
        // mainBar 자동 할당 (옵션)
        if (mainBar == null)
        {
            mainBar = gameObject.transform.GetChild(0).GetChild(1).GetComponent<Image>();
            if (mainBar == null)
                Debug.LogError("mainBar가 설정되지 않았고 자동으로도 찾을 수 없습니다.");
        }

        // ultGuageBarWrapper 자동 할당 (옵션)
        if (ultGuageBarWrapper == null)
        {
            ultGuageBarWrapper = gameObject.transform.GetChild(0).gameObject;
            if (ultGuageBarWrapper == null)
                Debug.LogError("ultGuageBarWrapper가 설정되지 않았고 자동으로도 찾을 수 없습니다.");
        }
    }

    private void Start()
    {
        ActivateUltGuageBar();
        SetUltAvailabilitySprite();
        mainBar.fillAmount = currentUltGuagePercent;
    }

    private void Update()
    {
        ActivateUltGuageBar();
    }

    public void IncreaseUltGuage(float chargePercent)
    {
        chargePercent = Mathf.Clamp01(chargePercent);

        // 현재 궁극기 게이지가 가득 찼다면 증가하지 않음
        if (currentUltGuagePercent == 1f) return;

        currentUltGuagePercent += chargePercent;
        mainBar.fillAmount = currentUltGuagePercent;

        // 궁극기 게이지에 따라 궁극기 아이콘을 변경 (궁 사용 가능 / 불가능)
        SetUltAvailabilitySprite();

        Debug.Log($"UltGuageBarManager: 현재 궁극기 게이지 상태: {currentUltGuagePercent * 100f}%");
    }

    public void SetUltAvailabilitySprite()
    {
        if (!CheckUltGuageConditions()) return;

        // 궁극기 게이지가 가득 찼을 때 스프라이트 변경
        if (currentUltGuagePercent >= 1f)
        {
            // 원래 궁극기 아이콘으로 변경
            SkillManager.Instance.skillUiDataSlotManager.GetUltSkillSlotData().transform.GetChild(1).GetComponent<Image>().sprite = ultSkillData.icon;
        }
        else if (currentUltGuagePercent < 1f)
        {
            // 궁극기 바가 다 차지 않았을 때 표시할 이미지로 변경
            SkillManager.Instance.skillUiDataSlotManager.GetUltSkillSlotData().transform.GetChild(1).GetComponent<Image>().sprite = ultNotAvailable;
            Debug.Log("UltGuageBarManager: 궁극기 게이지가 아직 다 차지 않았습니다.");
        }
    }

    public void ActivateUltGuageBar()
    {
        if (CheckUltGuageConditions()) ultGuageBarWrapper.SetActive(true);
        else ultGuageBarWrapper.SetActive(false);
    }

    public bool CheckUltGuageConditions()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        return (!(currentScene == "LobbyScene") && PlayerManager.Instance.playerData.skillDatas[5].isUnlock);
    }

    public bool CanUltGuageBeUsed()
    {
        // 궁극기 게이지가 가득 찼는지 확인하는 로직
        return currentUltGuagePercent >= 1f;
    }

    public bool IsUltGuageActive()
    {
        // 궁극기 게이지가 하이어라키 내에 활성화 상태인지 확인하는 로직
        return ultGuageBarWrapper.activeSelf;
    }

    public void ResetUltGuage()
    {
        // 궁극기 게이지를 소모하는 로직
        if (currentUltGuagePercent >= 1f)
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
