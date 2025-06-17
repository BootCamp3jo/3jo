using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;
    // Start is called before the first frame update
    public void OnClick()
    {
        Time.timeScale = 1.0f;
        pauseUI.SetActive(false);
    }
}
