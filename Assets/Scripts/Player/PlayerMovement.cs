using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.1f;

    private Vector2 moveInput;
    private int facingDirection; // 1: 오른쪽, -1: 왼쪽
    private bool isGrounded = true; // 점프 여부를 위한 변수\
    private bool isJumping = false;
    private bool isDoubleJumpReady = false;
    private bool isAttacking = false;

    private Coroutine attackCoroutine;
    private WaitForSeconds waitFor1_19sec;
    


    //----------------------------------------------------
    private void Start()
    {
        waitFor1_19sec = new WaitForSeconds(1.19f);    
    }

    void Update()
    {
        GetMoveInput();
        OnIdle();
    }

    //----------------------------------------------------

    public void OnIdle()
    {
        // 애니메이션 상태 Idle로 변경
        if (moveInput == Vector2.zero)
        {
            PlayerManager.Instance.playerAnimationHandler.ExitRunning();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // 이동 입력이 있을 때 애니메이션 상태 변경
        if (moveInput != Vector2.zero)
        {
            PlayerManager.Instance.playerAnimationHandler.EnterRunning();
            SetLookDirection();
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {   
        if (context.performed) moveSpeed *= 2;     // 달리기 로직 구현
        else if (context.canceled) moveSpeed /= 2; // 달리기 중지 로직 구현
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && !isJumping && isGrounded)
        {
            isJumping = true;
            isGrounded = false;

            
        }

    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking)
        {
            attackCoroutine = StartCoroutine(OnAttackCoroutine());
        }
    }

    private IEnumerator OnAttackCoroutine()
    {
        // 공격 애니메이션 재생 시간 동안 대기
        isAttacking = true;
        PlayerManager.Instance.playerAnimationHandler.EnterAttacking();
        yield return waitFor1_19sec; // 예시로 0.5초 대기
        PlayerManager.Instance.playerAnimationHandler.ExitAttacking();
        isAttacking = false;
    }

    private void GetMoveInput()
    {
        Vector2 move = new Vector2(moveInput.x, moveInput.y);
        transform.position += (Vector3)(move * moveSpeed * Time.deltaTime);
    }

    private void SetLookDirection()
    {
        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        if (horizontal > 0) facingDirection = 1;
        if (horizontal < 0) facingDirection = -1;

        transform.localScale = new Vector3(facingDirection, 1, 1);
    }

    private void PerformJump()
    {
        float targetY = transform.position.y + jumpHeight;

        // 점프 업
        transform.DOMoveY(targetY, jumpDuration / 2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                isDoubleJumpReady = true;
                if (isDoubleJumpReady)
                {

                }

                // 점프 다운
                transform.DOMoveY((targetY - jumpHeight), jumpDuration / 2f)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        isJumping = false;
                        isGrounded = true; // 점프가 끝나면 착지 상태로 변경
                    });
            });
    }
}
