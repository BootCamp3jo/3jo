using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int isRunning;
    private int isAttackState;
    private int isFrontAttacking;
    private int isUpAttacking;
    private int isDownAttacking;

    private int isUsingSkill;
    private int usingSkillA;
    private int usingSkillS;
    private int usingSkillD;
    private int usingSkillW;
    private int usingUlt;

    //------------------------------------------//

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        isRunning = Animator.StringToHash("IsRunning");
        isAttackState = Animator.StringToHash("IsAttackState");
        isFrontAttacking = Animator.StringToHash("IsFrontAttacking");
        isUpAttacking = Animator.StringToHash("IsUpAttacking");
        isDownAttacking = Animator.StringToHash("IsDownAttacking");
        isUsingSkill = Animator.StringToHash("IsUsingSkill");
        usingSkillA = Animator.StringToHash("UsingSkillA");
        usingSkillS = Animator.StringToHash("UsingSkillS");
        usingSkillD = Animator.StringToHash("UsingSkillD");
        usingSkillW = Animator.StringToHash("UsingSkillW");
        usingUlt = Animator.StringToHash("UsingUlt");
    }

    //------------------------------------------//

    public void EnterRunning()
    {
        animator.SetBool(isRunning, true);
    }

    public void ExitRunning()
    {
        animator.SetBool(isRunning, false);
    }

    //------------------------------------------//

    public void EnterAttackingState()
    {
        animator.SetBool(isAttackState, true);
    }

    public void ExitAttackingState()
    {
        animator.SetBool(isAttackState, false);
    }

    //------------------------------------------//

    public void EnterFrontAttacking()
    {
        animator.SetBool(isFrontAttacking, true);
    }

    public void ExitFrontAttacking()
    {
        animator.SetBool(isFrontAttacking, false);
    }

    //------------------------------------------//

    public void EnterUpAttacking()
    {
        animator.SetBool(isUpAttacking, true); // 위쪽 공격은 별도의 상태로 처리
    }

    public void ExitUpAttacking()
    {
        animator.SetBool(isUpAttacking, false);
    }

    //------------------------------------------//

    public void EnterDownAttacking()
    {
        animator.SetBool(isDownAttacking, true); // 아래쪽 공격은 별도의 상태로 처리
    }

    public void ExitDownAttacking()
    {
        animator.SetBool(isDownAttacking, false);
    }

    //------------------------------------------//

    public void EnterUsingSkill()
    {
        animator.SetBool(isUsingSkill, true);
    }

    public void ExitUsingSkill()
    {
        animator.SetBool(isUsingSkill, false);
    }

    //------------------------------------------// 

    public void EnterUsingSkillA()
    {
        animator.SetBool(usingSkillA, true);
    }

    public void ExitUsingSkillA()
    {
        animator.SetBool(usingSkillA, false);
    }

    //------------------------------------------//

    public void EnterUsingSkillS()
    {
        animator.SetBool(usingSkillS, true);
    }

    public void ExitUsingSkillS()
    {
        animator.SetBool(usingSkillS, false);
    }

    //------------------------------------------//

    public void EnterUsingSkillD()
    {
        animator.SetBool(usingSkillD, true);
    }

    public void ExitUsingSkillD()
    {
        animator.SetBool(usingSkillD, false);
    }

    //------------------------------------------//

    public void EnterUsingSkillW()
    {
        animator.SetBool(usingSkillW, true);
    }

    public void ExitUsingSkillW()
    {
        animator.SetBool(usingSkillW, false);
    }

    //------------------------------------------//

    public void EnterUsingUlt()
    {
        animator.SetBool(usingUlt, true);
    }

    public void ExitUsingUlt()
    {
        animator.SetBool(usingUlt, false);
    }
}
