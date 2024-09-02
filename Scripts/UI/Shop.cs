using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShopWarningText { GoldEmpty, InventoryFull, OneStaffHave, Healing, Essence , ItemSell, Ready } // 알림 종류
public class Shop : MonoBehaviour
{
    private ItemSlot shopWaitSlot;

    [Header("# Buy")]
    public ItemSlot staffSlot; // 스태프 슬롯
    public ItemSlot skillBookSlot; // 마법책 슬롯
    public ItemSlot essenceSlot; // 정수 슬롯;
    public ItemSlot posionSlot; // 힐링 포션 슬롯
    public StageClear stageClear; // 아이템 생성 컴포넌트
    public Text goldText; // 현재 골드 텍스트
    public GameObject[] soldOutTexts; // 팔림 표시

    [Header("# ReRoll")]
    public Button reRollBtn;
    public int reRollNum;
    public int[] reRollPrices;
    public Text reRollPriceText;
    public Sprite[] reRollBtnSprites; 

    [Header("# WarningText")]
    public Text warningTextObject;
    public string[] warningTexts;

    public void Staff_Book_Create()
    {
        int level = GameManager.instance.level / 10;

        // 레벨 별 스태프 생성
        if (level == 0)
        {
            stageClear.ShopItemCreate(ChestManager.instance.bronzeChest, 2);
        }
        else if (level == 1)
        {
            stageClear.ShopItemCreate(ChestManager.instance.silverChest, 2);
        }
        else if (level >= 2)
        {
            stageClear.ShopItemCreate(ChestManager.instance.goldChest,  2);
        }

        // 마법책 생성
        stageClear.ShopItemCreate(ChestManager.instance.qualityPer[GameManager.instance.spawner.spawnPerLevelUp].percent, 3);

    }
    public void ShopReset() // 상점 초기화 (매 스테이지 클리어 시 실행)
    {
        // 리롤 초기화
        reRollNum = 0;
        reRollPriceText.text = string.Format("{0}", reRollPrices[reRollNum]);

        Staff_Book_Create();

        stageClear.ShopItemCreate(GameManager.instance.shopManager.healingPosionPercent,  -1); // 힐링 포션

        EssenceCreate();

        SoldOutTextOff();
    }
    public void SoldOutTextOff() // 판매완료 텍스트 비활성화
    {
        for (int i = 0; i < soldOutTexts.Length; i++)
        {
            soldOutTexts[i].SetActive(false);
        }
    }
    public void ReRoll() // 리롤
    {

        GameManager.instance.gold -= reRollPrices[reRollNum];
        if(reRollNum != reRollPrices.Length - 1)
        {
            reRollNum++;
            reRollPriceText.text = string.Format("{0}", reRollPrices[reRollNum]);
        }

        shopWaitSlot = null;

        Staff_Book_Create();

        soldOutTexts[0].SetActive(false);
        soldOutTexts[1].SetActive(false);

        AudioManager.instance.SelectSfx();
    }
    private void OnEnable()
    {
        shopWaitSlot = null;
        warningTextObject.gameObject.SetActive(false);
    }
    public void StaffCreate(Item item) // 스태프 생성
    {
        staffSlot.item = item;
        staffSlot.ImageLoading();
        staffSlot.ItemPriceLoad();
    }
    public void SkillBookCreate(Item item) // 마법책 생성
    {
        skillBookSlot.item = item;
        skillBookSlot.ImageLoading();
        skillBookSlot.ItemPriceLoad();
    }
    private void EssenceCreate() // 정수 생성
    {
        int rand = Random.Range(0, GameManager.instance.shopManager.essences.Length);

        essenceSlot.item = new Item();
        essenceSlot.item.type = ItemType.Esscence;
        essenceSlot.item.skillNum = rand;
        essenceSlot.item.itemName = GameManager.instance.shopManager.essences[rand].essenceName;
        essenceSlot.item.itemSprite = GameManager.instance.shopManager.essences[rand].essenceSprite;
        essenceSlot.item.rate = GameManager.instance.shopManager.essences[rand].essenceIncreaseStat;
        essenceSlot.item.attack = GameManager.instance.shopManager.essences[rand].essencePrice;
        essenceSlot.ImageLoading();
        essenceSlot.ItemPriceLoad();
    }
    public void PosionCreate(int posionType) // 힐링 포션 생성
    {
        posionSlot.item = new Item();
        posionSlot.item.quality = (ItemQuality)posionType + 1;
        posionSlot.item.type = ItemType.Posion;
        posionSlot.item.itemName = GameManager.instance.shopManager.healingPosions[posionType].healingPosionName;
        posionSlot.item.itemSprite = GameManager.instance.shopManager.healingPosions[posionType].healingPosionSprite;
        posionSlot.item.attack = GameManager.instance.shopManager.healingPosions[posionType].healingPosionRecoveryHealth;

        posionSlot.ImageLoading();
        posionSlot.ItemPriceLoad();
    }
    public void WarningTextOn(ShopWarningText warning) // 알림문 텍스트 생성
    {
        warningTextObject.gameObject.SetActive(false);

        warningTextObject.text = warningTexts[(int)warning];

        warningTextObject.gameObject.SetActive(true);
    }
    public bool ShopReady(ItemSlot shopSlot) //  인첸트 아이템 선택 대기 상태
    {
        if (shopWaitSlot != null && shopWaitSlot == shopSlot)
        {
            shopWaitSlot = null;
            return true;
        }
        else
        {
            WarningTextOn(ShopWarningText.Ready);
            shopWaitSlot = shopSlot;
            return false;
        }
    }
    public void ShopReadyCancel()
    {
        shopWaitSlot = null;
    }
    private void Update()
    {
        // 리롤 버튼 활성화 비활성화
        if(reRollPrices[reRollNum] <= GameManager.instance.gold && !reRollBtn.interactable)
        {
            reRollBtn.interactable = true;
            reRollBtn.image.sprite = reRollBtnSprites[0];
        }
        else if(reRollPrices[reRollNum] > GameManager.instance.gold && reRollBtn.interactable)
        {
            reRollBtn.interactable = false;
            reRollBtn.image.sprite = reRollBtnSprites[1];
        }

        // 현재 골드
        goldText.text = string.Format("{0}", GameManager.instance.gold);
    }

    
}
