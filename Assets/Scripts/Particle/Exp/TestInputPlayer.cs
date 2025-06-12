using UnityEngine;

public class TestInputPlayer : MonoBehaviour
{
    [SerializeField] private Material shockWaveMaterial;
    public ExpManager expManager;

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
            shockWaveMaterial.SetFloat("_Speed", 0.5f);         // 파장 속도
            shockWaveMaterial.SetFloat("_WaveWidth", 0.1f);     // 링 두께
            shockWaveMaterial.SetFloat("_WaveCount", 3f);       // 링 개수
            shockWaveMaterial.SetFloat("_WaveGap", 0.25f);      // 링 간격

            // 렌더 패스에 전달
            ShockWaveFeature.Instance.SetParameters(0.5f, 0.1f, 10f); // 필요 시 사용
            ShockWaveFeature.Instance.SetCenter(new Vector2(viewportPos.x, viewportPos.y));
            ShockWaveFeature.Instance.TriggerShockWave();
        }
    }
}
