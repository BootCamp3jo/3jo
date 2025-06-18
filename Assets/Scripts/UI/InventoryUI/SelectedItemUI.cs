using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedItemUI : MonoSingleton<SelectedItemUI>
{
    private ItemSlotData itemSlotData;

    void Start()
    {
        gameObject.SetActive(false); // 초기에는 선택된 아이템 UI를 비활성화
    }

    public void ActivePopUp()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false); // 이미 활성화되어 있다면 비활성화
        }
        else
        {
            gameObject.SetActive(true); // 비활성화되어 있다면 활성화
        }
    }

    public void GetItemSlotData(ItemSlotData itemSlotInfo)
    {
        itemSlotData = itemSlotInfo; // 선택된 아이템 슬롯 데이터를 저장
    }

    public void OnThrowItemButton()
    {
        itemSlotData.SubtractItemQuantity(1); // 아이템 수량을 1 감소시킨다.
        //UIManager.Instance.playerUI.inventoryUIWrapper.ThrowItem(itemSlotData.itemData, 1); // 아이템을 월드에 생성한다
    }

    public void OnThrowAllItmeButton()
    {
        itemSlotData.ClearSlot(); // 아이템 수량을 0으로 설정하여 아이템을 제거한다.
        //UIManager.Instance.playerUI.inventoryUIWrapper.ThrowItem(itemSlotData.itemData, itemSlotData.ItemQuantity);
    }

    public void OnUseItemButton()
    {
        if (itemSlotData.itemData == null || itemSlotData.ItemQuantity <= 0)
        {
            Debug.LogWarning("사용할 수 있는 아이템이 없습니다.");
            return; // 아이템이 없거나 수량이 0인 경우 사용하지 않음
        }

        ItemType itemType = itemSlotData.itemData.itemType;
        string itemName = itemSlotData.itemData.itemName;

        if (itemType == ItemType.Consumable)
        {
            itemSlotData.SubtractItemQuantity(1); // 아이템 수량을 1 감소시킨다.
        }

    }

    public void OnExitSelectedItemButton()
    {
        gameObject.SetActive(false); // 선택된 아이템 UI를 비활성화
    }
}
