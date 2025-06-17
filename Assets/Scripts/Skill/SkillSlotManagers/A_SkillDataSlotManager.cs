using System;
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
    public Sprite LockedSkillIcon
    {
        get { return lockedSkillIcon; }
        set { lockedSkillIcon = value; }
    }

    public Action actionFor3rdSkillUpgrade;

    [SerializeField]
    protected List<SkillSlotData> skillSlotDatas;
    protected List<SkillData> skillDatas;

    // Start is called before the first frame update
    protected void OnEnable()
    {
        SkillDataSlotInit();
        SkillsToSlotInit();
        isUnlockedSkillInit();
        UltSkillInit();
        isUnLockedUltSkillInit();
    }

    // --------------- Initialization Methods ------------------

    protected void SkillDataSlotInit()
    {
        skillDatas = DataManager.Instance.gameContext.saveData.playerData.skillDatas;
        skillSlotDatas = new List<SkillSlotData>();

        Transform parent = transform;

        for (int i = 0; i < skillSlotDataCounts; i++)
        {
            Transform child = parent.GetChild(i);
            SkillSlotData skillDataSlot = child.GetComponent<SkillSlotData>();
            int index = i;

            if (skillDataSlot != null)
            {
                skillSlotDatas.Add(skillDataSlot);
                if (skillDatas[i].isUnlock)
                {
                    skillSlotDatas[i].UnlockSkillWithoutReducePoint();
                }
                skillSlotDatas[index].invokeAfterUnlock = () =>
                {
                    skillDatas[index].isUnlock = true;
                };
            }
#if UNITY_EDITOR
            else Debug.LogError("SkillDataSlotManager: Child at index " + i + " does not have a SkillSlotData component.");
#endif
        }

        if(ultSkillSlotData != null)
        {
            if (skillDatas[5].isUnlock)
            {
                ultSkillSlotData.UnlockSkillWithoutReducePoint();
            }
            ultSkillSlotData.invokeAfterUnlock = () =>
            {
                skillDatas[5].isUnlock = true;
            };
        }

        if (skillDatas[2].isUnlock && skillDatas[2].isUpgraded)
        {
            actionFor3rdSkillUpgrade?.Invoke();
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
            if (!IsSkillUnlocked(skillSlotDatas[i]))
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
        if (!IsUltSkillUnlocked())
            SetLockIcon(ultSkillSlotData);

        else RemoveLockIcon(ultSkillSlotData);
    }

    // --------------- Skill Unlock state check ----------------

    public bool IsUltSkillUnlocked()
    {
        if (ultSkillSlotData == null)
        {
#if UNITY_EDITOR
            Debug.LogError("SkillDataSlotManager: Ultimate skill slot data is not set.");
#endif
            return false;
        }

        if (!ultSkillSlotData.isUnlocked) return false;
        return true;
    }

    public bool IsSkillUnlocked(SkillSlotData skillSlotData)
    {
        if (!skillSlotData.isUnlocked) return false;

        return true;
    }

    // --------------- Getters for Skill Slot Data ----------------

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

    // --------------- LockIcon setter & remover -------------------

    private void SetLockIcon(SkillSlotData skillSlotData)
    {
        skillSlotData.SkillIcon.sprite = lockedSkillIcon;
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

        skillSlotData.SkillIcon.sprite = skillSlotData.GetSkillDataFromSlot().icon;
    }
}
