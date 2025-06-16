using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class Player : APlayer
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerEffectController playerEffectController;
    [SerializeField] private PlayerStatHandler playerStatHandler;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        playerMovement = PlayerManager.Instance.playerMovement;
        playerEffectController = PlayerManager.Instance.playerEffectController;
        playerStatHandler = PlayerManager.Instance.playerStatHandler;
    }

    public void OnHitByEnemy(float damage)
    {
        if (playerMovement.JustDodgeWindow)
        {
            playerEffectController.PlayJustDodgeEffect();
            playerEffectController.TriggerShockWave();
        }
        else
        {
            playerEffectController.PlayBlinkEffect(playerStatHandler.invincibleDuration);
            playerEffectController.PlayShakePlayerEffect();
            playerStatHandler.TakeDamage(damage);
        }
    }


    // 테스트용 함수 추후 제거 필요
    private void Update()
    {

    }
}
