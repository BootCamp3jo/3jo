using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerSkillHandler : MonoBehaviour
{
    [SerializeField] private PlayerAnimationHandler playerAnimationHandler;

    private SkillUIDataSlotManager skillUIDataSlotManager;
    private SkillCoolTimeHandler skillCoolTimeHandler;


    private bool isUsingSkill = false; // 스킬 애니메이션 관련 bool값

    private Coroutine aSkillCoroutine;
    private Coroutine sSkillCoroutine;
    private Coroutine dSkillCoroutine;
    private Coroutine wSkillCoroutine;
    private Coroutine ultSkillCoroutine;

    private void Start()
    {
        playerAnimationHandler = GetComponentInChildren<PlayerAnimationHandler>();
        skillUIDataSlotManager = SkillManager.Instance.skillUiDataSlotManager;
        skillCoolTimeHandler = SkillManager.Instance.skillCoolTimeHandler;
    }


    public void OnSkillButton(InputAction.CallbackContext context)
    {
        if (context.started && !isUsingSkill)
        {
            var control = context.control;

            // Level 01 skill 
            if (control == Keyboard.current.aKey)
            {
                EnterUsingSkillProcess(0,
                                       () => skillCoolTimeHandler.canUseSkillA,
                                       c => skillCoolTimeHandler.canUseSkillA = c, 
                                       ref skillCoolTimeHandler.skillACoolTimeCoroutine,
                                       skillCoolTimeHandler.skillACoolTime,
                                       skillCoolTimeHandler. waitForSeconds_ASkill, 
                                       UseSkillA);
            }

            // Level 02 skill
            else if (control == Keyboard.current.sKey)
            {
                EnterUsingSkillProcess(1,
                                       () => skillCoolTimeHandler.canUseSkillS,
                                       c => skillCoolTimeHandler.canUseSkillS = c,
                                       ref skillCoolTimeHandler.skillSCoolTimeCoroutine,
                                       skillCoolTimeHandler.skillSCoolTime,
                                       skillCoolTimeHandler.waitForSeconds_SSkill,
                                       UseSkillS);
            }

            // Level 03 skill
            else if (control == Keyboard.current.dKey)
            {
                EnterUsingSkillProcess(2,
                                       () => skillCoolTimeHandler.canUseSkillD,
                                       c => skillCoolTimeHandler.canUseSkillD = c,
                                       ref skillCoolTimeHandler.skillDCoolTimeCoroutine,
                                       skillCoolTimeHandler.skillDCoolTime,
                                       skillCoolTimeHandler.waitForSeconds_DSkill,
                                       UseSkillD);
            }

            // Level 04 skill
            else if (control == Keyboard.current.wKey)
            {
                EnterUsingSkillProcess(3,
                                       () => skillCoolTimeHandler.canUseSkillW, 
                                       c => skillCoolTimeHandler.canUseSkillW = c,
                                       ref skillCoolTimeHandler.skillWCoolTimeCoroutine,
                                       skillCoolTimeHandler.skillWCoolTime,
                                       skillCoolTimeHandler.waitForSeconds_WSkill,
                                       UseSkillW);
            }

            // Ultimate skill
            else if (control == Keyboard.current.qKey)
            {
                EnterUsingUltProcess(() => skillCoolTimeHandler.canUseUltSkill,
                                      c => skillCoolTimeHandler.canUseUltSkill = c,
                                      ref skillCoolTimeHandler.ultSkillCoolTimeCoroutine,
                                      skillCoolTimeHandler.ultSkillCoolTime,
                                      skillCoolTimeHandler.waitForSeconds_UltSkill,
                                      UseUltimateSkill);
            }
        }
    }

    //------------------------------------------//

    public void EnterUsingSkillProcess(int slotIndex,
                                      Func<bool> getCanUseSkill,      
                                      Action<bool> setCanUseSkill,
                                      ref Coroutine cooldownCoroutine,
                                      float cooldownTime,
                                      WaitForSeconds waitForSeconds,
                                      Action useSkillAction)
    {
        SkillSlotData skillSlotData = skillUIDataSlotManager.GetSkillSlotData(slotIndex);

        if (!skillUIDataSlotManager.IsSkillUnlocked(skillSlotData))
        {
            Debug.LogWarning($"Skill in slot {slotIndex} is not unlocked.");
            return;
        }

        if (!getCanUseSkill())
        {
            Debug.LogWarning($"Skill in slot {slotIndex} is on cooldown.");
            return;
        }

        if (cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);

        useSkillAction.Invoke();

        cooldownCoroutine = StartCoroutine(
                            skillCoolTimeHandler.SkillCoolTime(cooldownTime,
                                                               c => setCanUseSkill(c),
                                                               waitForSeconds));
    }

    public void EnterUsingUltProcess( Func<bool> getCanUseSkill,
                                      Action<bool> setCanUseSkill,
                                      ref Coroutine cooldownCoroutine,
                                      float cooldownTime,
                                      WaitForSeconds waitForSeconds,
                                      Action useSkillAction)
    {
        SkillSlotData skillSlotData = skillUIDataSlotManager.GetUltSkillSlotData();

        if (!skillUIDataSlotManager.IsSkillUnlocked(skillSlotData))
        {
            Debug.LogWarning($"Ult Skill is not unlocked.");
            return;
        }

        if (!getCanUseSkill())
        {
            Debug.LogWarning($"Ult Skill is on cooldown.");
            return;
        }

        if (cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);

        useSkillAction.Invoke();

        cooldownCoroutine = StartCoroutine(
                            skillCoolTimeHandler.SkillCoolTime(cooldownTime,
                                                               c => setCanUseSkill(c),
                                                               waitForSeconds));
    }


    //------------------------------------------//

    public void UseSkillA()
    {
        Debug.Log("Using Skill A");

        SkillManager.Instance.SummonSkillVFX(0);
    }

    public void UseSkillS()
    {
        SkillManager.Instance.SummonSkillVFX(1);
    }

    public void UseSkillD()
    {
        SkillManager.Instance.SummonSkillVFX(2);
    }

    public void UseSkillW()
    {
        SkillManager.Instance.SummonSkillVFX(3);
    }

    public void UseUltimateSkill()
    {
        if (ultSkillCoroutine != null)
            StopCoroutine(ultSkillCoroutine);

        ultSkillCoroutine = StartCoroutine(UseUltSkillMotion());
        SkillManager.Instance.SummonUltVFX();
    }

    //------------------------------------------//

    private IEnumerator UseUltSkillMotion()
    {
        isUsingSkill = true;
        playerAnimationHandler.EnterUsingSkill();
        playerAnimationHandler.EnterUsingUlt();
        yield return new WaitForSeconds(2f); // Adjust the duration as needed
        playerAnimationHandler.ExitUsingUlt();
        playerAnimationHandler.ExitUsingSkill();
        isUsingSkill = false;
    }
}
