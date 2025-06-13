using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ Serializable]
public class PlayerStatData : MonoBehaviour
{
    [Header("Player Stats")]

    // 체력
    [SerializeField]
    private int _health;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Clamp(value, 0, _maxHealth);
        }
    }

    // 마나
    [SerializeField]
    private int _mana;
    public int Mana
    {
        get { return _mana; }
        set
        {
            _mana = Mathf.Clamp(value, 0, _maxMana);
        }
    }

    // 레벨
    [SerializeField]
    private int _level = 1;
    public int Level
    {
        get { return _level; }
        set
        {
            _level = Mathf.Clamp(value, 1, _maxLevel);
        }
    }

    // 경험치
    [SerializeField]
    private int _experience;
    public int Experience
    {
        get { return _experience; }
        set
        {
            _experience = Mathf.Clamp(value, 0, _maxExperience);
        }
    }

    // 코인
    [SerializeField]
    private int _coin;
    public int Coin
    {
        get { return _coin; }
        set
        {
            _coin = Mathf.Clamp(value, 0, _maxCoin);
        }
    }


    // 최대 수치들
    private int _maxHealth = 100;
    private int _maxMana = 100;
    private int _maxLevel = 100;
    private int _maxExperience = 100;
    readonly int _maxCoin = 999999; // 코인은 최대 999,999까지 가능

    //----------------------------------------------------------//

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public void SetMaxHealth(int value)
    {
        _maxHealth = value;
    }

    //----------------------------------------------------------//

    public int GetMaxMana()
    {
        return _maxMana;
    }

    public void SetMaxMana(int value)
    {
        _maxMana = value;
    }

    //----------------------------------------------------------//

    public int GetMaxLevel()
    {
        return _maxLevel;
    }

    public void SetMaxLevel(int value)
    {
        _maxLevel = value;
    }

    //----------------------------------------------------------//

    public int GetMaxExperience()
    {
        return _maxExperience;
    }

    public void SetMaxExperience(int value)
    {
        _maxExperience = value;
    }

    //----------------------------------------------------------//

    public int GetMaxCoin()
    {
        return _maxCoin;
    }

    //----------------------------------------------------------//
}
