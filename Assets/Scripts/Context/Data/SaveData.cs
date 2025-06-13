using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneBundle
{
    public PlayerStateInScene playerStateInScene;
    public Queue<NPCData> npcDataQueue = new();
}

[Serializable]
public class SaveData
{
    public DifficultLevel difficultLevel = DifficultLevel.Easy;
    public string curSceneName;
    public int KillCount = 0;
    public Dictionary<AchievementID, AchievementData> achievements = new();
    public PlayerData playerData;
    public Dictionary<string, SceneBundle> sceneBundles = new();
}

public enum DifficultLevel
{
    Easy,
    Normal,
    Hard,
    Impossible
}