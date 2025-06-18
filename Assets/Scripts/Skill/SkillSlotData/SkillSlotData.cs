using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotData : MonoBehaviour
{
    [SerializeField] protected BaseSkillData skillData;

    [SerializeField] protected SkillSlotData matchingSkillSlotData;

    public Action invokeAfterUnlock;

    public SkillSlotData MatchingSkillSlotData
    {
        get { return matchingSkillSlotData; }
        set { matchingSkillSlotData = value; }
    }

    [SerializeField]
    protected Image _skillIcon;
    public Image SkillIcon
    {
        get { return _skillIcon; }
        set { _skillIcon = value; }
    }


    [SerializeField] protected float cooldownTime;
    [SerializeField] public bool isUnlocked = false;

    public void SetSkillToSlot(BaseSkillData skillData)
    {
        this.skillData = skillData;
        SkillIcon.sprite = skillData.icon;
    }

    public BaseSkillData GetSkillDataFromSlot()
    {
        return skillData;
    }

    public void UnlockSkill()
    {
        isUnlocked = true;
        SkillIcon.sprite = skillData.icon;

        // 스킬 아이콘 투명도 1f로 변경
        SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 1f);

        // 스킬포인트에서 소모된 스킬포인트 차감
        PlayerManager.Instance.playerStatHandler.UseSkillPoint(skillData.skillPointCost);
    }

    public void UnlockSkillWithoutReducePoint()
    {
        isUnlocked = true;
        SkillIcon.sprite = skillData.icon;

        // 스킬 아이콘 투명도 1f로 변경
        SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 1f);

    }
}
