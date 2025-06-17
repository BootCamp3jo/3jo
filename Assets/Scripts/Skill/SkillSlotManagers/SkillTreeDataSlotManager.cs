using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class SkillTreeDataSlotManager : A_SkillDataSlotManager
{
    private void Awake()
    {
        actionFor3rdSkillUpgrade = Upgrade3rdSkill;
    }
    protected override void SkillsToSlotInit()
    {
        for (int i = 0; i < skillSlotDataCounts; i++)
        {
            if (i == 3)
            {
                skillSlotDatas[3].SetSkillToSlot(SkillManager.Instance.upgradedSkillList[0]);
                continue;
            }

            if (i == 4)
            {
                skillSlotDatas[4].SetSkillToSlot(SkillManager.Instance.basicSkillList[SkillManager.Instance.basicSkillList.Count - 1]);
                continue;
            }

            skillSlotDatas[i].SetSkillToSlot(SkillManager.Instance.basicSkillList[i]);
        }

        Debug.Log("SkillDataSlotManager: Assigned skills to slots.");
    }


    public bool Is2ndSkillUnlocked()
    {
        if (IsSkillUnlocked(skillSlotDatas[2])) return true;
        else return false;
    }

    public bool IsThis3rdSkill(SkillTreeSlotData skillTreeSlot)
    {
        if (skillTreeSlot == skillSlotDatas[3]) return true;
        else return false;
    }

    public void Upgrade3rdSkill()
    {
        skillSlotDatas[3].MatchingSkillSlotData.SetSkillToSlot(SkillManager.Instance.upgradedSkillList[0]);
        skillDatas[2].isUpgraded = true;
        skillSlotDatas[3].UnlockSkill();
    }
}
