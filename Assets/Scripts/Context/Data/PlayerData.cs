using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillData
{
    public string skillSOPath;
    public bool isUnlock;
    public bool isUpgraded;
}

[Serializable]
public class PlayerData
{
    [Header("Prefab Info")]
    public string prefabPath;

    [Header("Player Stats")]
    [SerializeField] private float _attackPower;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _currentMana;
    [SerializeField] private int _level = 1;
    [SerializeField] private float _experience;
    [SerializeField] private int _coin;
    [SerializeField] private int _skillPoint = 0;

    [Header("Max Stats")]
    private float _maxAttackPower = 9999f; // 최대 공격력
    private float _maxHealth = 100;
    private float _maxMana = 100;
    private int _maxLevel = 100;
    private float _maxExperience = 100f;
    private int _maxCoin = 999_999;

    [Header("Skill Data")]
    [SerializeField] public List<SkillData> skillDatas = new List<SkillData>();

    //--------------------------------------//
    public float AttackPower
    {
        get => _attackPower;
        set => _attackPower = Mathf.Clamp(value, 0, _maxAttackPower);
    }

    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
    }

    public float CurrentMana
    {
        get => _currentMana;
        set => _currentMana = Mathf.Clamp(value, 0, _maxMana);
    }

    public int Level
    {
        get => _level;
        set => _level = Mathf.Clamp(value, 1, _maxLevel);
    }

    public float Experience
    {
        get => _experience;
        set => _experience = value;
    }

    public int Coin
    {
        get => _coin;
        set => _coin = Mathf.Clamp(value, 0, _maxCoin);
    }

    public int SkillPoint
    {
        get => _skillPoint;
        set => _skillPoint = Mathf.Max(value, 0);
    }

    //--------------------------------------//
    public float MaxAttackPower => _maxAttackPower;
    public float MaxHealth => _maxHealth;
    public float MaxMana => _maxMana;
    public int MaxLevel => _maxLevel;
    public float MaxExperience => _maxExperience;
    public int MaxCoin => _maxCoin;

    //--------------------------------------//
    public void SetAttackPower(float value) => _attackPower = Mathf.Clamp(value, 0, _maxAttackPower);
    public void SetMaxHealth(int value) => _maxHealth = value;
    public void SetMaxMana(int value) => _maxMana = value;
    public void SetMaxLevel(int value) => _maxLevel = value;
    public void SetMaxExperience(float maxExpModifier) => _maxExperience += maxExpModifier;

    //--------------------------------------//
}

[Serializable]
public class PlayerStateInScene
{
    public bool isPlayerExist;
    public float posX;
    public float posY;
    public float posZ;
}