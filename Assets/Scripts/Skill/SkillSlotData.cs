using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotData : MonoBehaviour
{
    [SerializeField] private BaseSkillData skillData;
    [SerializeField] public Image skillIcon;
    [SerializeField] private float cooldownTime;
    [SerializeField] public bool isUnlocked = false;

    public void SetSkillToSlot(BaseSkillData skillData)
    {
        this.skillData = skillData;
        skillIcon.sprite = skillData.icon;
    }

    public BaseSkillData GetSkillDataFromSlot()
    {
        return skillData;
    }
}
