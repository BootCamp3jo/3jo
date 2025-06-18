using System;
using System.Collections;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerStatHandler : MonoBehaviour, IDamageable
{
    private PlayerData playerData;
    private PlayerStatsUI playerStatsUI;

    private Coroutine manaRegeneration;
    private WaitForSeconds waitForSeconds;
    private float manaRegenRate = 0.03f; // 마나 회복 속도

    // 무적시간체크
    private bool isInvincible = false;      // 무적 상태 여부
    public float invincibleDuration = 1.5f; // 무적 시간

    private void Awake()
    {
        playerData = DataManager.Instance.gameContext.saveData.playerData;
    }

    private void Start()
    {
        playerData = DataManager.Instance.gameContext.saveData.playerData;
        playerStatsUI = UIManager.Instance.playerStatsUI;
        waitForSeconds = new WaitForSeconds(1f);
        StartManaRegeneration();
        if (playerData.CurrentHealth <= 0)
        {
            playerData.CurrentHealth = 0;
            OnPlayerDeath(); // ← 사망 처리 호출
        }
    }

    // ------------------- 체력 ------------------- //
    public void TakeDamage(float amount)
    {
        if (isInvincible) return;

        playerData.CurrentHealth -= amount;
        playerStatsUI.UpdateHealthBar();

        if (playerData.CurrentHealth <= 0)
        {
            playerData.CurrentHealth = 0;
            OnPlayerDeath(); // ← 사망 처리 호출
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }
    private void OnPlayerDeath()
    {
        // 게임 오버 UI 띄우기
        GameOverUI.Instance.Show();

        // 플레이어 조작 막기 등 추가 처리
        StopManaRegeneration();
        // 예: GetComponent<PlayerController>().enabled = false;

        // 게임 정지
        Time.timeScale = 0f;
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

    public void StartManaRegeneration()
    {
        if (manaRegeneration == null)
        {
            manaRegeneration = StartCoroutine(ManaRegenLoop());
        }
    }

    public void StopManaRegeneration()
    {
        if (manaRegeneration != null)
        {
            StopCoroutine(manaRegeneration);
            manaRegeneration = null;
        }
    }

    private IEnumerator ManaRegenLoop()
    {
        while (true)
        {
            if (playerData.CurrentMana < playerData.MaxMana)
            {
                playerData.CurrentMana += playerData.MaxMana * manaRegenRate; // 1
                playerStatsUI.UpdateManaBar();
            }
            yield return waitForSeconds;
        }
    }

    // ------------------- 경험치 & 레벨 ------------------- //
    public void AddExperience(float amount)
    {
        playerData.Experience += amount;

        while (playerData.Experience >= playerData.MaxExperience)
        {
            playerData.Experience -= playerData.MaxExperience;
            LevelUp();
            playerData.SetMaxExperience(CalculateMaxExpModifier(playerData.Level));
        }

        playerStatsUI.UpdateExpBar();
    }

    private void LevelUp()
    {
        AddSkillPoint(playerData.Level);
        playerData.Level++;
        playerStatsUI.UpdateLevelText();
    }

    private float CalculateMaxExpModifier(int level)
    {
        return (50 * level);
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

    // ------------------- 스킬포인트 ------------------- //

    public void AddSkillPoint(int level)
    {
        int amount = SkillPointCalculation(level);

        playerData.SkillPoint += amount;
        SkillManager.Instance.SetSkillPointText();
    }

    public void UseSkillPoint(int amount)
    {
        if (playerData.SkillPoint >= amount)
        {
            playerData.SkillPoint -= amount;
            SkillManager.Instance.SetSkillPointText();
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("Not enough skill points!");
#endif
        }
    }

    private int SkillPointCalculation(int level)
    {
        int calculatedSkillPoint = 0;

        if (level <= 2)
            calculatedSkillPoint = 1; // Level 1 - 2

        else if (level <= 4)
            calculatedSkillPoint = 2; // Level 3 - 4

        else if (level <= 6)
            calculatedSkillPoint = 3; // Level 5 - 6

        else if (level <= 8)
            calculatedSkillPoint = 4; // Level 7 - 8

        else
            calculatedSkillPoint = 5; // Level 9 ~

        return calculatedSkillPoint;
    }
}