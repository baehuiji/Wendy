/////////////////////Inventory/////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    static public Inventory instance;

    [SerializeField]
    private GameObject go_InventoryBase;

    [SerializeField]
    private GameObject go_SlotsParent;

    private Slot[] slots;

    private bool remove_count = false;
    public bool Remove_Count { get { return remove_count; } } 

    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    public void RemoveSlot(Item _item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (slots[i].item == _item)
                {
                    slots[i].RemoveItem(_item);
                    remove_count = true;
                    //Debug.Log(remove_count);
                    return;
                }
                else
                {
                    //Debug.Log("인벤토리에 해당 아이템이 없습니다"); 
                    remove_count = false;
                }
            }
            remove_count = false;
        }

    }
    public bool AcquireItem(Item _item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Item.ItemType.Read != _item.itemType && Item.ItemType.Equipment != _item.itemType)
            {
                if (slots[i].item == null)
                {
                    slots[i].AddItem(_item);
                    return true;
                }
            }
        }
        return false;
    }

    // - 아이템 사용하기
    public GameObject get_Item(int index) // 아이템 게임오브젝트 얻기
    {
        return slots[index].item.itemPrefab;
    }

    public Item get_ItemInfo(int index) // 아이템 얻기
    {
        return slots[index].item;
    }

    public int get_ItemCode(int index) // 아이템 코드 얻기
    {
        if (slots[index].item == null)
            return 99;
        return slots[index].item.itemCode;
    }

    public void clear_Slot(int index) 
    {
        slots[index].ClearSlot();
    }

    public bool IsVoid_Slot(int index)
    {
        return slots[index].IsVoid();
    }
}








