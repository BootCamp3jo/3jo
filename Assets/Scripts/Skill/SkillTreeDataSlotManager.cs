using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class SkillTreeDataSlotManager : A_SkillDataSlotManager
{
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
}
