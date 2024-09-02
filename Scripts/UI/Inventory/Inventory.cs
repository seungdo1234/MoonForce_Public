using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public enum InventoryTextType { ItemEquip, ItemUnEquip}
public class Inventory : MonoBehaviour
{
    [Header("# Item Slot")]
    public ItemSlot mainEqquipment;
    public ItemSlot[] subEqquipments;
    public ItemSlot[] waitEqquipments;

    [Header("# Equipment")]
    public ItemSlot equipWaitItemSlot;
    public bool equipReady;
    public RectTransform[] selectAnimations;
    public Text noticeText;
    public string[] noticeTextType;
    public Vector3 selecetAnimationPos;

    [Header("# Uranus Skill")]
    public HorizontalLayoutGroup subEquipmentsLayout;
    public GameObject uranusSlot;
    public bool uranusEqiup;


    [Header("# Skill Books")]
    public Item prevEquipStaff ; // ���� ������ ������
    [SerializeField]
    private List<Item> equipBooks = new List<Item>(); // ���� ������ ����å�� ���ִ� ����Ʈ
    private Item item;

    private void OnEnable()
    {
        equipWaitItemSlot = null;
        GetItems();
    }

    private void GetItems() // ������ ���̽����� �������� ������
    {
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            item = ItemDatabase.instance.Set(i);
            if (!item.isLoad) // �̹� �κ��丮�� �ε�� �������̶�� �ε尡 �ȵǰ� ��
            {
                item.isLoad = true;
                for (int j = 0; j < waitEqquipments.Length; j++) // �������� �� ������ ������ ã�� ���� 
                {
                    if ( waitEqquipments[j].item.itemSprite == null)
                    {
                        waitEqquipments[j].item = item;
                        break;
                    }
                }
            }
        }

        StartCoroutine(LoadImages());
    }

    public void UranusOn()
    {
        subEquipmentsLayout.spacing = 0;

        uranusSlot.SetActive(true);

        uranusEqiup = true;
    }

    public void UranusOff()
    {
        uranusEqiup = false;
        uranusSlot.SetActive(false);
        subEquipmentsLayout.spacing = -100;

        if (subEqquipments[3].item != null && subEqquipments[3].item.itemSprite != null)
        {
            UnranusBookUnEquip();
        }
        GetItems();
    }
    private void UnranusBookUnEquip()
    {
        for (int i = 0; i < waitEqquipments.Length; i++) // �������� �� ������ ������ ã�� ���� 
        {
            if (waitEqquipments[i].item.itemSprite == null)
            {
                Item item = subEqquipments[3].item;
                subEqquipments[3].item = waitEqquipments[i].item;
                waitEqquipments[i].item = item;
                break;
            }
        }
        subEqquipments[3].ImageLoading();

        EquipBooks();
    }
    private IEnumerator LoadImages()
    {
        // �̹��� �ε��� ���� �����ӱ��� ����
        yield return null;

        mainEqquipment.ImageLoading();

        for (int i = 0; i < subEqquipments.Length ; i++)
        {
            if (i == subEqquipments.Length - 1 && !uranusEqiup)
            {
                break;
            }
            subEqquipments[i].ImageLoading();
        }

        for (int i = 0; i < waitEqquipments.Length; i++)
        {
            waitEqquipments[i].ImageLoading();
        }

    }
    private void Notice(bool isActive , InventoryTextType type)
    {
        if (isActive)
        {
            noticeText.gameObject.SetActive(true);
            noticeText.text = noticeTextType[(int)type];
        }
        else
        {
            noticeText.gameObject.SetActive(false);
        }
    }
    public void InventoryReset() // ���� Ŭ����, ���� ����, ���θ޴��� �� �� �κ��丮 �ʱ�ȭ
    {
        SkillBookInit();
        equipBooks.Clear(); // ����Ʈ �ʱ�ȭ

        if (mainEqquipment.item.itemSprite != null)
        {
            mainEqquipment.item.reset();
            mainEqquipment.ImageLoading();
            EquipStaff();
        }

        for (int i = 0; i < GameManager.instance.inventory.subEqquipments.Length; i++)
        {
            if (subEqquipments[i].item.itemSprite != null)
            {
                GameManager.instance.magicManager.magicInfo[subEqquipments[i].item.skillNum].isMagicActive = false;
                subEqquipments[i].item.reset();
                subEqquipments[i].ImageLoading();
            }
        }


        for (int i = 0; i < waitEqquipments.Length; i++)
        {
            if (waitEqquipments[i].item.itemSprite != null)
            {
                waitEqquipments[i].item.reset();
                waitEqquipments[i].ImageLoading();
            }
        }
    }
    public void EquipReady(ItemSlot itemSlot)
    {
        Notice(true, InventoryTextType.ItemEquip);

         equipReady = true;
        equipWaitItemSlot = itemSlot;

        EquipmentAnimationInit();

        if (equipWaitItemSlot.item.type == ItemType.Staff)
        {
            StaffEquipmentAnimation();
        }
        else
        {
            BookEquipmentAnimation();
        }
    }
    public void EquipReadyCancel()
    {
        equipReady = false;
        equipWaitItemSlot = null;

        Notice(false, InventoryTextType.ItemEquip);

        EquipmentAnimationInit();
    }
    public void Equipment(ItemSlot equipSlot)
    {

        if ((equipSlot.slotType == SlotType.Main && equipWaitItemSlot.item.type == ItemType.Staff) ||(equipSlot.slotType == SlotType.Sub && equipWaitItemSlot.item.type == ItemType.Book))
        {
            Item temp = equipWaitItemSlot.item;
            equipWaitItemSlot.item = equipSlot.item;
            equipSlot.item = temp;

            equipWaitItemSlot.ImageLoading();
            equipSlot.ImageLoading();

            if(equipSlot.item.type == ItemType.Staff)
            {
                EquipStaff();
            }
            else
            {
                EquipBooks();
            }
        }

        EquipReadyCancel();
    }
    public void UnEquipReady(ItemSlot unEquipSlot)
    {
        if(equipWaitItemSlot != null  && equipWaitItemSlot == unEquipSlot)
        {
            for(int i =0; i<waitEqquipments.Length; i++)
            {
                if(waitEqquipments[i].item.itemSprite == null)
                {

                    Item temp = waitEqquipments[i].item;
                    waitEqquipments[i].item = equipWaitItemSlot.item;
                    equipWaitItemSlot.item = temp;

                    waitEqquipments[i].ImageLoading();
                    equipWaitItemSlot.ImageLoading();

                    if (equipWaitItemSlot.slotType == SlotType.Main)
                    {
                        EquipStaff();
                    }
                    else
                    {
                        EquipBooks();
                    }

                    equipWaitItemSlot.preview.gameObject.SetActive(false);
                    Notice(false, InventoryTextType.ItemEquip);

                    equipWaitItemSlot = null;
                    return;
                }
            }
        }
        else
        {
            Notice(true, InventoryTextType.ItemUnEquip);
            equipWaitItemSlot = unEquipSlot;
        }

    }
    private void StaffEquipmentAnimation() // ������ ���� �� ���� ���� �ִϸ��̼�
    {
        // Staff ������ �ִϸ��̼Ǹ� Ȱ��ȭ
        selectAnimations[0].gameObject.SetActive(true);

        // Staff ���Կ� �ִϸ��̼� ����
        selectAnimations[0].SetParent(mainEqquipment.transform, false);
        selectAnimations[0].transform.localPosition = selecetAnimationPos;
    }

    private void BookEquipmentAnimation() // ����å ���� �� ���� ���� �ִϸ��̼�
    {
        for (int i = 0; i < subEqquipments.Length; i++)
        {
            if (i == subEqquipments.Length - 1 && !uranusSlot.activeSelf)
            {
                break;
            }

            // �� ���� ���Կ� ���� �ִϸ��̼� Ȱ��ȭ
            selectAnimations[i].gameObject.SetActive(true);

            // �ش� ���� ���Կ� �ִϸ��̼� ����
            selectAnimations[i].SetParent(subEqquipments[i].transform, false);
            selectAnimations[i].transform.localPosition = selecetAnimationPos;
        }
    }

    public void EquipmentAnimationInit() // �ִϸ��̼� ������Ʈ�� �ʱ�ȭ
    {
        for(int i =0; i <selectAnimations.Length; i++)
        {
            selectAnimations[i].gameObject.SetActive(false);
        }
    }
    public void SkillBookInit()
    {
        for (int i = 0; i < equipBooks.Count; i++) // ���� ������ ���� å ��ų �ʱ�ȭ
        {
            GameManager.instance.magicManager.magicInfo[equipBooks[i].skillNum].isMagicActive = false;
            equipBooks[i].isEquip = false;
            MagicAdditionalStat(equipBooks[i], -1);
        }
    }
    public void SkillBookActive()
    {

        equipBooks.Clear(); // ����Ʈ �ʱ�ȭ

        for (int i = 0; i < subEqquipments.Length; i++)
        {
            if (i == subEqquipments.Length - 1 && !uranusEqiup)
            {
                break;
            }

            if (subEqquipments[i].item != null && subEqquipments[i].item.itemSprite != null) // ������ ���� å ���� Ȱ��ȭ
            {
                GameManager.instance.magicManager.magicInfo[subEqquipments[i].item.skillNum].isMagicActive = true;
                MagicAdditionalStat(subEqquipments[i].item, 1);
                equipBooks.Add(subEqquipments[i].item);
                subEqquipments[i].item.isEquip = true;
            }
        }
    }
    public void EquipBooks() // ���� ����å ����
    {

        SkillBookInit();

        SkillBookActive();
    }

    private void MagicAdditionalStat(Item item , int operation) // ����å �߰� �ɷ� ����
    {

        for (int i = 0; i < item.aditionalAbility.Length; i++) 
        {

            switch (item.aditionalAbility[i])
            {
                case 0:
                    GameManager.instance.magicManager.magicInfo[item.skillNum].damagePer += GameManager.instance.magicManager.magicInfo[item.skillNum].damageIncreaseValue * operation;
                    break;
                case 1:

                    if (GameManager.instance.magicManager.magicInfo[item.skillNum].magicCoolTime == 0)
                    {
                        GameManager.instance.magicManager.magicInfo[item.skillNum].magicRateStep+= operation;
                    }
                    else
                    {
                        if(GameManager.instance.magicManager.magicInfo[item.skillNum].magicCoolTime - GameManager.instance.magicManager.magicInfo[item.skillNum].coolTimeDecreaseValue * operation >= 0)
                        {
                            GameManager.instance.magicManager.magicInfo[item.skillNum].magicCoolTime -= GameManager.instance.magicManager.magicInfo[item.skillNum].coolTimeDecreaseValue * operation;
                        }
                    }
                    break;
                case 2:
                    if (GameManager.instance.magicManager.magicInfo[item.skillNum].magicCountIncrease)
                    {
                        GameManager.instance.magicManager.magicInfo[item.skillNum].magicCount+= operation;
                    }
                    else
                    {
                        GameManager.instance.magicManager.magicInfo[item.skillNum].magicSizeStep+= operation;
                    }
                    break;
            }
        }
        GameManager.instance.statManager.weaponNum+= operation ;
    }
    public void EquipStaff() // ������ ����
    {

        prevEquipStaff.isEquip = false;
        mainEqquipment.item.isEquip = true;
        // �ɷ�ġ ����
        GameManager.instance.attribute = mainEqquipment.item.itemAttribute;

        GameManager.instance.statManager.attack = GameManager.instance.statManager.baseAttack + mainEqquipment.item.attack;
        if (GameManager.instance.attribute == ItemAttribute.Dark)
        {
            GameManager.instance.statManager.rate = GameManager.instance.statManager.baseRate * 2 - mainEqquipment.item.rate;
        }
        else
        {
            GameManager.instance.statManager.rate = GameManager.instance.statManager.baseRate - mainEqquipment.item.rate;
        }
        GameManager.instance.statManager.moveSpeed = GameManager.instance.statManager.baseMoveSpeed + mainEqquipment.item.moveSpeed;


        if (GameManager.instance.statManager.essenceOn) // �������� �������̶��
        {
            GameManager.instance.statManager.EssenceOn(GameManager.instance.statManager.activeEssenceNum, GameManager.instance.statManager.essenceStat[GameManager.instance.statManager.activeEssenceNum]);   
        }

            if (prevEquipStaff.rank == ItemRank.Legendary) // ���������� �������� ������ ������ �̶�� �ش� ��ų Off
        {

            if (prevEquipStaff.skillNum != 0)
            {
                GameManager.instance.magicManager.magicInfo[prevEquipStaff.skillNum].isMagicActive = false;
            }
            else
            {
                UranusOff();
            }
        }

        if (mainEqquipment.item.rank == ItemRank.Legendary) // ������ �������� �����帮 �������̶�� ��ų On
        {
            if(mainEqquipment.item.skillNum != 0)
            {
                GameManager.instance.magicManager.magicInfo[mainEqquipment.item.skillNum].isMagicActive = true;
            }
            else
            {
                UranusOn();
            }
        }

        prevEquipStaff = mainEqquipment.item;    // ������ ������ �����͸� ����
    }


}
