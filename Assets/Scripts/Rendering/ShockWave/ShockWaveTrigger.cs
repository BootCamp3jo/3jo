using UnityEngine;

public class ShockWaveTrigger : MonoBehaviour
{
    public Transform bossTransform;  // 보스 위치 연결

    public void OnClickShockWave()
    {
        if (ShockWaveFeature.Instance != null && bossTransform != null)
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(bossTransform.position);

            // 머티리얼의 _Center 값을 직접 세팅해주자
            ShockWaveFeature.Instance.shockWaveMaterial.SetVector("_Center", new Vector4(viewportPos.x, viewportPos.y, 0, 0));

            // 충격파 시작 호출 (이제 파라미터 없음)
            ShockWaveFeature.Instance.TriggerShockWave();
        }
    }
}
