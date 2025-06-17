using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;
    // Start is called before the first frame update
    private void Start()
    {
        pauseUI.SetActive(false);
    }
    public void OnClick()
    {
        Time.timeScale = 0.0f;
        pauseUI.SetActive(true);
    }
}
