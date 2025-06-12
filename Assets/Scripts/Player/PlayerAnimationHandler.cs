using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int isRunning;
    private int isAttacking;

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
        isAttacking = Animator.StringToHash("IsAttacking");
    }

    public void EnterRunning()
    {
        animator.SetBool(isRunning, true);
    }

    public void ExitRunning()
    {
        animator.SetBool(isRunning, false);
    }

    public void EnterAttacking()
    {
        animator.SetBool(isAttacking, true);
    }

    public void ExitAttacking()
    {
        animator.SetBool(isAttacking, false);
    }
}
