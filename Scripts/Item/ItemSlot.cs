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
    // 스킬 이미지 로드
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
    // 스킬 이미지 초기화
    public void SkillSpriteReset()
    {
        skillBookImage.sprite = null;
        skillBookImage.color = new Color(1, 1, 1, 0);
    }
    // 스킬 이미지 설정
    public void SkillSpriteInit()
    {
        skillBookImage.sprite = GameManager.instance.coolTime.skillSprites[item.skillNum - 1];
        skillBookImage.color = new Color(1, 1, 1, 1);
        skillBookImage.SetNativeSize();
    }
    private void EnchantItemSelecet()
    {
       
        switch (slotType) // 슬롯의 형태에 따라 다른 이벤트
        {
            case SlotType.EnchantItemSpace: // 인첸트 슬롯을 눌렀을 때
                AudioManager.instance.SelectSfx();
                enchant.EnchantItemoff(); // 인첸트 슬롯에 있는 아이템을 해제함
                break;
            case SlotType.EnchantWaitSpace: // 대기 슬롯을 눌렀을 때
                if (enchant.itemSelect) // 인첸트 슬롯에 아이템이 있을 때
                {
                    AudioManager.instance.SelectSfx();
                    enchant.EnchantMaterialSelect(this);
                }
                else // 인첸트 슬롯에 아이템이 없을 때
                {
                    if (enchant.EnchantItemOn(this)) // 아이템을 인첸스 슬롯에 넣음 (가능하면 true, 불가능 하면 false)
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
            if (item.type == ItemType.Book || item.type == ItemType.Staff) // 아이템 슬롯에 빈자리게 있는지 확인
            {
                int waitItemNum = 0;
                for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
                {
                    if (!ItemDatabase.instance.Set(i).isEquip)
                    {
                        waitItemNum++;
                    }
                }

                if (waitItemNum >= GameManager.instance.inventory.waitEqquipments.Length) // 빈자리가 없다면 경고 메세지와 함께 구매 X
                {
                    shop.WarningTextOn(ShopWarningText.InventoryFull);
                    AudioManager.instance.PlayerSfx(Sfx.BuySellFail);
                    return;
                }
            }
            AudioManager.instance.PlayerSfx(Sfx.BuySell); // 효과음
            GameManager.instance.gold -= itemPrice;
            switch (item.type) // 어떤 아이템을 구매 했는지 ?
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
            shop.soldOutTexts[(int)item.type - 1].SetActive(true); // 팔림 표시
            item = null; // 구매 아이템은 null 처리
            ImageLoading(); // 이미지 로딩
        }
        else
        {
            shop.WarningTextOn(ShopWarningText.GoldEmpty); // 돈 없음 텍스트
            AudioManager.instance.PlayerSfx(Sfx.BuySellFail);
        }
    }
    private void ItemSell()
    {
        if (item.type == ItemType.Staff) // 판매 아이템이 스태프 일 경우
        {
            int staffNum = 0;
            for (int i = 0; i < ItemDatabase.instance.itemCount(); i++) // 인벤토리에 스태프가 하나 밖에 없다면 판매 X
            {
                if (ItemDatabase.instance.Set(i).type == ItemType.Staff)
                {
                    staffNum++;
                }
            }

            if (staffNum < 2) // 갯수가 하나일 때
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
                if (item.isEquip) // 장착중인 아이템이라면 
                {
                    if (item.type == ItemType.Book) // 마법책
                    {
                        GameManager.instance.inventory.SkillBookInit();
                        item.reset();
                        GameManager.instance.inventory.SkillBookActive();
                    }
                    else // 스태프
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
        shop.WarningTextOn(ShopWarningText.ItemSell); // 아이템 판매 텍스트
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
        if (GameManager.instance.inventory.equipReady && (slotType == SlotType.Main || slotType == SlotType.Sub)) // 장비 장착
        {
            GameManager.instance.inventory.Equipment(this);
            preview.gameObject.SetActive(false);
            AudioManager.instance.SelectSfx(); // 효과음
        }
        else if (item.itemSprite != null)
        {
            if (slotType == SlotType.WaitSpace) // 장착할 아이템 선택 
            {
                GameManager.instance.inventory.EquipReady(this);
            }
            else // 장비 해제
            {
                GameManager.instance.inventory.UnEquipReady(this);
            }
            AudioManager.instance.SelectSfx(); // 효과음
        }
        else // 다른 비어있는 슬롯을 눌렀을 때
        {
            preview.gameObject.SetActive(false);
            if (GameManager.instance.inventory.equipReady)
            {
                GameManager.instance.inventory.EquipReadyCancel();
            }
            AudioManager.instance.SelectSfx(); // 효과음
        }

    }
    public void OnPointerClick(PointerEventData eventData) // 터치 이벤트
    {
        if (item != null && eventData.button == PointerEventData.InputButton.Left)
        {
            PreviewItemOn();

            switch (slotType)
            {
                case SlotType.Main:
                case SlotType.Sub:
                case SlotType.WaitSpace: // 인벤토리 터치 이벤트
                    InventoryTouchEvents();
                    break;
                case SlotType.EnchantItemSpace: // 인첸트 창 터치 이벤트
                case SlotType.EnchantWaitSpace:
                    EnchantTouchEvents();
                    break;
                case SlotType.ShopSpace: // 상점 터치 이벤트
                case SlotType.SellSpace:
                    ShopTouchEvents();
                    break;
            }
        }
    }
    public void ItemPriceLoad() // 아이템 가격 
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
