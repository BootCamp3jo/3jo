using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public float testDamage = 10f;
    public float testHeal = 15f;
    public float testManaUsage = 10f;
    public float testManaRegen = 5f;
    public int testCoins = 100;
    public int testXP = 50;

    [ContextMenu("TestDamage")]
    public void TestDamage()
    {
        PlayerStatHandler playerStatHandler = PlayerManager.Instance.playerStatHandler;
        playerStatHandler.TakeDamage(testDamage);

    }

    [ContextMenu("TestHeal")]
    public void TestHeal()
    {
        PlayerStatHandler playerStatHandler = PlayerManager.Instance.playerStatHandler;
        playerStatHandler.Heal(testHeal);
    }

    [ContextMenu("TestManaUsage")]
    public void TestManaUsage()
    {
        PlayerStatHandler playerStatHandler = PlayerManager.Instance.playerStatHandler;
        playerStatHandler.UseMana(testManaUsage);
    }

    [ContextMenu("TestManaRegen")]
    public void TestManaRegen()
    {
        PlayerStatHandler playerStatHandler = PlayerManager.Instance.playerStatHandler;
        playerStatHandler.RecoverMana(testManaRegen);
    }

    [ContextMenu("TestAddCoins")]
    public void TestAddCoins()
    {
        PlayerStatHandler playerStatHandler = PlayerManager.Instance.playerStatHandler;
        playerStatHandler.AddCoins(testCoins);
    }

    [ContextMenu("TestAddXP")]
    public void TestAddXP()
    {
        PlayerStatHandler playerStatHandler = PlayerManager.Instance.playerStatHandler;
        playerStatHandler.AddExperience(testXP);
    }
}
