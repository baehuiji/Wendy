using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    public int outlineIndex;

    public int get_itemCode()
    {
        return item.itemCode;
    }
}
