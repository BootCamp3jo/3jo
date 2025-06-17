using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class A_SkillDataSlotManager : MonoBehaviour
{
    [SerializeField]
    protected SkillSlotData ultSkillSlotData;

    [SerializeField]
    protected int skillSlotDataCounts;

    [SerializeField]
    protected Sprite lockedSkillIcon;

    [SerializeField]
    protected List<SkillSlotData> skillSlotDatas;

    // Start is called before the first frame update
    protected void Start()
    {
        SkillDataSlotInit();
        SkillsToSlotInit();
        isUnlockedSkillInit();
        UltSkillInit();
        isUnLockedUltSkillInit();
    }

    protected void SkillDataSlotInit()
    {
        skillSlotDatas = new List<SkillSlotData>();

        Transform parent = transform;

        for (int i = 0; i < skillSlotDataCounts; i++)
        {
            Transform child = parent.GetChild(i);
            SkillSlotData skillDataSlot = child.GetComponent<SkillSlotData>();

            if (skillDataSlot != null)
            {
                skillSlotDatas.Add(skillDataSlot);
            }
#if UNITY_EDITOR
            else Debug.LogError("SkillDataSlotManager: Child at index " + i + " does not have a SkillSlotData component.");
#endif
        }

        Debug.Log("SkillDataSlotManager: Initialized " + skillSlotDatas.Count + " skill data slots.");
    }

    protected virtual void SkillsToSlotInit()
    {
       
    }

    protected void UltSkillInit()
    {
        ultSkillSlotData.SetSkillToSlot(SkillManager.Instance.ultimateSkillList[0]);
    }

    protected void isUnlockedSkillInit()
    {
        for (int i = 0; i < skillSlotDatas.Count; i++)
        {
            if (!isSkillUnlocked(skillSlotDatas[i]))
            {
                SetLockIcon(skillSlotDatas[i]);
            }
            else
            {
                RemoveLockIcon(skillSlotDatas[i]);
            }
        }
    }

    protected void isUnLockedUltSkillInit()
    {
        if (!isUltSkillUnlocked())
            SetLockIcon(ultSkillSlotData);
     
        else RemoveLockIcon(ultSkillSlotData);
    }

    public bool isUltSkillUnlocked()
    {
        if (ultSkillSlotData == null)
        {
#if UnityEditor
            Debug.LogError("SkillDataSlotManager: Ultimate skill slot data is not set.");
#endif
            return false;
        }

        if (!ultSkillSlotData.isUnlocked) return false;
        return true;
    }

    public bool isSkillUnlocked(SkillSlotData skillSlotData)
    {  
        if (!skillSlotData.isUnlocked) return false;

        return true;
    }

    private void SetLockIcon(SkillSlotData skillSlotData)
    {
        skillSlotData.skillIcon.sprite = lockedSkillIcon;
    }

    private void RemoveLockIcon(SkillSlotData skillSlotData)
    {
        if (skillSlotData.GetSkillDataFromSlot() == null)
        {
#if UNITY_EDITOR
            Debug.LogError("SkillDataSlotManager: Skill data is null, cannot remove unlock icon.");
#endif
            return;
        }

        skillSlotData.skillIcon.sprite = skillSlotData.GetSkillDataFromSlot().icon;
    }

    public SkillSlotData GetUltSkillSlotData()
    {
        if (ultSkillSlotData == null)
        {
            Debug.LogError("SkillDataSlotManager: Ultimate skill slot data is not set.");
            return null;
        }
        return ultSkillSlotData;
    }

    public SkillSlotData GetSkillSlotData(int index)
    {
        if (index < 0 || index >= skillSlotDatas.Count)
        {
            Debug.LogError("SkillDataSlotManager: Index out of range.");
            return null;
        }
        return skillSlotDatas[index];
    }
}
