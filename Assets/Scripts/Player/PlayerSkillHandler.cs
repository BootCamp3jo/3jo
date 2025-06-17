using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillHandler : MonoBehaviour
{
    [SerializeField] private PlayerAnimationHandler playerAnimationHandler;

    private SkillUIDataSlotManager skillUIDataSlotManager;

    private bool isUsingSkill = false;

    private Coroutine aSkillCoroutine;
    private Coroutine sSkillCoroutine;
    private Coroutine dSkillCoroutine;
    private Coroutine wSkillCoroutine;
    private Coroutine ultSkillCoroutine;

    private void Start()
    {
        playerAnimationHandler = GetComponentInChildren<PlayerAnimationHandler>();
        skillUIDataSlotManager = SkillManager.Instance.skillUiDataSlotManager;
    }


    public void OnSkillButton(InputAction.CallbackContext context)
    {
        if (context.started && !isUsingSkill)
        {
            var control = context.control;

            // Level 01 skill 
            if (control == Keyboard.current.aKey)
            {
                SkillSlotData skillSlotData = skillUIDataSlotManager.GetSkillSlotData(0);

                if (skillUIDataSlotManager.IsSkillUnlocked(skillSlotData))
                {
                    UseSkillA();
                }
                else Debug.LogWarning("A skill is not unlocked yet.");
            }

            // Level 02 skill
            else if (control == Keyboard.current.sKey)
            {
                SkillSlotData skillSlotData = skillUIDataSlotManager.GetSkillSlotData(1);

                if (skillUIDataSlotManager.IsSkillUnlocked(skillSlotData))
                {
                    UseSkillS();
                }
                else Debug.LogWarning("S skill is not unlocked yet.");
            }

            // Level 03 skill
            else if (control == Keyboard.current.dKey)
            {
                SkillSlotData skillSlotData = skillUIDataSlotManager.GetSkillSlotData(2);

                if (skillUIDataSlotManager.IsSkillUnlocked(skillSlotData))
                {
                    UseSkillD();
                }
                else Debug.LogWarning("D skill is not unlocked yet.");
            }

            // Level 04 skill
            else if (control == Keyboard.current.wKey)
            {
                SkillSlotData skillSlotData = skillUIDataSlotManager.GetSkillSlotData(3);

                if (skillUIDataSlotManager.IsSkillUnlocked(skillSlotData))
                {
                    UseSkillW();
                }
                else Debug.LogWarning("W skill is not unlocked yet.");                    
            }   

            // Ultimate skill
            else if (control == Keyboard.current.qKey)
            {
                SkillSlotData skillSlotData = skillUIDataSlotManager.GetUltSkillSlotData();

                if (skillUIDataSlotManager.IsUltSkillUnlocked())
                {
                    UseUltimateSkill();
                }
                else Debug.LogWarning("Ultimate skill is not unlocked yet.");
            }
        }
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
