using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoSingleton<SkillManager>
{
    [Header("SkillManager")]
    [SerializeField] private SkillDataSlotManager skillDataSlotManager;
    [SerializeField] private Transform VFXSpawnPoint;

    public List<BaseSkillData> basicSkillList;
    public List<BaseSkillData> upgradedSkillList;
    public List<BaseSkillData> ultimateSkillList;

    public void SummonSkillVFX(int skillIndex)
    {
        BaseSkillData currentSkillData = GetSkillData(skillIndex);

        if (currentSkillData != null)
        {
            GameObject skillVFX = Instantiate(currentSkillData.skillPrefab, VFXSpawnPoint.position, Quaternion.identity);
            skillVFX.transform.SetParent(VFXSpawnPoint);
            FlipVFX(skillVFX);
        }
        else
        {
            Debug.LogWarning("No current skill data set. Cannot summon VFX.");
        }
    }

    public void SummonUltVFX()
    {
        BaseSkillData ultSkillData = GetUltSkillData();

        if (ultSkillData != null)
        {
            GameObject ultVFX = Instantiate(ultSkillData.skillPrefab, VFXSpawnPoint.position, Quaternion.identity);
            ultVFX.transform.SetParent(VFXSpawnPoint);
            FlipVFX(ultVFX);
        }
        else
        {
            Debug.LogWarning("No ultimate skill data set. Cannot summon VFX.");
        }
    }

    public void FlipVFX(GameObject ultVFX)
    {
        Vector3 pivotLocalScale = VFXSpawnPoint.localScale;
        Vector3 localScale = ultVFX.transform.localScale;

        if (pivotLocalScale.x > 0)
            localScale.x = Mathf.Abs(localScale.x);
        else if (pivotLocalScale.x < 0)
            localScale.x = -Mathf.Abs(localScale.x);

        ultVFX.transform.localScale = localScale;
    }

    private BaseSkillData GetSkillData(int skillIndex)
    {
        return skillDataSlotManager.GetSkillSlotData(skillIndex).GetSkillDataFromSlot();
    }

    private BaseSkillData GetUltSkillData()
    {
        return skillDataSlotManager.ultSkillSlotData.GetSkillDataFromSlot();
    }
}


