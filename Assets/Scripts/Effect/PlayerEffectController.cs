using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] private Material shockWaveMaterial;
    [SerializeField] private Transform footPoint; 

    private ShakeEffect shakeEffect;
    private BlinkEffect blinkEffect;
     private float invincibleDuration = 2f;
    private GhostTrail ghostTrail; // 대쉬 고스트
    private void Awake()
    {
        shakeEffect = GetComponent<ShakeEffect>();  
        blinkEffect = GetComponent<BlinkEffect>();
        ghostTrail = GetComponent<GhostTrail>();
    }
    public void PlayJustDodgeEffect()
    {
        CircleMaskEffectController.Instance.TriggerEffect();
    }
    public void PlayShakePlayerEffect()
    {
        shakeEffect.Shake();
    }
    public void PlayBlinkEffect()
    {
        blinkEffect.StartBlink(invincibleDuration);
    }
    public void PlayJumpEffect()
    {
        ParticleManager.Instance.Play(ParticleType.JumpDust, footPoint.position);
    }
    public void PlayLandEffect()
    {
        ParticleManager.Instance.Play(ParticleType.LandDust, footPoint.position);
    }
    public void PlayTrailEffect()
    {
        ghostTrail.StartTrail();
    }
    public void StopTrailEffect()
    {
        ghostTrail.StopTrail();
    }
    public void TriggerShockWave()
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
