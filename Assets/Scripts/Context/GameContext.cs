using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameContext
{
    private readonly string saveDataPath;
    private readonly string defaultSaveDataPath;
    private readonly string achievementSODir;
    private readonly AchievementUnlockUI achievementUnlockUI;

    public SaveData saveData;

    private Dictionary<AchievementID, AchievementSO> achievementSOs = 
        new Dictionary<AchievementID, AchievementSO>();

    #region Save Option
    public bool dontSaveCurSceneBundle = false;
    #endregion  

    #region Object In Scene
    public APlayer player = null;
    public HashSet<ANPC> npcs = new HashSet<ANPC>();
    #endregion

    #region Temp Scene Data
    public PlayerStateInScene playerStateInScene = new PlayerStateInScene();
    public Dictionary<ANPC, NPCData> npcDatas = new Dictionary<ANPC, NPCData>();
    #endregion

    #region API
    // 난이도 선택 옵션
    public void SelectDifficultLevel(DifficultLevel difficultLevel)
    {
        saveData.difficultLevel = difficultLevel;
    }
    
    public DifficultLevel GetDifficultLevel()
    {
        return saveData.difficultLevel;
    }

    public void AddKillCount(int count)
    {
        int tmp = saveData.KillCount;
        saveData.KillCount += count;
        if (tmp < 1 && saveData.KillCount >= 1)
        {
            Unlock(AchievementID.FirstBlood);
        }
        if (tmp < 5 && saveData.KillCount >= 5)
        {
            Unlock(AchievementID.PentaKill);
        }
    }

    public void ClearCurSceneBundle()
    {
        saveData.sceneBundles.Remove(saveData.curSceneName);
    }

    public void DontSaveCurSceneBundle()
    {
        dontSaveCurSceneBundle = true;
    }

    // 업적 언락
    public void Unlock(AchievementID achievementID)
    {
        AchievementData achievementData;
        if (!saveData.achievements.ContainsKey(achievementID))
        {
            achievementData = new AchievementData(achievementID, false);
            saveData.achievements.Add(achievementID, achievementData);
        }
        else
        {
            achievementData = saveData.achievements[achievementID];
        }
        if (achievementData.unlocked == false)
        {
            if (achievementSOs.TryGetValue(achievementID, out AchievementSO achievementSO))
            {
                achievementData.unlocked = true;
                achievementUnlockUI.Unlock(achievementSO);
            }
        }
    }
    #endregion

    public GameContext(string saveDataPath,
        string defaultSaveDataPath,
        string achievementSODir,
        AchievementUnlockUI achievementUnlockUI)
    {
        this.saveDataPath = saveDataPath;
        this.defaultSaveDataPath = defaultSaveDataPath;
        this.achievementSODir = achievementSODir;
        this.achievementUnlockUI = achievementUnlockUI;
        InitializeAchievements();
        if (File.Exists(saveDataPath))
        {
            Load();
        }
        else if (File.Exists(defaultSaveDataPath))
        {
            LoadDefault();
            Save();
        }
        else
        {
            saveData = new SaveData();
            Save();
        }
    }

    private void InitializeAchievements()
    {
        AchievementSO[] loadedSOs = Resources.LoadAll<AchievementSO>(achievementSODir);
        for(int i = 0; i < loadedSOs.Length; i++)
        {
            achievementSOs[loadedSOs[i].achievementID] = loadedSOs[i];
        }
    }

    public void RegisterNPC(ANPC anpc, NPCData data)
    {
        npcDatas[anpc] = data;
        
    }

    public void UnregisterNPC(ANPC anpc)
    {
        npcDatas.Remove(anpc);
    }

    public void Load()
    {
        string json = File.ReadAllText(saveDataPath);
        saveData = JsonConvert.DeserializeObject<SaveData>(json);
    }

    public void LoadDefault()
    {
        string json = File.ReadAllText(defaultSaveDataPath);
        saveData = JsonConvert.DeserializeObject<SaveData>(json);
    }

    public void Save()
    {
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(saveDataPath, json);
    }

    public void ResetSave()
    {
        LoadDefault();
        Save();
    }

    public void ClearBeforeLoad()
    {
        npcDatas.Clear();
        if (player != null)
        {
            GameObject.Destroy(player.gameObject);
        }
        foreach (ANPC anpc in npcDatas.Keys)
        {
            if (anpc != null)
            {
                GameObject.Destroy(anpc.gameObject);
            }
        }
    }

    public void SetCurrentScene(string sceneName)
    {
        saveData.curSceneName = sceneName;
    }

    public void SaveCurrentScene()
    {
        if (player != null)
        {
            player.Save();
        }
        foreach (ANPC aNPC in npcDatas.Keys)
        {
            if (aNPC != null)
            {
                aNPC.Save();
            }
        }
        if (!dontSaveCurSceneBundle)
        {
            Dictionary<string, Queue<NPCData>> tempNPCDataQueues = new Dictionary<string, Queue<NPCData>>();
            foreach (KeyValuePair<ANPC, NPCData> pair in npcDatas)
            {
                string prefabPath = pair.Value.prefabPath;
                if (!tempNPCDataQueues.ContainsKey(prefabPath))
                {
                    tempNPCDataQueues.Add(prefabPath, new Queue<NPCData>());
                }
                tempNPCDataQueues[prefabPath].Enqueue(pair.Value);
            }
            var bundle = new SceneBundle
            {
                playerStateInScene = playerStateInScene,
                npcDataQueues = tempNPCDataQueues
            };
            if (saveData.sceneBundles.ContainsKey(saveData.curSceneName))
            {
                saveData.sceneBundles[saveData.curSceneName] = bundle;
            }
            else
            {
                saveData.sceneBundles.Add(saveData.curSceneName, bundle);
            }
        }
        else
        {
            dontSaveCurSceneBundle = false;
        }
    }

    public void LoadCurrentSceneData()
    {
        string sceneName = saveData.curSceneName;

        if (saveData.sceneBundles.TryGetValue(sceneName, out var bundle))
        {
            playerStateInScene = bundle.playerStateInScene ?? new PlayerStateInScene();
            npcDatas.Clear();

            Logger.Log($"[GameContext] Loaded existing SceneBundle for: {sceneName}");
        }
        else
        {
            Logger.Log($"[GameContext] No saved SceneBundle found for: {sceneName}, initialized empty runtime state.");
        }
    }

    public bool IsSceneSaved(string sceneName)
    {
        if (saveData.sceneBundles.ContainsKey(sceneName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
