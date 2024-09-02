using UnityEngine;

public enum ItemType { Default, Staff, Book , Esscence, Posion }
public enum ItemRank { Default, Common, Rare, Epic, Legendary }
public enum ItemQuality { Default, Low, Normal, High }
public enum ItemAttribute {Default, Non, Fire, Water, Eeath , Grass , Dark , Holy }

public class ItemInfo : MonoBehaviour
{


    [Header("# Item Infomation")]
    public ItemType type;
    public ItemRank rank;
    public ItemQuality quality;

    [Header("Common Staff")]
    public Sprite[] commonStaffImgaes;
    [Header("Rare Staff")]
    public Sprite[] rareStaffImgaes;
    [Header("Epic Staff")]
    public Sprite[] epicStaffImgaes;
    [Header("Legendary Staff")]
    public Sprite[] legendaryStaffImgaes;

    [Header("Staff Desc")]
    public string[] staffNames; 
    public string[] staffDescs;
    public string[] legendaryStaffNames;
    public string[] legendaryStaffDescs;




    [Header("# Item Base Stat")]
    public int[] baseAttack;
    public float[] baseRate;
    public float[] MoveSpeed;


    [Header("# Item Increase Stat")]
    public int[] increaseAttack;
    public float[] increaseRate;
    public float[] increaseMoveSpeed;

    [Header("# Magic Book")]
    public Sprite magicBookSprite;
    public string[] aditionalAbility;


}
