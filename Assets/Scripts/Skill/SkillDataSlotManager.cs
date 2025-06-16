using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class SkillDataSlotManager : MonoBehaviour
{
    public SkillSlotData ultSkillSlotData;

    private int skillSlotDataCounts = 4; 

    List<SkillSlotData> skilSlotDatas;

    // Start is called before the first frame update
    void Start()
    {
        SkillDataSlotInit();
        SkillsToSlotInit();
        UltSkillInit(); 
    }

    public void SkillDataSlotInit()
    {
        skilSlotDatas = new List<SkillSlotData>();

        Transform parent = transform;

        for (int i = 0; i < skillSlotDataCounts; i++)
        {
            Transform child = parent.GetChild(i);
            SkillSlotData skillDataSlot = child.GetComponent<SkillSlotData>();

            if (skillDataSlot != null)
            {
                skilSlotDatas.Add(skillDataSlot);
            }
#if UNITY_EDITOR
            else Debug.LogError("SkillDataSlotManager: Child at index " + i + " does not have a SkillSlotData component.");
#endif
        }

        Debug.Log("SkillDataSlotManager: Initialized " + skilSlotDatas.Count + " skill data slots.");
    }

    public void SkillsToSlotInit()
    {
        for (int i = 0; i < skillSlotDataCounts; i++)
        {
            skilSlotDatas[i].SetSkillToSlot(SkillManager.Instance.basicSkillList[i]);
        }

        Debug.Log("SkillDataSlotManager: Assigned skills to slots.");
    }

    public void UltSkillInit()
    {
        ultSkillSlotData.SetSkillToSlot(SkillManager.Instance.ultimateSkillList[0]);
    }
}
