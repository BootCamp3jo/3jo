using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeSlotData : SkillSlotData
{
    private void Update()
    {
        if (IsSkillUnlockable())
            SkillManager.Instance.LockIconBlinking(this);
    }

    public bool IsSkillUnlockable()
    {
        return !isUnlocked && (PlayerManager.Instance.playerData.SkillPoint >= skillData.skillPointCost);
    }
}
