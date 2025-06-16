using System;
using System.Collections;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour, IDamageable
{
    private PlayerData playerData;
    private PlayerStatsUI playerStatsUI;

    // 무적시간체크
    private bool isInvincible = false;      // 무적 상태 여부
    public float invincibleDuration = 1.5f; // 무적 시간


     private void Start()
    {
        playerData = PlayerManager.Instance.playerData;
        playerStatsUI = UIManager.Instance.playerStatsUI;
    }

    // ------------------- 체력 ------------------- //
    public void TakeDamage(float amount)
    {
        if (isInvincible) return; // 무적 상태면 데미지 무시

        playerData.CurrentHealth -= amount;
        playerStatsUI.UpdateHealthBar();

        StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    public void Heal(float amount)
    {
        playerData.CurrentHealth += amount;
        playerStatsUI.UpdateHealthBar();
    }

    // ------------------- 마나 ------------------- //
    public void UseMana(float amount)
    {
        if (playerData.CurrentMana >= amount)
        {
            playerData.CurrentMana -= amount;
            playerStatsUI.UpdateManaBar();
        }
    }

    public void RecoverMana(float amount)
    {
        playerData.CurrentMana += amount;
        playerStatsUI.UpdateManaBar();

    }

    // ------------------- 경험치 & 레벨 ------------------- //
    public void AddExperience(float amount)
    {
        playerData.Experience += amount;

        while (playerData.Experience >= playerData.MaxExperience)
        {
            playerData.Experience -= playerData.MaxExperience;
            LevelUp();
            playerData.SetMaxExperience(playerData.Level);
        }

        playerStatsUI.UpdateExpBar();
    }

    private void LevelUp()
    {
        playerData.Level++;
        playerStatsUI.UpdateLevelText();
    }

    // ------------------- 코인 ------------------- //
    public void AddCoins(int amount)
    {
        playerData.Coin += amount;
        playerStatsUI.UpdateCoinText();
    }

    public bool SpendCoins(int amount)
    {
        if (playerData.Coin >= amount)
        {
            playerData.Coin -= amount;
            playerStatsUI.UpdateCoinText();
            return true;
        }
        return false;
    }
}