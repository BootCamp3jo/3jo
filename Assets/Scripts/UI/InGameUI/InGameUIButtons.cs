using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIButtons : MonoBehaviour
{
    public Animator animator;

    [Header("Skill Buttons")]
    public Button ultSkillButton;

    public void OnUltSkillButton()
    {
        animator.Play("Ult_UltVFX");
    }
}
