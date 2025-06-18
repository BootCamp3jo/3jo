using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] public InGameUIButtons inGameUIButtons;
    [SerializeField] public InventoryUI inventoryUI;
    [SerializeField] public PlayerStatsUI playerStatsUI;
    [SerializeField] public UIWindowManager uIWindowManager;
    [SerializeField] public SkillUI skillUI;
    [SerializeField] public UltGuageBarManager ultGuageBarManager;

    void Awake()
    {
        inGameUIButtons = GetComponentInChildren<InGameUIButtons>();
        inventoryUI = GetComponentInChildren<InventoryUI>();
        playerStatsUI = GetComponentInChildren<PlayerStatsUI>();
        uIWindowManager = GetComponentInChildren<UIWindowManager>();
        skillUI = GetComponentInChildren<SkillUI>();
        ultGuageBarManager = GetComponentInChildren<UltGuageBarManager>();
    }
}
