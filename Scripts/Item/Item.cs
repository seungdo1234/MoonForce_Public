using UnityEngine;

[System.Serializable]
public class Item 
{
    public bool isLoad;
    public bool isEquip;

    [Header("BaseInfo")]
    public ItemType type;
    public ItemRank rank;
    public ItemQuality quality;
    public Sprite itemSprite;
    public ItemAttribute itemAttribute;
    [Header("Staff")]
    public string itemName;
    public int attack;
    public float rate;
    public float moveSpeed;
    public string itemDesc;
    public int enchantStep;
    [Header("Book")]
    public string bookName;
    public int skillNum;
    public int[] aditionalAbility;



    public void Staff(ItemType type, ItemRank rank, ItemQuality quality, Sprite itemSprite, ItemAttribute itemAttribute,string itemName, int attack, float rate, float moveSpeed, string itemDesc, int skillNum)
    {
        this.type = type;
        this.rank = rank;
        this.quality = quality;
        this.itemSprite = itemSprite;
        this.itemAttribute = itemAttribute;
        this.itemName = itemName;
        this.attack = attack;  
        this.rate = rate;
        this.moveSpeed = moveSpeed;
        this.itemDesc = itemDesc;
        this.skillNum = skillNum;
    }
    public void Book(ItemType type, ItemQuality quality , Sprite itemSprite ,string bookName ,int skillNum, int[] aditionalAbility)
    {
        this.type = type;
        this.quality = quality;
        this.itemSprite = itemSprite;
        this.bookName = bookName;
        this.skillNum = skillNum;
        this.aditionalAbility = aditionalAbility;
    }
    public void reset() // 아이템 초기화
    {
        isLoad = false;
        type = ItemType.Default; // ItemType과 ItemRank는 각자 기본 값을 설정해주어야 합니다.
        rank = ItemRank.Default;
        quality = ItemQuality.Default;
        itemAttribute = ItemAttribute.Default;
        itemSprite = null;
        itemName = "";
        attack = 0;
        rate = 0f;
        moveSpeed = 0f;
        itemDesc = "";
        skillNum = -1;
        aditionalAbility = null;
    }
}
