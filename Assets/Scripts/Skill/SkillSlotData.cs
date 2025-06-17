using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotData : MonoBehaviour
{
    [SerializeField] protected BaseSkillData skillData;

    [SerializeField]
    protected Image _skillIcon;
    public Image SkillIcon
    {
        get { return _skillIcon; }
        set { _skillIcon = value; }
    }


    [SerializeField] protected float cooldownTime;
    [SerializeField] public bool isUnlocked = false;

    public void SetSkillToSlot(BaseSkillData skillData)
    {
        this.skillData = skillData;
        SkillIcon.sprite = skillData.icon;
    }

    public BaseSkillData GetSkillDataFromSlot()
    {
        return skillData;
    }
}
