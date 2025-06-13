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

    private Rigidbody2D rb;
    private Vector2 input;

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
            TriggerShockWave();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            DashWithGhost();
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            ParticleManager.Instance.Play(ParticleType.JumpDust, footPoint.position);
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            ParticleManager.Instance.Play(ParticleType.LandDust, footPoint.position);
        }



        // 이동 중 먼지 파티클 생성
        if (input.magnitude > 0.1f && Time.time - lastDustTime > dustEmitInterval)
        {
            ParticleManager.Instance.Play(ParticleType.WalkDust, footPoint.position);
            lastDustTime = Time.time;
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

    private void TriggerShockWave()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        shockWaveMaterial.SetVector("_Center", new Vector4(viewportPos.x, viewportPos.y, 0, 0));
        shockWaveMaterial.SetFloat("_WaveTime", 0f);
        shockWaveMaterial.SetFloat("_Speed", 2f);
        shockWaveMaterial.SetFloat("_WaveWidth", 0.015f);
        shockWaveMaterial.SetFloat("_WaveCount", 1f);
        shockWaveMaterial.SetFloat("_WaveGap", 0.25f);

        ShockWaveFeature.Instance.SetParameters(0.5f, 0.1f, 10f);
        ShockWaveFeature.Instance.SetCenter(new Vector2(viewportPos.x, viewportPos.y));
        ShockWaveFeature.Instance.TriggerShockWave();
    }
}
