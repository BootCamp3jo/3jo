using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillHandler : MonoBehaviour
{
    [SerializeField] private PlayerAnimationHandler playerAnimationHandler;

    private bool isUsingSkill = false;

    private Coroutine aSkillCoroutine;
    private Coroutine sSkillCoroutine;
    private Coroutine dSkillCoroutine;
    private Coroutine wSkillCoroutine;
    private Coroutine ultSkillCoroutine;

    private void Start()
    {
        playerAnimationHandler = GetComponentInChildren<PlayerAnimationHandler>();
    }


    public void OnSkillButton(InputAction.CallbackContext context)
    {
        if (context.started && !isUsingSkill)
        {
            var control = context.control;

            // Level 01 skill 
            if (control == Keyboard.current.aKey)
            {
                Debug.Log("A Key Skill Activated");
                UseSkillA();
            }

            // Level 02 skill
            else if (control == Keyboard.current.sKey)
            {
                Debug.Log("S Key Skill Activated");
                UseSkillS();
            }

            // Level 03 skill
            else if (control == Keyboard.current.dKey)
            {
                Debug.Log("D Key Skill Activated");
                UseSkillD();
            }

            // Level 04 skill
            else if (control == Keyboard.current.wKey)
            {
                Debug.Log("W Key Skill Activated");
                UseSkillW();
            }   

            // Ultimate skill
            else if (control == Keyboard.current.qKey)
            {
                Debug.Log("Ultimate Key Skill Activated");
                UseUltimateSkill();
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
