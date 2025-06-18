using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUI : MonoBehaviour
{
    public ItemSlotData[] quickSlotDatas;
    public Transform slotPanel;

    // Start is called before the first frame update
    void Start()
    {
        slotPanel = gameObject.transform;
        QuickSlotInit();
    }

    void Update()
    {
        QuickSlotUpdate();
    }

    public void UseItem(int slotIndex)
    {
        ItemSlotData itemSlotData = quickSlotDatas[slotIndex];
        BasicItemData itemData = quickSlotDatas[slotIndex].itemData;

        if (itemData == null || itemData == null || itemSlotData.ItemQuantity <= 0)
        {
            Debug.LogWarning("사용할 수 있는 아이템이 없습니다.");
            return; // 아이템이 없거나 수량이 0인 경우 사용하지 않음
        }

        ItemType itemType = itemSlotData.itemData.itemType;
        string itemName = itemSlotData.itemData.itemName;

        if (itemType == ItemType.Consumable)
        {
            if (itemName == "Health_Potion") 
                PlayerManager.Instance.playerStatHandler.Heal(50f);

            if (itemName == "Mana_Potion")
                PlayerManager.Instance.playerStatHandler.RecoverMana(50f);

            itemSlotData.SubtractItemQuantity(1); // 아이템 수량을 1 감소시킨다.
        }
    }

    private void QuickSlotUpdate()
    {
        // 각 슬롯에 있는 ItemSlotData를 가져와 ItemSlotDatas 배열에 저장
        for (int i = 0; i < slotPanel.childCount; i++)
        {
            // 각 슬롯의 ItemSlotData 컴포넌트를 가져와 quickSlotDatas 배열에 저장
            quickSlotDatas[i] = slotPanel.GetChild(i).GetComponentInChildren<ItemSlotData>();

            // 각 ItemSlotData의 인덱스를 설정
            quickSlotDatas[i].itemSlotIndex = i;
        }
    }

    private void QuickSlotInit()
    {
        quickSlotDatas = new ItemSlotData[slotPanel.childCount];

        // 각 슬롯에 있는 ItemSlotData를 가져와 ItemSlotDatas 배열에 저장
        for (int i = 0; i < slotPanel.childCount; i++)
        {
            // 각 슬롯의 ItemSlotData 컴포넌트를 가져와 quickSlotDatas 배열에 저장
            quickSlotDatas[i] = slotPanel.GetChild(i).GetComponentInChildren<ItemSlotData>();

            // 각 ItemSlotData의 인덱스를 설정
            quickSlotDatas[i].itemSlotIndex = i;
        }
    }
}
