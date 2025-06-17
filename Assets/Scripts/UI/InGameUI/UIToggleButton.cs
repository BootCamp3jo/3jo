using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggleButton : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void ToggleUI()
    {
        if (panel.activeSelf == false)
            panel.SetActive(true);
        else
            panel.SetActive(false);
    }

}
