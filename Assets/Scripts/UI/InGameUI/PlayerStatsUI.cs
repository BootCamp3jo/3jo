using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    [SerializeField] private HealthBarFade healthBar;
    [SerializeField] private Image manaBar;
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI coinText;
    // 경험치 이펙트
    [SerializeField] private ExpEffect expEffect;

    private void Start()
    {
        playerData = PlayerManager.Instance.playerData;
        Init();
    }

    private void Init()
    {
        coinText.text = playerData.Coin.ToString();
        levelText.text = playerData.Level.ToString();

        // 기본 스탯 UI 업데이트
        SkillManager.Instance.SetSkillPointText();
    }

    public void UpdateHealthBar()
    {
        if (playerData.MaxHealth > 0f)
        {
            healthBar.SetHp(Mathf.Clamp01(playerData.CurrentHealth / playerData.MaxHealth));
        }
    }

    public void UpdateManaBar()
    {
        if (playerData.MaxMana > 0f)
        {
            manaBar.fillAmount = Mathf.Clamp01(playerData.CurrentMana / playerData.MaxMana);
        }
    }

    public void UpdateExpBar()
    {
        if (playerData.MaxExperience > 0f)
        {
            expEffect.AnimateBarPop();
            expEffect.SpawnParticleAt();
            expBar.fillAmount = Mathf.Clamp01(playerData.Experience / playerData.MaxExperience);
        }
    }

    public void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = playerData.Level.ToString();
        }
    }

    public void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = playerData.Coin.ToString();
        }
    }


}
