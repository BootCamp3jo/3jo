using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] InGameUIButtons inGameUIButtons;
    [SerializeField] PlayerStatsUI playerStatsUI;
    [SerializeField] SkillUI skillUI;

    void Awake()
    {
        inGameUIButtons = GetComponent<InGameUIButtons>();
        playerStatsUI = GetComponentInChildren<PlayerStatsUI>();
        SkillUI skillUI = GetComponentInChildren<SkillUI>();
    }

    void Start()
    {
        
    }
}
