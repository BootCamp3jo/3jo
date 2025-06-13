using UnityEngine;

public class ShockWaveController : MonoBehaviour
{
    public Material shockWaveMaterial;
    public Transform bossTransform;  // 보스 트랜스폼 연결

    private float waveTime = 0f;
    private bool isPlaying = false;

    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void TriggerShockWave()
    {
        waveTime = 0f;
        isPlaying = true;

        // 보스 위치를 화면 좌표(뷰포트)로 변환
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(bossTransform.position);

        // 머티리얼에 _Center 값 세팅 (z,w는 0으로)
        shockWaveMaterial.SetVector("_Center", new Vector4(viewportPos.x, viewportPos.y, 0, 0));
    }

    void Update()
    {
        if (!isPlaying)
            return;

        waveTime += Time.deltaTime;
        shockWaveMaterial.SetFloat("_WaveTime", waveTime);

        if (waveTime > 2f)
        {
            isPlaying = false;
            shockWaveMaterial.SetFloat("_WaveTime", 0f);
        }
    }
}
