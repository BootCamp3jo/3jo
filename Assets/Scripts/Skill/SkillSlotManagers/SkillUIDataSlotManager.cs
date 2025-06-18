using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class SkillUIDataSlotManager : A_SkillDataSlotManager
{
    protected override void SkillsToSlotInit()
    {
        base.SkillsToSlotInit();

        for (int i = 0; i < skillSlotDataCounts; i++)
        {
            skillSlotDatas[i].SetSkillToSlot(SkillManager.Instance.basicSkillList[i]);
        }
        if (skillDatas[2].isUnlock && skillDatas[2].isUpgraded)
        {
            skillSlotDatas[2].SetSkillToSlot(SkillManager.Instance.upgradedSkillList[0]);
        }

        Debug.Log("SkillDataSlotManager: Assigned skills to slots.");
    }
}
