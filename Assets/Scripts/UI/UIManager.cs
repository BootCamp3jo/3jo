using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] public InGameUIButtons inGameUIButtons;
    [SerializeField] public InventoryUI inventoryUI;
    [SerializeField] public PlayerStatsUI playerStatsUI;
    [SerializeField] public SkillUI skillUI;

    void Awake()
    {
        inGameUIButtons = GetComponentInChildren<InGameUIButtons>();
        inventoryUI = GetComponentInChildren<InventoryUI>();
        playerStatsUI = GetComponentInChildren<PlayerStatsUI>();
        skillUI = GetComponentInChildren<SkillUI>();
    }

    void Start()
    {
        
    }
}
