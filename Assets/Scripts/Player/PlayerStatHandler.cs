using UnityEngine;

public class PlayerStatHandler : MonoBehaviour, IDamageable
{
    private PlayerStatData playerStatData;

    private void Awake()
    {
        playerStatData = PlayerManager.Instance.playerStatData;
    }

    // ------------------- 체력 ------------------- //
    public void TakeDamage(int amount)
    {
        playerStatData.Health -= amount;
    }

    public void Heal(int amount)
    {
        playerStatData.Health += amount;
    }

    // ------------------- 마나 ------------------- //
    public void UseMana(int amount)
    {
        if (playerStatData.Mana >= amount)
        {
            playerStatData.Mana -= amount;
        }
    }

    public void RecoverMana(int amount)
    {
        playerStatData.Mana += amount;
    }

    // ------------------- 경험치 & 레벨 ------------------- //
    public void AddExperience(int amount)
    {
        playerStatData.Experience += amount;
        if (playerStatData.Experience >= playerStatData.GetMaxExperience())
        {
            LevelUp();
            playerStatData.Experience -= playerStatData.GetMaxExperience();           // 경험치 초기화 또는 조정
            playerStatData.SetMaxExperience(playerStatData.GetMaxExperience() + 100); // 레벨업 시 최대 경험치 증가
        }
    }

    private void LevelUp()
    {
        playerStatData.Level++;
    }

    // ------------------- 코인 ------------------- //
    public void AddCoins(int amount)
    {
        playerStatData.Coin += amount;
    }

    public bool SpendCoins(int amount)
    {
        if (playerStatData.Coin >= amount)
        {
            playerStatData.Coin -= amount;
            return true;
        }
        return false;
    }
}