using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    public CinemachineVirtualCamera virtualCam;
    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine shakeRoutine;

    void Awake()
    {
        Instance = this;

        if (virtualCam != null)
            noise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float duration = 0.2f, float amplitude = 2f, float frequency = 2f)
    {
        if (noise == null) return;

        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeCoroutine(duration, amplitude, frequency));
    }

    private IEnumerator ShakeCoroutine(float duration, float amplitude, float frequency)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;

        yield return new WaitForSeconds(duration);

        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}
