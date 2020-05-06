using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New item", menuName = "New item/item")]
public class Item : ScriptableObject
{
    public int itemCode; 
    public string itemName;
    public ItemType itemType;
    public Sprite itemImage;
    public GameObject itemPrefab; 

    public string weaponType;

    public enum ItemType
    {
        Equipment, 
        Used,       
        Read,       
        Puzzle,     
        Doll,
        One_time,
    }

}




