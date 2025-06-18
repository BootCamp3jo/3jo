using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeSlotData : SkillSlotData
{
    [SerializeField] private Button skillTreeSlotButton;
    

    private void Start()
    {
        skillTreeSlotButton = GetComponent<Button>();
    }

    private void Update()
    {
        if (skillData == null) return;

        if (IsSkillUnlockable())
        {
            // 스킬이 언락 가능한 상태가 되었을 때 : 
            // 버튼 컴포넌트 켜주기 && 잠금 아이콘 깜빡거리기
            SkillTreeSlotButtonOn();
            SkillManager.Instance.LockIconBlinking(this);
        }
        if (!IsSkillUnlockable())
        {
            // lockIcon 깜빡임 중지 && 알파값 1로 변경
            SkillTreeSlotButtonOff();
            SkillManager.Instance.StopLockIconBlinking(this);
        }
    }

    public void OnSkillTreeButton()
    {
        if (PlayerManager.Instance.playerData.SkillPoint < skillData.skillPointCost)
        {
            return;
        }

        // 이 스킬슬롯 데이터가 3번째 업그레이드 가능한 스킬인지 확인
        if (SkillManager.Instance.skillTreeDataSlotManager.IsThis3rdSkill(this))
        {
            // 3번째 업그레이드 가능한 스킬이라면, 업그레이드 전 베이스 스킬이 언락되었는지 확인
            if (!SkillManager.Instance.skillTreeDataSlotManager.Is2ndSkillUnlocked())
            {
                Debug.Log("SkillTreeDataSlotManager: Unlock the base skill first before upgrading!");
            }
            else
            {
                SkillManager.Instance.skillTreeDataSlotManager.Upgrade3rdSkill();
                UnlockSkill();
            }
            return;
        }

        UnlockSkill();
        invokeAfterUnlock?.Invoke();
        MatchingSkillSlotData.UnlockSkill();

        if (this == SkillManager.Instance.skillTreeDataSlotManager.GetUltSkillSlotData())
        {
            if (UIManager.Instance.ultGuageBarManager.CheckUltGuageConditions())
                matchingSkillSlotData.SkillIcon.sprite = UIManager.Instance.ultGuageBarManager.ultNotAvailable;
        }
    }

    public bool IsSkillUnlockable()
    {
        return !isUnlocked && (PlayerManager.Instance.playerData.SkillPoint >= skillData.skillPointCost);
    }

    public void SkillTreeSlotButtonOn()
    {
        // 버튼 컴포넌트 켜기
        skillTreeSlotButton.enabled = true;
    }

    public void SkillTreeSlotButtonOff()
    {
        // 버튼 컴포넌트 끄기
        skillTreeSlotButton.enabled = false;
    }

    
}
