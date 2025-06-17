using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Collections;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    private PlayerAnimationHandler playerAnimationHandler;

    [Header("Movement Settings")]
    [SerializeField] private float baseMoveSpeed = 3f; // 기본 이동 속도  
    [SerializeField] private float moveSpeedMultiplier = 2f; // 달리기 속도 배수

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.1f;

    [Header("Dash Settings")]
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashDuration = 0.2f; // 대시 지속 시간
    [SerializeField] private float dashCoolDown = 1f;
    [SerializeField] private float doubleTabThreshold = 0.3f;

    [Header("Skill Settings")]
    [SerializeField] private Transform skillVFXSpawnPoint; // 스킬 VFX 생성 위치

    // Move & Look
    private float moveSpeed;
    private Vector2 moveInput;
    private int facingDirection;      // 1: 오른쪽, -1: 왼쪽
    private enum LastDirection { Front, Up, Down }
    private LastDirection lastDirection = LastDirection.Front; // 마지막 공격 방향

    // Jump
    private float landingSpotY;       // 착지 위치 Y 좌표 (점프 후 착지 위치를 저장하기 위한 변수)
    private bool isGrounded = true;   // 점프 여부를 위한 변수\
    private bool isJumping = false;
    private bool isDoubleJumpReady = false;
    private bool hasDoubleJumped = false;
    private bool shouldResumeFallAfterDash = false; // 대시 후 낙하를 재개할지 여부
    private Tween activeJumpTween;
    private Tween activeFallTween;

    // Run & Dash
    private float lastShiftTabTime = -1f;
    private bool isRunning = false;   // 달리기 여부를 위한 변수
    private bool isDashing = false;   // 대시 여부를 위한 변수
    private bool canDash = true;
    private Vector2 lastDashDirection = Vector2.zero; // 마지막 대시 방향을 저장하기 위한 변수
    private Tween dashTween;
    private WaitForSeconds waitForDashCoolDown;

    private float justDodgeTime = 0.3f;
    private bool justDodgeWindow = false; // 대시 후 짧은 시간 동안 공격을 받을 시 저스트닷지를 발동시킬 수 있는 찰나
    private Coroutine justDodgeCoroutine;// 저스트닷지 코루틴을 저장하기 위한 변수
    private WaitForSeconds waitForDodgeTime;

    // Attack
    private bool isAttacking = false; // 공격 여부를 위한 변수
    private WaitForSeconds waitFor1_19sec;

    // Particle
    private PlayerEffectController playerEffectController; 
    [SerializeField] private Transform footPivot; // 발 위치
    [SerializeField] private float dustEmitInterval = 0.2f; // 이동 파티클 생성 주기
    private float lastDustTime = 0f;
    public bool JustDodgeWindow => justDodgeWindow; // 저스트닷지 불값 반환



    //----------------------------------------------------

    private void Start()
    {
        playerAnimationHandler = PlayerManager.Instance.playerAnimationHandler;
        playerEffectController = PlayerManager.Instance.playerEffectController;
        waitFor1_19sec = new WaitForSeconds(1.19f);
        waitForDashCoolDown = new WaitForSeconds(dashCoolDown);
        waitForDodgeTime = new WaitForSeconds(justDodgeTime);
        moveSpeed = baseMoveSpeed; // 초기 이동 속도 설정
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

        // 이동 입력이 있을 때 마지막 방향 추적 (공격모션 전환시 필요)
        TrackLastDirection();

        // 이동 입력이 있을 때 애니메이션 상태 변경
        if (moveInput != Vector2.zero)
        {
            playerAnimationHandler.EnterRunning();
            SetLookDirection();
            lastDashDirection = moveInput.normalized; // 마지막 대시 방향 업데이트 (Vector.zero일 때 바라보고 있는 방향으로 대시하기 위함)
        }

    }

    public void OnRunOrDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            float timeSinceLastTab = Time.time - lastShiftTabTime;

            if (timeSinceLastTab <= doubleTabThreshold && !isDashing)
            {
                // 더블 탭이 감지되면 대시 시작
                OnDash(context);
                lastShiftTabTime = -1f; // 대시 후 마지막 탭 시간 초기화
                isRunning = false;      // 대시 중에는 달리기 상태 해제
            }
            else
            {
                lastShiftTabTime = Time.time; // 마지막 탭 시간 업데이트
                isRunning = true; // 달리기 시작

                // 혹시모를 달리기 상태 전환에 대비
                OnRun(context);
            }
        }
        else if (context.performed && isRunning)
        {
            // 달리기 상태 시작
            OnRun(context); // 달리기 상태 해제
        }
        else if (context.canceled)
        {
            // 달리기 상태 해제
            isRunning = false;
            OnRun(context);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isGrounded && !isJumping)
        {
            landingSpotY = transform.position.y;

            // 처음 점프
            PerformJump();
            isGrounded = false;
            isJumping = true;
            isDoubleJumpReady = true;
            hasDoubleJumped = false;
        }
        else if (isDoubleJumpReady && !hasDoubleJumped)
        {
            // 더블 점프
            PerformJump();
            hasDoubleJumped = true;
            isDoubleJumpReady = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking)
        {
            switch (lastDirection)
            {
                case LastDirection.Up:
                    StartCoroutine(OnUpAttackCoroutine());
                    break;

                case LastDirection.Down:
                    StartCoroutine(OnDownAttackCoroutine());
                    break;

                default:
                    StartCoroutine(OnFrontAttackCoroutine());
                    break;

            }
        }
    }

    //------------------------------------------//

    private void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed) moveSpeed *= moveSpeedMultiplier; // 달리기 시작
        else if (context.canceled) moveSpeed = baseMoveSpeed; // 달리기 중지
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (!context.started || isDashing || !canDash) return;

        // 대시 시작
        canDash = false; // 대시 사용 후 쿨타임 시작
        isDashing = true;
        isRunning = false; // 대시 중에는 달리기 상태 해제

        // 이전 대시 트윈이 있다면 중지
        dashTween?.Kill();

        // 저스트닷지 코루틴이 있다면 중지
        if (justDodgeCoroutine != null)
            StopCoroutine(justDodgeCoroutine);

        justDodgeCoroutine = StartCoroutine(JustDodgeWindowCoroutine());

        // 인풋이 없는 상태에서 대쉬를 사용했을 때를 대비 (바라보고있는 방향으로 대시 사용)
        Vector2 dashDirection = moveInput != Vector2.zero ? moveInput.normalized : lastDashDirection;

        // 대시 후 목표 위치 계산
        Vector3 targetPosition = transform.position + (Vector3)(dashDirection * dashDistance);

        // 점프중인지 확인
        if (isJumping && !isGrounded)
        {
            activeJumpTween?.Kill(); // 점프 트윈 중지
            activeFallTween?.Kill(); // 낙하 트윈 중지
            shouldResumeFallAfterDash = true; // 대시 후 낙하 재개 여부 설정
        }

        // 대쉬 고스트 시작
        playerEffectController.PlayTrailEffect();
        AudioManager.instance.PlaySFX(SFXType.Dash, 0.8f, 1.0f);
        dashTween = transform.DOMove(targetPosition, dashDuration)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        isDashing = false;

                        if (shouldResumeFallAfterDash)
                        {
                            ResumeFallAfterDash();
                            shouldResumeFallAfterDash = false; // 낙하 재개 여부 초기화
                        }
                        playerEffectController.StopTrailEffect();
                        StartCoroutine(DashCoolDown());
                    });
    }

    private void GetMoveInput()
    {
        Vector2 move = new Vector2(moveInput.x, moveInput.y);
        transform.position += (Vector3)(move * moveSpeed * Time.deltaTime);
      
        //이동시 파티클 생성
        if (move != Vector2.zero && Time.time - lastDustTime > dustEmitInterval)
        {
            ParticleManager.Instance.Play(ParticleType.WalkDust, footPivot.position);
            lastDustTime = Time.time;
        }
    }

    private void TrackLastDirection()
    {
        if (moveInput.y > 0) lastDirection = LastDirection.Up;
        else if (moveInput.y < 0) lastDirection = LastDirection.Down;
        else if (moveInput.x != 0) lastDirection = LastDirection.Front;
    }

    private void SetLookDirection()
    {
        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        if (horizontal > 0) facingDirection = 1;
        if (horizontal < 0) facingDirection = -1;

        transform.localScale = new Vector3(facingDirection, 1, 1);
        skillVFXSpawnPoint.localScale = new Vector3(facingDirection, 1, 1); // 스킬 VFX 방향 설정
    }

    private void PerformJump()
    {
        // 이전 점프나 낙하 트윈이 있다면 중지
        activeJumpTween?.Kill(); 
        activeFallTween?.Kill();
        shouldResumeFallAfterDash = false; // 대시 후 낙하 재개 여부 초기화

        float targetY = transform.position.y + jumpHeight;

        // 점프 시작 파티클
        playerEffectController.PlayJumpEffect();
        AudioManager.instance.PlaySFX(SFXType.Dash, 0.8f, 1.0f);
        // 점프 업
        activeJumpTween = transform.DOMoveY(targetY, jumpDuration / 2f)
                          .SetEase(Ease.OutQuad)
                          .OnComplete(() =>
                          {
                              // 점프 다운
                              activeFallTween = transform.DOMoveY(landingSpotY, jumpDuration / 2f)
                                                .SetEase(Ease.InQuad)
                                                .OnComplete(() =>
                                                {
                                                    AudioManager.instance.PlaySFX(SFXType.WalkGrass, 0.8f, 1.0f);
                                                    playerEffectController.PlayLandEffect();
                                                    isJumping = false;
                                                    isGrounded = true; // 점프가 끝나면 착지 상태로 변경
                                                });
                          });
    }

    private void ResumeFallAfterDash()
    {
        float currentY = transform.position.y;

        activeFallTween = transform.DOMoveY(landingSpotY, jumpDuration / 2f)
                          .SetEase(Ease.InQuad)
                          .OnComplete(() =>
                          {
                              isJumping = false;
                              isGrounded = true; // 낙하가 끝나면 착지 상태로 변경
                          });
    }

    private IEnumerator DashCoolDown()
    {
        yield return waitForDashCoolDown; // 대시 쿨타임 동안 대기
        canDash = true;                   // 대시 쿨타임이 끝나면 대시 가능
    }

    private IEnumerator JustDodgeWindowCoroutine()
    {
        justDodgeWindow = true;        // 저스트닷지 기회 활성화
        yield return waitForDodgeTime; // 저스트닷지 기회 시간 동안 대기
        justDodgeWindow = false;       // 저스트닷지 기회 비활성화
    }

    private IEnumerator OnFrontAttackCoroutine()
    {
        // 공격 애니메이션 재생 시간 동안 대기
        isAttacking = true;
        playerAnimationHandler.EnterAttackingState();
        playerAnimationHandler.EnterFrontAttacking();
        yield return waitFor1_19sec; // 예시로 0.5초 대기
        playerAnimationHandler.ExitFrontAttacking();
        playerAnimationHandler.ExitAttackingState();
        isAttacking = false;
    }

    private IEnumerator OnUpAttackCoroutine()
    {
        // 위쪽 공격 애니메이션 재생 시간 동안
        isAttacking = true;
        playerAnimationHandler.EnterAttackingState();
        playerAnimationHandler.EnterUpAttacking();
        yield return waitFor1_19sec;
        playerAnimationHandler.ExitUpAttacking();
        playerAnimationHandler.ExitAttackingState();
        isAttacking = false;
    }

    private IEnumerator OnDownAttackCoroutine()
    {
        // 아래쪽 공격 애니메이션 재생 시간 동안 
        isAttacking = true;
        playerAnimationHandler.EnterAttackingState();
        playerAnimationHandler.EnterDownAttacking();
        yield return waitFor1_19sec;
        playerAnimationHandler.ExitDownAttacking();
        playerAnimationHandler.ExitAttackingState();
        isAttacking = false;
    }
}
