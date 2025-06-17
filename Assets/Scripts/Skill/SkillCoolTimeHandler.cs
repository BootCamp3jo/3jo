using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTimeHandler : MonoBehaviour
{
    // 스킬 사용 가능 여부 플래그
    public bool canUseSkillA = true;
    public bool canUseSkillS = true;
    public bool canUseSkillD = true;
    public bool canUseSkillW = true;
    public bool canUseUltSkill = true;

    // 각 스킬별 쿨타임 설정값
    public float skillACoolTime;
    public float skillSCoolTime;
    public float skillDCoolTime;
    public float skillWCoolTime;
    public float ultSkillCoolTime;

    // 각 스킬별 쿨타임 Coroutine 참조
    public Coroutine skillACoolTimeCoroutine;
    public Coroutine skillSCoolTimeCoroutine;
    public Coroutine skillDCoolTimeCoroutine; 
    public Coroutine skillWCoolTimeCoroutine;
    public Coroutine ultSkillCoolTimeCoroutine;

    // 각 스킬 쿨타임 표시용 이미지 (fillAmount로 사용)
    [SerializeField] private Image skillAImage;
    [SerializeField] private Image skillSImage;
    [SerializeField] private Image skillDImage;
    [SerializeField] private Image skillWImage;
    [SerializeField] private Image ultSkillImage;

    private void Start()
    {
        SkillCoolTimeInit();
    }

    #region [초기화 메서드]

    // SkillManager에서 각 스킬의 쿨타임 값을 가져옴
    private void SkillCoolTimeInit()
    {
        skillACoolTime = SkillManager.Instance.GetSkillData(0).coolDown;
        skillSCoolTime = SkillManager.Instance.GetSkillData(1).coolDown;
        skillDCoolTime = SkillManager.Instance.GetSkillData(2).coolDown;
        skillWCoolTime = SkillManager.Instance.GetSkillData(3).coolDown;
        ultSkillCoolTime = SkillManager.Instance.GetUltSkillData().coolDown;
    }

    public IEnumerator SkillCoolTime(float coolTime, Action<bool> setCanUseSkill, Image fillImage)
    {
        setCanUseSkill(false);

        float elapsed = 0f;
        fillImage.fillAmount = 0f; // 쿨타임 시작 시 비워진 상태

        while (elapsed < coolTime)
        {
            elapsed += Time.deltaTime;
            fillImage.fillAmount = elapsed / coolTime; // 점점 차오름
            yield return null;
        }

        fillImage.fillAmount = 1f; // 쿨타임 종료 시 가득 참
        setCanUseSkill(true);
    }

    public Image GetSkillFillImageByIndex(int index)
    {
        return index switch
        {
            0 => skillAImage,
            1 => skillSImage,
            2 => skillDImage,
            3 => skillWImage,
            _ => null
        };
    }

    public Image GetUltFillImage() => ultSkillImage;

    #endregion
}
