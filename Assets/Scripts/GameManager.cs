using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string saveDataDir;
    [SerializeField] private string defaultSaveDataPath;
    [SerializeField] private List<string> abortSceneNameList = new();
    private HashSet<string> abortSceneNames = new();

    public static GameManager instance { get; private set; }
    public GameContext gameContext;

    void Awake()
    {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        gameContext = new GameContext(saveDataDir, defaultSaveDataPath);

        foreach(string name in abortSceneNameList)
        {
            abortSceneNames.Add(name);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnApplicationQuit()
    {
        if (abortSceneNames.Contains(SceneManager.GetActiveScene().name))
        {
            return;
        }
        gameContext.SaveCurrentScene();
            gameContext.Save();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (abortSceneNames.Contains(SceneManager.GetActiveScene().name))
        {
            return;
        }
        gameContext.SetCurrentScene(scene.name);
        gameContext.LoadCurrentSceneData();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (abortSceneNames.Contains(SceneManager.GetActiveScene().name))
        {
            return;
        }
        gameContext.SaveCurrentScene();
    }

    [ContextMenu("DontSaveCurSceneBundle")]
    public void DontSaveCurSceneBundle()
    {
        gameContext.dontSaveCurSceneBundle = true;
    }
}
