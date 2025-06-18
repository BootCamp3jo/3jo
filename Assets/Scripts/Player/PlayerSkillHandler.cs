using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillHandler : MonoBehaviour
{
    [SerializeField] private PlayerAnimationHandler playerAnimationHandler;

    private SkillUIDataSlotManager skillUIDataSlotManager;
    private SkillCoolTimeHandler skillCoolTimeHandler;

    private bool isUsingSkill = false;

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

            if (control == Keyboard.current.aKey)
            {
                EnterUsingSkillProcess(0,
                                       () => skillCoolTimeHandler.canUseSkillA,
                                       c => skillCoolTimeHandler.canUseSkillA = c,
                                       ref skillCoolTimeHandler.skillACoolTimeCoroutine,
                                       skillCoolTimeHandler.skillACoolTime,
                                       skillCoolTimeHandler.GetSkillFillImageByIndex(0),
                                       UseSkillA);
            }
            else if (control == Keyboard.current.sKey)
            {
                EnterUsingSkillProcess(1,
                                       () => skillCoolTimeHandler.canUseSkillS,
                                       c => skillCoolTimeHandler.canUseSkillS = c,
                                       ref skillCoolTimeHandler.skillSCoolTimeCoroutine,
                                       skillCoolTimeHandler.skillSCoolTime,
                                       skillCoolTimeHandler.GetSkillFillImageByIndex(1),
                                       UseSkillS);
            }
            else if (control == Keyboard.current.dKey)
            {
                EnterUsingSkillProcess(2,
                                       () => skillCoolTimeHandler.canUseSkillD,
                                       c => skillCoolTimeHandler.canUseSkillD = c,
                                       ref skillCoolTimeHandler.skillDCoolTimeCoroutine,
                                       skillCoolTimeHandler.skillDCoolTime,
                                       skillCoolTimeHandler.GetSkillFillImageByIndex(2),
                                       UseSkillD);
            }
            else if (control == Keyboard.current.wKey)
            {
                EnterUsingSkillProcess(3,
                                       () => skillCoolTimeHandler.canUseSkillW,
                                       c => skillCoolTimeHandler.canUseSkillW = c,
                                       ref skillCoolTimeHandler.skillWCoolTimeCoroutine,
                                       skillCoolTimeHandler.skillWCoolTime,
                                       skillCoolTimeHandler.GetSkillFillImageByIndex(3),
                                       UseSkillW);
            }
            else if (control == Keyboard.current.qKey)
            {
                if (!UIManager.Instance.ultGuageBarManager.CanUltGuageBeUsed())
                {
                    Debug.LogWarning("Ultimate skill cannot be used yet. Fill up the ult guage bar first.");
                    return;
                }

                UIManager.Instance.ultGuageBarManager.ResetUltGuage();

                EnterUsingUltProcess(() => skillCoolTimeHandler.canUseUltSkill,
                                     c => skillCoolTimeHandler.canUseUltSkill = c,
                                     ref skillCoolTimeHandler.ultSkillCoolTimeCoroutine,
                                     skillCoolTimeHandler.ultSkillCoolTime,
                                     skillCoolTimeHandler.GetUltFillImage(),
                                     UseUltimateSkill);
            }
        }
    }

    public void EnterUsingSkillProcess(int slotIndex,
                                       Func<bool> getCanUseSkill,
                                       Action<bool> setCanUseSkill,
                                       ref Coroutine cooldownCoroutine,
                                       float cooldownTime,
                                       UnityEngine.UI.Image fillImage,
                                       Action useSkillAction)
    {
        SkillSlotData skillSlotData = skillUIDataSlotManager.GetSkillSlotData(slotIndex);

        if (!skillUIDataSlotManager.IsSkillUnlocked(skillSlotData)) return;
        if (!getCanUseSkill()) return;

        if (cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);

        useSkillAction.Invoke();

        cooldownCoroutine = StartCoroutine(
            skillCoolTimeHandler.SkillCoolTime(cooldownTime, setCanUseSkill, fillImage));
    }

    public void EnterUsingUltProcess(Func<bool> getCanUseSkill,
                                     Action<bool> setCanUseSkill,
                                     ref Coroutine cooldownCoroutine,
                                     float cooldownTime,
                                     UnityEngine.UI.Image fillImage,
                                     Action useSkillAction)
    {
        SkillSlotData skillSlotData = skillUIDataSlotManager.GetUltSkillSlotData();

        if (!skillUIDataSlotManager.IsSkillUnlocked(skillSlotData)) return;
        if (!getCanUseSkill()) return;

        if (cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);

        useSkillAction.Invoke();

        cooldownCoroutine = StartCoroutine(
            skillCoolTimeHandler.SkillCoolTime(cooldownTime, setCanUseSkill, fillImage));
    }

    public void UseSkillA() => SkillManager.Instance.SummonSkillVFX(0);
    public void UseSkillS() => SkillManager.Instance.SummonSkillVFX(1);
    public void UseSkillD() => SkillManager.Instance.SummonSkillVFX(2);
    public void UseSkillW() => SkillManager.Instance.SummonSkillVFX(3);

    public void UseUltimateSkill()
    {
        if (ultSkillCoroutine != null)
            StopCoroutine(ultSkillCoroutine);

        ultSkillCoroutine = StartCoroutine(UseUltSkillMotion());
        SkillManager.Instance.SummonUltVFX();
    }

    private IEnumerator UseUltSkillMotion()
    {
        isUsingSkill = true;
        playerAnimationHandler.EnterUsingSkill();
        playerAnimationHandler.EnterUsingUlt();
        yield return new WaitForSeconds(2f);
        playerAnimationHandler.ExitUsingUlt();
        playerAnimationHandler.ExitUsingSkill();
        isUsingSkill = false;
    }
}