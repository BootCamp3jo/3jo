using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillHandler : MonoBehaviour
{
    [SerializeField] private PlayerAnimationHandler playerAnimationHandler;

    private void Start()
    {
        playerAnimationHandler = PlayerManager.Instance.playerAnimationHandler;
    }


    public void OnSkillButton(InputAction.CallbackContext context)
    {
        if (context.started)
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
        // Implement skill A logic here
    }

    public void UseSkillS()
    {
        // Update logic if needed
    }

    public void UseSkillD()
    {
        // Update logic if needed
    }

    public void UseSkillW()
    {
        // Update logic if needed
    }

    public void UseUltimateSkill()
    {
        Debug.Log("Using Ultimate Skill");

        UseUltSkillMotion();
        SummonUltSkillVFX();
    }

    //------------------------------------------//
    
    private IEnumerator UseUltSkillMotion()
    {
        playerAnimationHandler.EnterUsingSkill();
        playerAnimationHandler.EnterUsingUlt();
        yield return new WaitForSeconds(2f); // Adjust the duration as needed
        playerAnimationHandler.ExitUsingUlt();
        playerAnimationHandler.ExitUsingSkill();
    }

    private IEnumerator SummonUltSkillVFX()
    {
        SkillManager.Instance.ShowSkillVFX(5); // Assuming 0 is the index for the ultimate skill VFX
        yield return new WaitForSeconds(2f); // Adjust the duration as needed
        SkillManager.Instance.HideSkillVFX(5); // Hide the VFX after the skill duration
    }
}
