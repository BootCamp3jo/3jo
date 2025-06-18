using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
            AudioManager.instance.PlaySFX(SFXType.SlowMotion);
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

    public void OnQuickSlotButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            var control = context.control;

            if (control == Keyboard.current.digit1Key)
            {
                UIManager.Instance.quickSlotUI.UseItem(0);

            }
            else if (control == Keyboard.current.digit2Key)
            {
                UIManager.Instance.quickSlotUI.UseItem(1);
            }
            else if (control == Keyboard.current.digit3Key)
            {
                UIManager.Instance.quickSlotUI.UseItem(2);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Items"))
        {
            Debug.Log("successfully picked up Items!");
            BasicItemData basicItemData = collision.gameObject.GetComponent<ItemObject>().itemData;

            if (basicItemData == null) return;
            
            // 아이템을 인벤토리에 추가한다
            UIManager.Instance.inventoryUI.AddItem(basicItemData);

            // 월드에서 먹은 아이템을 월드에서 없애준다 (획득한 것)
            Destroy(collision.gameObject);
        }
    }

    // 테스트용 함수 추후 제거 필요
    private void Update()
    {

    }
}
