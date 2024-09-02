using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShopWarningText { GoldEmpty, InventoryFull, OneStaffHave, Healing, Essence , ItemSell, Ready } // �˸� ����
public class Shop : MonoBehaviour
{
    private ItemSlot shopWaitSlot;

    [Header("# Buy")]
    public ItemSlot staffSlot; // ������ ����
    public ItemSlot skillBookSlot; // ����å ����
    public ItemSlot essenceSlot; // ���� ����;
    public ItemSlot posionSlot; // ���� ���� ����
    public StageClear stageClear; // ������ ���� ������Ʈ
    public Text goldText; // ���� ��� �ؽ�Ʈ
    public GameObject[] soldOutTexts; // �ȸ� ǥ��

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

        // ���� �� ������ ����
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

        // ����å ����
        stageClear.ShopItemCreate(ChestManager.instance.qualityPer[GameManager.instance.spawner.spawnPerLevelUp].percent, 3);

    }
    public void ShopReset() // ���� �ʱ�ȭ (�� �������� Ŭ���� �� ����)
    {
        // ���� �ʱ�ȭ
        reRollNum = 0;
        reRollPriceText.text = string.Format("{0}", reRollPrices[reRollNum]);

        Staff_Book_Create();

        stageClear.ShopItemCreate(GameManager.instance.shopManager.healingPosionPercent,  -1); // ���� ����

        EssenceCreate();

        SoldOutTextOff();
    }
    public void SoldOutTextOff() // �ǸſϷ� �ؽ�Ʈ ��Ȱ��ȭ
    {
        for (int i = 0; i < soldOutTexts.Length; i++)
        {
            soldOutTexts[i].SetActive(false);
        }
    }
    public void ReRoll() // ����
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
    public void StaffCreate(Item item) // ������ ����
    {
        staffSlot.item = item;
        staffSlot.ImageLoading();
        staffSlot.ItemPriceLoad();
    }
    public void SkillBookCreate(Item item) // ����å ����
    {
        skillBookSlot.item = item;
        skillBookSlot.ImageLoading();
        skillBookSlot.ItemPriceLoad();
    }
    private void EssenceCreate() // ���� ����
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
    public void PosionCreate(int posionType) // ���� ���� ����
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
    public void WarningTextOn(ShopWarningText warning) // �˸��� �ؽ�Ʈ ����
    {
        warningTextObject.gameObject.SetActive(false);

        warningTextObject.text = warningTexts[(int)warning];

        warningTextObject.gameObject.SetActive(true);
    }
    public bool ShopReady(ItemSlot shopSlot) //  ��þƮ ������ ���� ��� ����
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
        // ���� ��ư Ȱ��ȭ ��Ȱ��ȭ
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

        // ���� ���
        goldText.text = string.Format("{0}", GameManager.instance.gold);
    }

    
}
