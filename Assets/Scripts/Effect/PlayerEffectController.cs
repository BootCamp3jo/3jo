using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
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
}
