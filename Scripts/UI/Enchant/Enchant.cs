using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public enum EnchantNoticeType { EncahntItemSelect, MaterialSelect,MaterialEmpty, EnchantMaxStep , EnchantReady}
public class Enchant : MonoBehaviour
{
    [SerializeField]
    private List<Item> itemList = new List<Item>();
    public EnchantCheckUI EnchantCheckUI;


    private ItemSlot enchantWaitSlot;

    [Header("NoticeText")]
    public Text enchantNoticeText;
    public string[] noticeType;
    public Animator textAnim;
    public RuntimeAnimatorController textAnimCon;

    [Header("Items")]
    public ItemSlot enchantSpace = null;
    public ItemSlot[] waitSpaces = null;

    public bool itemSelect; // ��þƮ�� �������� �����ߴٸ�
 
    private void OnEnable()
    {
        enchantWaitSlot = null;
        enchantNoticeText.text = noticeType[(int)EnchantNoticeType.EncahntItemSelect];
        itemSelect = false;
        ItemLoad();
        StartCoroutine(LoadImages());
    }

    private void ItemLoad() // �������� ��� ���Կ� ����
    {
        // �������� ��� ���Կ� �ֱ�
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            waitSpaces[i].item = ItemDatabase.instance.Set(i);

        }
    }
    private IEnumerator LoadImages() // ������ �ε�
    {
        // �̹��� �ε��� ���� �����ӱ��� ����
        yield return null;

        enchantSpace.ImageLoading();

        WaitSpaceImageLoading();
    }
    public bool EnchantItemOn(ItemSlot slot) // ��� ������ �������� ��þƮ ���Կ� �ִ� �Լ�
    {
        if (EnchantMaterial(slot.item)) // �κ��丮�� �ش� �������� ��ȭ ��ᰡ �ִٸ� true
        {
            EnchantSelectTextOn(EnchantNoticeType.MaterialSelect);
            itemSelect = true;
            enchantSpace.item = slot.item;
            enchantSpace.ImageLoading();


            WaitSpaceReset();
            EnchantMaterialLoad();
            WaitSpaceImageLoading();

            return true;
        }
        else  // �κ��丮�� �ش� �������� ��ȭ ��ᰡ ���ٸ� false
        {
            return false;
        }
  
    }

    public bool EnchantReady(ItemSlot waitSlot) //  ��þƮ ������ ���� ��� ����
    {
        if (enchantWaitSlot != null && enchantWaitSlot == waitSlot)
        {
            enchantWaitSlot = null;

            return true;
        }
        else
        {
            EnchantSelectTextOn(EnchantNoticeType.EnchantReady);
            enchantWaitSlot = waitSlot;
            return false;
        }
    }
    public void EnchantReadyCancel()
    {
        enchantWaitSlot = null;
        EnchantSelectTextOn(EnchantNoticeType.EncahntItemSelect);
    }
    public void EnchantItemoff() // ��þƮ ���Կ� �ִ� �������� ������
    {
        EnchantSelectTextOn(EnchantNoticeType.EncahntItemSelect);
        itemSelect = false;
        ItemReset();
        ItemLoad();
        StartCoroutine(LoadImages());
    }
    public void ItemReset() // ��þƮ â ����
    {
        //�ʱ�ȭ
        enchantSpace.item = null;
        enchantSpace.itemImage.sprite = null;

        WaitSpaceReset();

    }
    private void WaitSpaceReset() // ��� ���� ����
    {

        for (int i = 0; i < waitSpaces.Length; i++)
        {

            if (waitSpaces[i].item == null)
            {
                break;
            }
            waitSpaces[i].item = null;
            waitSpaces[i].itemImage.sprite = null;
        }
    }
    private void WaitSpaceImageLoading() // ��� ���� �̹��� �ε�
    {
        for (int i = 0; i < waitSpaces.Length; i++)
        {
            waitSpaces[i].ImageLoading();
        }
    }
    private bool EnchantMaterial(Item enchantItem) // ��ȭ ��Ḧ ����Ʈ�� �ֱ� (��ȭ ��ᰡ ������ false)
    {
        if(enchantItem.enchantStep >= EnchantManager.instance.maxEnchantStep) // ��ȭ �ܰ谡 8�̻��̸� ��ȭ X
        {
            EnchantSelectTextOn(EnchantNoticeType.EnchantMaxStep);
            return false;
        }

        itemList.Clear();
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            Item item = ItemDatabase.instance.Set(i);
            if ((((enchantItem.type == ItemType.Staff && item.type == ItemType.Staff) || (enchantItem.type == ItemType.Book && item.skillNum == enchantItem.skillNum)))&& enchantItem != item)
            {
                itemList.Add(item);
            }
        }
        if(itemList.Count != 0)
        {
            return true;
        }
        else
        {
            EnchantSelectTextOn(EnchantNoticeType.MaterialEmpty);
            return false;
        }
    }
    private void EnchantMaterialLoad() // ����Ʈ�� �ִ� �������� ��� ���Կ� �ֱ�
    {
        WaitSpaceReset();

        // �������� ��� ���Կ� �ֱ�
        for (int i = 0; i < itemList.Count; i++)
        {
            waitSpaces[i].item = itemList[i];
        }
    }
    public void EnchantMaterialSelect(ItemSlot slot) // ��þƮ â On
    {
        enchantNoticeText.gameObject.SetActive(false);

        EnchantCheckUI.UIOn(enchantSpace.item , slot.item);

    }
    public void EnchantSelectTextOn(EnchantNoticeType type) // ��þƮ�� �������� ���� �� ������ �ؽ�Ʈ On/Off
    {
        enchantNoticeText.gameObject.SetActive(true);
        enchantNoticeText.text = noticeType[(int)type];

        if(type == EnchantNoticeType.MaterialEmpty || type == EnchantNoticeType.EnchantMaxStep)
        {
            textAnim.runtimeAnimatorController = textAnimCon;
         }
    }
    public void NoiticeTextAnimatorOff()
    {
        textAnim.runtimeAnimatorController = null;
    }
}
