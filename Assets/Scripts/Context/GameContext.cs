using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameContext
{
    private readonly string saveDataPath;
    private readonly string defaultSaveDataPath;

    public SaveData saveData;

    public string currentSceneName = null;
    public PlayerData playerData = null;
    public PlayerStateInScene playerStateInScene = new PlayerStateInScene();
    public Queue<NPCData> npcDataQueue = new Queue<NPCData>();
    public Dictionary<GameObject, NPCData> npcDatas = new Dictionary<GameObject, NPCData>();
    public bool dontSaveCurSceneBundle = false;

    public void DontSaveCurSceneBundle()
    {
        dontSaveCurSceneBundle = true;
    }

    public void RegisterNPC(GameObject obj, NPCData data)
    {
        npcDatas[obj] = data;
    }

    public void UnregisterNPC(GameObject obj)
    {
        npcDatas.Remove(obj);
    }

    public GameContext(string savePath, string defaultPath)
    {
        saveDataPath = savePath;
        defaultSaveDataPath = defaultPath;
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

    public void ClearAfterSave()
    {
        npcDataQueue.Clear();
        npcDatas.Clear();
    }


    public void SetCurrentScene(string sceneName)
    {
        currentSceneName = sceneName;
    }

    public void SaveCurrentScene()
    {
        string scene = currentSceneName;
        saveData.curSceneName = currentSceneName;
        saveData.playerData = playerData;
        if (!dontSaveCurSceneBundle)
        {
            var bundle = new SceneBundle
            {
                playerStateInScene = playerStateInScene,
                npcDataQueue = new Queue<NPCData>(npcDatas.Values)
            };
            if (saveData.sceneBundles.ContainsKey(scene))
            {
                saveData.sceneBundles[scene] = bundle;
            }
            else
            {
                saveData.sceneBundles.Add(scene, bundle);
            }
        }
        else
        {
            saveData.sceneBundles.Remove(scene);
            dontSaveCurSceneBundle = false;
        }
        ClearAfterSave();
    }

    public void LoadCurrentSceneData()
    {
        string sceneName = currentSceneName;

        if (saveData.sceneBundles.TryGetValue(sceneName, out var bundle))
        {
            playerStateInScene = bundle.playerStateInScene ?? new PlayerStateInScene();
            npcDatas.Clear();
            npcDataQueue.Clear();

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
