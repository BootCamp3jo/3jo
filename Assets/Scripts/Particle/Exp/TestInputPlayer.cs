using DG.Tweening;
using UnityEngine;

public class TestInputPlayer : MonoBehaviour
{
    [SerializeField] private Material shockWaveMaterial;
    public ExpManager expManager;

    [Header("이동 관련")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform footPoint; // 발 위치

    [Header("대시 관련")]
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private GhostTrail ghostTrail;
    private Tween dashTween;

    [Header("이동 파티클")]
    [SerializeField] private float dustEmitInterval = 0.2f;
    private float lastDustTime = 0f;

    [Header("무적 관련(깜빡임)")]
    [SerializeField] private BlinkEffect blinkEffect;
    [SerializeField] private float invincibleDuration = 2f;

    [Header("피격 관련(깜빡임)")]
    [SerializeField] private FlashEffect flashEffect;

    [Header("피격 관련(파티클)")]
    [SerializeField] private HitEffect hitEffect;

    [Header("피격 관련(진동)")]
    [SerializeField] private ShakeEffect shakeEffect;

    [Header("저스트회피")]
    [SerializeField] private WaveGrayEffectManager waveGrayEffectManager;

    private Rigidbody2D rb;
    private Vector2 input;

    // --------------------------------------------- //
    // 체력바 감소 - 페이드
    [SerializeField] private HealthBarFade barFade;
    private float simulatedHp = 1f; // 현재 체력 비율 (0~1)

    [Header("월드 체력바")]
    [SerializeField] private EnemyHpBar enemyHpBar; // 월드 위치 체력바 (슬라이스 떨어짐 효과)
    private float testEnemyHp = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 입력 처리
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            expManager.SpawnExp(transform.position, 1);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            simulatedHp -= 0.1f;
            simulatedHp = Mathf.Clamp01(simulatedHp); // 0보다 작아지지 않도록
            barFade.SetHp(simulatedHp);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            simulatedHp += 0.1f;
            simulatedHp = Mathf.Clamp01(simulatedHp);
            barFade.SetHp(simulatedHp);
        }
        if (Input.GetKeyDown(KeyCode.F4)) // 체력 감소 → 체력바 조각 떨어짐
        {
            testEnemyHp -= 0.1f;
            testEnemyHp = Mathf.Clamp01(testEnemyHp);
            enemyHpBar.SetHp(testEnemyHp);
        }

        if (Input.GetKeyDown(KeyCode.F5)) // 체력 회복 → 단순히 증가
        {
            testEnemyHp += 0.1f;
            testEnemyHp = Mathf.Clamp01(testEnemyHp);
            enemyHpBar.SetHp(testEnemyHp);
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
         //   DashWithGhost();
        }

        if (Input.GetKeyDown(KeyCode.F7)) // 예시 키 적 피격
        {
            hitEffect.PlayHitEffect(1);
            flashEffect.TriggerFlash(); // 직접 호출
        }
        if (Input.GetKeyDown(KeyCode.F8)) // 예시 키
        {
            hitEffect.PlayHitEffect(1);
            flashEffect.TriggerFlash(); // 직접 호출
            shakeEffect.Shake();
        }//
    


        // 이동 중 먼지 파티클 생성
        if (input.magnitude > 0.1f && Time.time - lastDustTime > dustEmitInterval)
        {
           // ParticleManager.Instance.Play(ParticleType.WalkDust, footPoint.position);
            //lastDustTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        // 기본 이동 처리
        rb.velocity = input * moveSpeed;
    }

    private void DashWithGhost()
    {
        // 중복 대시 방지
        if (dashTween != null && dashTween.IsActive()) return;

        ghostTrail.StartTrail();

        Vector3 targetPos = transform.position + Vector3.right * dashDistance;
        dashTween = transform.DOMove(targetPos, dashDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                ghostTrail.StopTrail();
            });
    }


}
