using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggleButton : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    public bool isStopTime = false;

    public void ToggleUI()
    {
        if (panel.activeSelf == false)
        {
            panel.SetActive(true);
            if (isStopTime)
                Time.timeScale = 0f;
        }

        else
        {
            panel.SetActive(false);
            if (isStopTime)
                Time.timeScale = 1f;
        }
          
    }

}
