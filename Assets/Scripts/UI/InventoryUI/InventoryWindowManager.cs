using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindowManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private GameObject CraftingPanel;
    [SerializeField] private Button craftingButton;
    [SerializeField] private RectTransform TransformAlone;
    [SerializeField] private RectTransform inventoryPaneltTransform;
    [SerializeField] private RectTransform craftingPanelRectTransform;

    public void InventoryTransformInit()
    {
        CraftingPanel.SetActive(false);
        inventoryPanel.GetComponent<RectTransform>().anchoredPosition = TransformAlone.anchoredPosition;
    }

    public void OnInventoryButton()
    {
        if (CraftingPanel.activeInHierarchy)
        {
            CraftingPanel.SetActive(false);
            inventoryPanel.GetComponent<RectTransform>().anchoredPosition = TransformAlone.anchoredPosition;
        }
    }

    public void OnCraftingButton()
    {
        if (CraftingPanel.activeInHierarchy)
        {
            CraftingPanel.SetActive(false);
            inventoryPanel.GetComponent<RectTransform>().anchoredPosition = TransformAlone.anchoredPosition;
        }
        else if (!CraftingPanel.activeInHierarchy)
        {
            CraftingPanel.SetActive(true);
            inventoryPanel.GetComponent<RectTransform>().anchoredPosition = inventoryPaneltTransform.anchoredPosition;
        }
    }
}
