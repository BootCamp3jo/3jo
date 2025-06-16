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
        }
        else
        {
            playerStatHandler.TakeDamage(damage);
        }
    }


    // 테스트용 함수 추후 제거 필요
    private void Update()
    {
        // 저스트 회피 연결 후 제거
        if (Input.GetKeyDown(KeyCode.F10))
        {
            playerEffectController.PlayJustDodgeEffect();
            playerEffectController.TriggerShockWave();
        }
        // 피격 연결 이후 제거
        if (Input.GetKeyDown(KeyCode.F11))
        {
            playerEffectController.PlayBlinkEffect();
            playerEffectController.PlayShakePlayerEffect();
        }
    }
}
