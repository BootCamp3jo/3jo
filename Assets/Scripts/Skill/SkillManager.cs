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

    public void ShowSkillVFX(int skillIndex)
    {
        Transform selectedSkillVFX = transform.GetChild(skillIndex);
        selectedSkillVFX.gameObject.SetActive(true);
    }

    public void HideSkillVFX(int skillIndex)
    {
        Transform selectedSkillVFX = transform.GetChild(skillIndex);
        selectedSkillVFX.gameObject.SetActive(false);
    }
}
