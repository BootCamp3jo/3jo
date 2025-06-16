using System;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour, IDamageable
{
    private PlayerData playerData;
    private PlayerStatsUI playerStatsUI;

    private void Start()
    {
        playerData = PlayerManager.Instance.playerData;
        playerStatsUI = UIManager.Instance.playerStatsUI;
    }

    // ------------------- 체력 ------------------- //
    public void TakeDamage(float amount)
    {
        playerData.CurrentHealth -= amount;
        playerStatsUI.UpdateHealthBar();
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