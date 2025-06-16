using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class A_SkillDataSlotManager : MonoBehaviour
{
    [SerializeField]
    protected SkillSlotData ultSkillSlotData;

    [SerializeField]
    protected int skillSlotDataCounts;

    [SerializeField]
    protected List<SkillSlotData> skillSlotDatas;

    // Start is called before the first frame update
    protected void Start()
    {
        SkillDataSlotInit();
        SkillsToSlotInit();
        UltSkillInit();
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
