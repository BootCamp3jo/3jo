using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveEffect : MonoBehaviour
{
    [SerializeField] private Material shockWaveMaterial;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float width = 0.015f;
    [SerializeField] private float duration = 1f;

    public void TriggerShockWave(Transform pivot)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(pivot.position);
        shockWaveMaterial.SetVector("_Center", new Vector4(viewportPos.x, viewportPos.y, 0, 0));
        shockWaveMaterial.SetFloat("_WaveTime", 0f);
        shockWaveMaterial.SetFloat("_Speed", 3f);
        shockWaveMaterial.SetFloat("_WaveWidth", 0.015f);
        shockWaveMaterial.SetFloat("_WaveCount", 1f);
        shockWaveMaterial.SetFloat("_WaveGap", 0.25f);


        ShockWaveFeature.Instance.SetParameters(speed, width, duration);
        ShockWaveFeature.Instance.SetCenter(new Vector2(viewportPos.x, viewportPos.y));
        ShockWaveFeature.Instance.TriggerShockWave();
    }
}
