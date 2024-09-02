using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    
    [SerializeField]
    private List<Item> itemDB = new List<Item>();

    private void Awake()
    {
        instance = this;
    }


    public void GetStaff(ItemType type, ItemRank rank, ItemQuality quality, Sprite itemSprite , ItemAttribute itemAttribute,string itemName, int attack, float rate, float moveSpeed, string itemDesc , int skillNum)
    {
        //Item newItem = new Item(type, rank, quality ,itemSprite, itemName, attack, rate, moveSpeed, itemDesc, skillNum,skillDesc);
        Item newItem = new Item();
        newItem.Staff(type, rank, quality, itemSprite, itemAttribute, itemName, attack, rate, moveSpeed, itemDesc, skillNum);
        itemDB.Add(newItem);
    }
    public void GetBook(ItemType type,  ItemQuality quality , Sprite itemSprite, string bookName,int skillNum, int[] aditionalAbility)
    {
        Item newItem = new Item();
        newItem.Book(type, quality, itemSprite, bookName, skillNum, aditionalAbility);
        itemDB.Add(newItem);
    }
    public Item Set(int itemNum)
    {
        return itemDB[itemNum];
    }

    public int itemCount()
    {
        return itemDB.Count;
    }
    public void ItemRemove(int itemNum)
    {
        itemDB.RemoveAt(itemNum);
    }
    public void ItemReset()
    {
        GameManager.instance.inventory.InventoryReset();
        itemDB.Clear();
    }
}
