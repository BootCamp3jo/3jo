using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotData : MonoBehaviour
{
    [SerializeField] private BaseSkillData skillData;
    [SerializeField] private Image skillIcon;
    [SerializeField] private float cooldownTime;

    public void SetSkillToSlot(BaseSkillData skillData)
    {
        this.skillData = skillData;
        skillIcon.sprite = skillData.icon;
    }
}
