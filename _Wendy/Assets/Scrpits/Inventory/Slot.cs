using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public Image itemImage;

    //슬롯이 비었는지? -> 비었으면 true
    private bool voidState = true;


    //slot item alpha
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    public void AddItem(Item _item)
    {
        item = _item;
        itemImage.sprite = item.itemImage;

        SetColor(1);

        voidState = false;
    }

    public void RemoveItem(Item _item)
    {
        item = null;
        itemImage.sprite = null;
        SetColor(0);
    }

    public void EquipmentItem()
    {
        if (item != null)
        {
            if (item.itemType == Item.ItemType.Equipment)
            {

            }
        }
    }

    public void ClearSlot()
    {
        item = null;

        itemImage.sprite = null;
        SetColor(0);

        voidState = true;
    }

    public bool IsVoid()
    {
        return voidState;
    }
}

