using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCoolTimeHandler : MonoBehaviour
{
    // 스킬 CoolTime 관련
    public bool canUseSkillA = true;
    public bool canUseSkillS = true;
    public bool canUseSkillD = true;
    public bool canUseSkillW = true;
    public bool canUseUltSkill = true;
  
    public float skillACoolTime;
    public float skillSCoolTime;
    public float skillDCoolTime;
    public float skillWCoolTime;
    public float ultSkillCoolTime;
    
    public Coroutine skillACoolTimeCoroutine;
    public Coroutine skillSCoolTimeCoroutine;
    public Coroutine skillDCoolTimeCoroutine;
    public Coroutine skillWCoolTimeCoroutine;
    public Coroutine ultSkillCoolTimeCoroutine;
 
    public WaitForSeconds waitForSeconds_ASkill;
    public WaitForSeconds waitForSeconds_SSkill;
    public WaitForSeconds waitForSeconds_DSkill;
    public WaitForSeconds waitForSeconds_WSkill;
    public WaitForSeconds waitForSeconds_UltSkill;

    private void Start()
    {
        SkillCoolTimeInit();
        WaitForSecondsInit();
    }

    #region [Initialization Methods]
    private void SkillCoolTimeInit()
    {
        skillACoolTime = SkillManager.Instance.GetSkillData(0).coolDown;
        skillSCoolTime = SkillManager.Instance.GetSkillData(1).coolDown;
        skillDCoolTime = SkillManager.Instance.GetSkillData(2).coolDown;
        skillWCoolTime = SkillManager.Instance.GetSkillData(3).coolDown;
        ultSkillCoolTime = SkillManager.Instance.GetUltSkillData().coolDown;
    }

    private void WaitForSecondsInit()
    {
        waitForSeconds_ASkill = new WaitForSeconds(SkillManager.Instance.GetSkillData(0).coolDown);
        waitForSeconds_SSkill = new WaitForSeconds(SkillManager.Instance.GetSkillData(1).coolDown);
        waitForSeconds_DSkill = new WaitForSeconds(SkillManager.Instance.GetSkillData(2).coolDown);
        waitForSeconds_WSkill = new WaitForSeconds(SkillManager.Instance.GetSkillData(3).coolDown);
        waitForSeconds_UltSkill = new WaitForSeconds(SkillManager.Instance.GetUltSkillData().coolDown);
    }
    #endregion

    public IEnumerator SkillCoolTime(float coolTime, Action<bool> canUseSkill, WaitForSeconds waitForSeconds)
    {
        Debug.Log("Cooldown started for skill with cool time: " + coolTime);
        canUseSkill(false);
        yield return waitForSeconds;
        canUseSkill(true);
    }
}
