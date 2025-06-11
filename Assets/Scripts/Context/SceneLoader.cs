using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    GameContext gameContext;
    private static SceneLoader instance;
    [SerializeField] private List<string> abortSceneNameList = new();
    private HashSet<string> abortSceneNames = new();

    private void Awake()
    {
    }

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        foreach (string name in abortSceneNameList)
        {
            abortSceneNames.Add(name);
        }
        gameContext = GameManager.instance.gameContext;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (abortSceneNames.Contains(scene.name))
        {
            return;
        }
        if (gameContext.IsSceneSaved(scene.name))
        {

            SceneBundle saveData = gameContext.saveData.sceneBundles[scene.name];
            while (saveData.npcDataQueue.Count > 0)
            {
                NPCData npcData = saveData.npcDataQueue.Dequeue();
                GameObject prefab = Resources.Load<GameObject>(npcData.prefabPath);
                if (prefab != null)
                {
                    GameObject npc = GameObject.Instantiate(prefab);
                    npc.transform.position = new Vector3(npcData.posX, npcData.posY, npcData.posZ);
                    if(npc.TryGetComponent<NPC>(out NPC _npc))
                    {
                        _npc.isCreatedBySceneLoader = true;
                        _npc.npcData = npcData;
                    }
                }
                else
                {
                    Logger.LogError($"Prefab not found at path: {npcData.prefabPath}");
                }
            }
        }
    }
}
