using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* [ClassINFO : SlotManager]
   @ Description : This class is used to track the currently selected button in the inventory UI and manage its outline.
   @ Attached at : UIManager -> InventoryUI -> InventoryPanel -> SlotPanel
   @ Methods : =============================================
               [public]
               - TrackClickedButton() : Track the clicked button and manage its outline.
               =============================================
               [private]
               - SlotsInit() : Initialize the slots with default images and add button click listeners.
               =============================================
    */

public class SlotManager : MonoBehaviour
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    public GameObject[] slots; 
    public int slotCounts = 20; // 슬롯 개수
    public Sprite buttonPressedImage;
    public Sprite buttonNotpressedImage;
    public Transform slotPanel;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]
    // ========================== //
    #region [Unity LifeCycle]
    private void Awake()
    {
        slotPanel = gameObject.transform;
    }
    #endregion


    // ========================== //
    //     [Public Methods]
    // ========================== //
    #region [Public Methods]
    #endregion


    // ========================== //
    //     [Private Methods]
    // ========================== //
    #region [Private Methods]
    private void AddMoreSlots()
    {
        // AddMoreSLots : 인벤토리내 슬롯이 부족할 시, 슬롯 개수를 늘려주는 메소드
    }
    #endregion
}
