using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType { Main, Sub, WaitSpace, EnchantItemSpace, EnchantWaitSpace, EnchantCheck, ShopSpace, SellSpace , RewardSpace }
public class ItemSlot : MonoBehaviour, IPointerClickHandler
{

    public SlotType slotType;

    [Header("# Inventory")]
    public ItemPreview preview;
    public bool fixedPreview;
    public Transform fixedPreviewTransform;
    public Vector3 preiviewPos = new Vector3(0,60,0);

    [Header("# Enchant")]
    public Enchant enchant;

    [Header("# Shop")]
    public Shop_Sell sell;
    public Shop shop;
    private int itemPrice;

    [Header("# Item Info")]
    public Item item;

    public Image itemImage;


    public Image skillBookImage;
    private void Awake()
    {
        itemImage = GetComponent<Image>();

        Image[] image = GetComponentsInChildren<Image>();
        skillBookImage = image[1];
    }
    public void ImageLoading()
    {
        if (item != null && item.itemSprite != null)
        {
            itemImage.sprite = item.itemSprite;
            itemImage.color = new Color(1, 1, 1, 1);

            SkillBookImageLoading();
        }
        else
        {
            if (itemImage != null)
            {
                itemImage.color = new Color(1, 1, 1, 0);
                if (skillBookImage.sprite != null)
                {
                    SkillSpriteReset();
                }
            }
        }
    }
    // ��ų �̹��� �ε�
    public void SkillBookImageLoading()
    {

        switch (item.type)
        {
            case ItemType.Staff:
                if (skillBookImage.sprite != null)
                {
                    SkillSpriteReset();
                }
                break;
            case ItemType.Book:
                if (skillBookImage.sprite == null || skillBookImage.sprite != GameManager.instance.coolTime.skillSprites[item.skillNum - 1])
                {
                    SkillSpriteInit();
                }
                break;
        }
    }
    // ��ų �̹��� �ʱ�ȭ
    public void SkillSpriteReset()
    {
        skillBookImage.sprite = null;
        skillBookImage.color = new Color(1, 1, 1, 0);
    }
    // ��ų �̹��� ����
    public void SkillSpriteInit()
    {
        skillBookImage.sprite = GameManager.instance.coolTime.skillSprites[item.skillNum - 1];
        skillBookImage.color = new Color(1, 1, 1, 1);
        skillBookImage.SetNativeSize();
    }
    private void EnchantItemSelecet()
    {
       
        switch (slotType) // ������ ���¿� ���� �ٸ� �̺�Ʈ
        {
            case SlotType.EnchantItemSpace: // ��þƮ ������ ������ ��
                AudioManager.instance.SelectSfx();
                enchant.EnchantItemoff(); // ��þƮ ���Կ� �ִ� �������� ������
                break;
            case SlotType.EnchantWaitSpace: // ��� ������ ������ ��
                if (enchant.itemSelect) // ��þƮ ���Կ� �������� ���� ��
                {
                    AudioManager.instance.SelectSfx();
                    enchant.EnchantMaterialSelect(this);
                }
                else // ��þƮ ���Կ� �������� ���� ��
                {
                    if (enchant.EnchantItemOn(this)) // �������� ��þ�� ���Կ� ���� (�����ϸ� true, �Ұ��� �ϸ� false)
                    {
                        AudioManager.instance.SelectSfx();
                        preview.gameObject.SetActive(false);
                    }
                    else
                    {
                        AudioManager.instance.PlayerSfx(Sfx.BuySellFail);
                    }
                }
                break;
        }
    }
    private void EnchantTouchEvents()
    {
        if(item.itemSprite != null)
        {
            if (enchant.EnchantReady(this))
            {
                preview.gameObject.SetActive(false);
                EnchantItemSelecet();
            }
            else
            {
                AudioManager.instance.SelectSfx();
            }
        }
        else
        {
            preview.gameObject.SetActive(false);
            AudioManager.instance.SelectSfx();
            enchant.EnchantReadyCancel();
        }
    }
    private void ItemBuy()
    {
        if (GameManager.instance.gold >= itemPrice)
        {
            if (item.type == ItemType.Book || item.type == ItemType.Staff) // ������ ���Կ� ���ڸ��� �ִ��� Ȯ��
            {
                int waitItemNum = 0;
                for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
                {
                    if (!ItemDatabase.instance.Set(i).isEquip)
                    {
                        waitItemNum++;
                    }
                }

                if (waitItemNum >= GameManager.instance.inventory.waitEqquipments.Length) // ���ڸ��� ���ٸ� ��� �޼����� �Բ� ���� X
                {
                    shop.WarningTextOn(ShopWarningText.InventoryFull);
                    AudioManager.instance.PlayerSfx(Sfx.BuySellFail);
                    return;
                }
            }
            AudioManager.instance.PlayerSfx(Sfx.BuySell); // ȿ����
            GameManager.instance.gold -= itemPrice;
            switch (item.type) // � �������� ���� �ߴ��� ?
            {
                case ItemType.Staff:
                    ItemDatabase.instance.GetStaff(item.type, item.rank, item.quality, item.itemSprite, item.itemAttribute, item.itemName, item.attack, item.rate, item.moveSpeed, item.itemDesc, item.skillNum);
                    break;
                case ItemType.Book:
                    ItemDatabase.instance.GetBook(item.type, item.quality, item.itemSprite, item.bookName, item.skillNum, item.aditionalAbility);
                    break;
                case ItemType.Posion:
                    shop.WarningTextOn(ShopWarningText.Healing);
                    GameManager.instance.statManager.curHealth = Mathf.Min((GameManager.instance.statManager.maxHealth * ((float)item.attack * 0.01f)) + GameManager.instance.statManager.curHealth, GameManager.instance.statManager.maxHealth);
                    break;
                case ItemType.Esscence:
                    shop.WarningTextOn(ShopWarningText.Essence);
                    GameManager.instance.statManager.EssenceOn(item.skillNum, item.rate);
                    break;
            }
            shop.soldOutTexts[(int)item.type - 1].SetActive(true); // �ȸ� ǥ��
            item = null; // ���� �������� null ó��
            ImageLoading(); // �̹��� �ε�
        }
        else
        {
            shop.WarningTextOn(ShopWarningText.GoldEmpty); // �� ���� �ؽ�Ʈ
            AudioManager.instance.PlayerSfx(Sfx.BuySellFail);
        }
    }
    private void ItemSell()
    {
        if (item.type == ItemType.Staff) // �Ǹ� �������� ������ �� ���
        {
            int staffNum = 0;
            for (int i = 0; i < ItemDatabase.instance.itemCount(); i++) // �κ��丮�� �������� �ϳ� �ۿ� ���ٸ� �Ǹ� X
            {
                if (ItemDatabase.instance.Set(i).type == ItemType.Staff)
                {
                    staffNum++;
                }
            }

            if (staffNum < 2) // ������ �ϳ��� ��
            {
                shop.WarningTextOn(ShopWarningText.OneStaffHave);
                AudioManager.instance.PlayerSfx(Sfx.BuySellFail);
                return;
            }
        }

        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            if (ItemDatabase.instance.Set(i) == item)
            {
                if (item.isEquip) // �������� �������̶�� 
                {
                    if (item.type == ItemType.Book) // ����å
                    {
                        GameManager.instance.inventory.SkillBookInit();
                        item.reset();
                        GameManager.instance.inventory.SkillBookActive();
                    }
                    else // ������
                    {
                       item.reset();
                        GameManager.instance.inventory.EquipStaff();
                    }
                    ItemDatabase.instance.ItemRemove(i);
                    break;
                }
                item.reset();
                ItemDatabase.instance.ItemRemove(i);
                break;
            }
        }


        itemImage.sprite = null;
        sell.ItemLoad();

        GameManager.instance.gold += itemPrice;
        AudioManager.instance.PlayerSfx(Sfx.BuySell);
        shop.WarningTextOn(ShopWarningText.ItemSell); // ������ �Ǹ� �ؽ�Ʈ
    }
    private void ShopTouchEvents()
    {
        if (item.itemSprite != null)
        {
            if (shop.ShopReady(this))
            {
                if (slotType == SlotType.ShopSpace)
                {
                    ItemBuy();
                }
                else
                {
                    ItemSell();
                }
                preview.gameObject.SetActive(false);
            }
            else
            {
                AudioManager.instance.SelectSfx();
            }
        }
        else
        {
            preview.gameObject.SetActive(false);
            shop.ShopReadyCancel();
            AudioManager.instance.SelectSfx();
        }
    }
    private void PreviewItemOn()
    {
        if (item.itemSprite != null)
        {
            preview.gameObject.SetActive(true);

            if(fixedPreview && slotType != SlotType.ShopSpace)
            {
                preview.transform.SetParent(fixedPreviewTransform , false) ;
            }
            else {
                preview.transform.SetParent(transform, false);
            }


            preview.transform.localPosition = preiviewPos;

            /*
            if (slotType == SlotType.Main)
            {
                preview.gameObject.transform.localPosition = preiviewPos;
            }
            else if (!fixedPreview)
            {
                preview.gameObject.transform.localPosition = new Vector3(-20, 60, 0);
            }
            else if (fixedPreview)
            {
                if(slotType == SlotType.ShopSpace)
                {
                    preview.gameObject.transform.localPosition =  new Vector3(-70, 60, 0);
                }
                else
                {
                    preview.gameObject.transform.localPosition = new Vector3(0, 60, 0);
                }
            }
            */
            if (slotType == SlotType.SellSpace || slotType == SlotType.ShopSpace)
            {
                preview.IsSell(true, itemPrice);
            }
            else
            {
                preview.IsSell(false, itemPrice);
            }

            preview.ItemInfoSet(item);
        }

    }
    
    private void InventoryTouchEvents()
    {
        if (GameManager.instance.inventory.equipReady && (slotType == SlotType.Main || slotType == SlotType.Sub)) // ��� ����
        {
            GameManager.instance.inventory.Equipment(this);
            preview.gameObject.SetActive(false);
            AudioManager.instance.SelectSfx(); // ȿ����
        }
        else if (item.itemSprite != null)
        {
            if (slotType == SlotType.WaitSpace) // ������ ������ ���� 
            {
                GameManager.instance.inventory.EquipReady(this);
            }
            else // ��� ����
            {
                GameManager.instance.inventory.UnEquipReady(this);
            }
            AudioManager.instance.SelectSfx(); // ȿ����
        }
        else // �ٸ� ����ִ� ������ ������ ��
        {
            preview.gameObject.SetActive(false);
            if (GameManager.instance.inventory.equipReady)
            {
                GameManager.instance.inventory.EquipReadyCancel();
            }
            AudioManager.instance.SelectSfx(); // ȿ����
        }

    }
    public void OnPointerClick(PointerEventData eventData) // ��ġ �̺�Ʈ
    {
        if (item != null && eventData.button == PointerEventData.InputButton.Left)
        {
            PreviewItemOn();

            switch (slotType)
            {
                case SlotType.Main:
                case SlotType.Sub:
                case SlotType.WaitSpace: // �κ��丮 ��ġ �̺�Ʈ
                    InventoryTouchEvents();
                    break;
                case SlotType.EnchantItemSpace: // ��þƮ â ��ġ �̺�Ʈ
                case SlotType.EnchantWaitSpace:
                    EnchantTouchEvents();
                    break;
                case SlotType.ShopSpace: // ���� ��ġ �̺�Ʈ
                case SlotType.SellSpace:
                    ShopTouchEvents();
                    break;
            }
        }
    }
    public void ItemPriceLoad() // ������ ���� 
    {
        if(item == null)
        {
            return;
        }

        switch (item.type)
        {
            case ItemType.Staff:
                if(slotType == SlotType.ShopSpace)
                {
                    itemPrice = GameManager.instance.shopManager.staffRankPrices[(int)item.rank - 1];
                }
                else
                {
                    itemPrice = GameManager.instance.shopManager.staffRankSalePrices[(int)item.rank - 1];
                }
                break;
            case ItemType.Book:
                if (slotType == SlotType.ShopSpace)
                {
                    itemPrice = GameManager.instance.shopManager.bookQualityPrices[(int)item.quality - 1];
                }
                else
                {
                    itemPrice = GameManager.instance.shopManager.bookQualitySalePrices[(int)item.quality - 1];
                }
                break;
            case ItemType.Posion:
                itemPrice = GameManager.instance.shopManager.healingPosions[(int)item.quality - 1].healingPosionPrice;
                break;
            case ItemType.Esscence:
                itemPrice = item.attack;
                break;
        }
    }


    private void Update()
    {


    }
}
