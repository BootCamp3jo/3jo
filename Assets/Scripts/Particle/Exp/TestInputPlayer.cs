using DG.Tweening;
using UnityEngine;

public class TestInputPlayer : MonoBehaviour
{
    [SerializeField] private Material shockWaveMaterial;
    public ExpManager expManager;

    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private GhostTrail ghostTrail;
    private Tween dashTween;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            expManager.SpawnExp(this.transform.position, 1);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            // 화면 기준 위치로 변환
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(this.transform.position);

            // 머티리얼에 파라미터 설정
            shockWaveMaterial.SetVector("_Center", new Vector4(viewportPos.x, viewportPos.y, 0, 0));
            shockWaveMaterial.SetFloat("_WaveTime", 0f);        // 시간 초기화
            shockWaveMaterial.SetFloat("_Speed", 2f);         // 파장 속도
            shockWaveMaterial.SetFloat("_WaveWidth", 0.015f);     // 링 두께
            shockWaveMaterial.SetFloat("_WaveCount", 1f);       // 링 개수
            shockWaveMaterial.SetFloat("_WaveGap", 0.25f);      // 링 간격

            // 렌더 패스에 전달
            ShockWaveFeature.Instance.SetParameters(0.5f, 0.1f, 10f); // 필요 시 사용
            ShockWaveFeature.Instance.SetCenter(new Vector2(viewportPos.x, viewportPos.y));
            ShockWaveFeature.Instance.TriggerShockWave();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            DashWithGhost();
        }


    }
    private void DashWithGhost()
    {
        // 중복 대시 방지
        if (dashTween != null && dashTween.IsActive()) return;

        // 고스트 효과 시작
        ghostTrail.StartTrail();

        // 오른쪽으로 대시
        Vector3 targetPos = transform.position + Vector3.right * dashDistance;
        dashTween = transform.DOMove(targetPos, dashDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // 고스트 효과 멈춤
                ghostTrail.StopTrail();
            });
    }
}
