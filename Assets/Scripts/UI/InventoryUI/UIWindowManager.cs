using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private GameObject skillTreePanel;
    [SerializeField] private Button skillTreeButton;
    [SerializeField] private RectTransform TransformAlone;
    [SerializeField] private RectTransform inventoryPaneltTransform;
    [SerializeField] private RectTransform skillTreePanelRectTransform;

    public void InventoryTransformInit()
    {
        skillTreePanel.SetActive(false);
        inventoryPanel.GetComponent<RectTransform>().anchoredPosition = TransformAlone.anchoredPosition;
    }

    public void OnInventoryButton()
    {
        if (skillTreePanel.activeInHierarchy)
        {
            skillTreePanel.SetActive(false);
            inventoryPanel.GetComponent<RectTransform>().anchoredPosition = TransformAlone.anchoredPosition;
        }
    }

    public void OnskillTreeButton()
    {
        if (skillTreePanel.activeInHierarchy)
        {
            skillTreePanel.SetActive(false);
            inventoryPanel.GetComponent<RectTransform>().anchoredPosition = TransformAlone.anchoredPosition;
        }
        else if (!skillTreePanel.activeInHierarchy)
        {
            skillTreePanel.SetActive(true);
            inventoryPanel.GetComponent<RectTransform>().anchoredPosition = inventoryPaneltTransform.anchoredPosition;
        }
    }
}
