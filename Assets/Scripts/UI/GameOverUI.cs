using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Show()
    {
        gameOverPanel.SetActive(true);
    }
}
