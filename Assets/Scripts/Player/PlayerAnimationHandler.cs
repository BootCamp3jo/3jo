using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int isRunning;
    private int isAttackState;
    private int isFrontAttacking;
    private int isUpAttacking;
    private int isDownAttacking;

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
}
