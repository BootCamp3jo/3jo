using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggleButton : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    public bool isStopTime = false;
    [SerializeField] private GameObject[] closePanels;

    public void ToggleUI()
    {
        if(closePanels.Length != 0)
        {
            for(int i = 0; i < closePanels.Length; i++)
            {
                closePanels[i].SetActive(false);
            }
        }


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
