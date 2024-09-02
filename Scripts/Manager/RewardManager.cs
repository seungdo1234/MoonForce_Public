using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public ItemInfo itemInfo;

    public ItemSlot rewardSlot;

    private ItemType type;
    private ItemRank rank;
    private ItemQuality quality;
    private Sprite itemSprite;
    private ItemAttribute itemAttribute;
    private string itemName;
    private string desc;
    private int attack;
    private float rate;
    private float moveSpeed;
    private int skillNumber;
    private int[] aditionalAbility;
    private int createType;
    private int itemRank;
    private void Start()
    {

    }


    // createType은 아이템을 생성하는 형태 (0번 스태프, 1번 마법책 , 2번 상점 스태프, 3번 상점 마법책)
    public void ItemCreate(int itemRank, int createType) // 랜덤 아이템 생성
    {
        this.createType = createType;
        this.itemRank = itemRank;

        if (itemRank > -1)
        {
            switch (createType)
            {
                // 스태프
                case 0:
                case 2:
                    rank = (ItemRank)itemRank + 1;
                    type = ItemType.Staff;
                    if (rank == ItemRank.Legendary) // 레전드리는 무조건 품질 상
                    {
                        quality = ItemQuality.High;
                    }
                    else
                    {
                        // 스태프 품질 정하기
                        int value = ChestManager.instance.Percent(ChestManager.instance.qualityPer[GameManager.instance.spawner.spawnPerLevelUp].percent);
                        quality = (ItemQuality)value + 1;
                    }
                    break;
                // 마법책
                case 1:
                case 3:
                    quality = (ItemQuality)itemRank + 1;
                    type = ItemType.Book;
                    break;
            }
        }
        else // 처음 게임 시작할 때 스태프, 마법 책 장착
        {
            quality = ItemQuality.Low;
            if (itemRank % -2 == -1)
            {
                type = ItemType.Staff;
                rank = ItemRank.Common;
                SetStaffStat();
            }
            else
            {
                type = ItemType.Book;
            }
        }

        if (type == ItemType.Staff) // 스태프 생성
        {
            StaffCreate();
        }
        else // 마법책 생성
        {
            BookCreate();
        }

    }
    public void StaffCreate()
    {
        int skillNum = -1;

        int rankNum = this.rank == ItemRank.Legendary ? 0 : -1;

        if (itemRank > -1)
        {
            itemAttribute = (ItemAttribute)Random.Range(1, System.Enum.GetValues(typeof(ItemAttribute)).Length + rankNum);
        }
        else // 게임 시작 시 나오는 스태프 속성 
        {
            switch (itemRank)
            {
                case -1:
                    itemAttribute = ItemAttribute.Non;
                    break;
                case -3:
                    itemAttribute = ItemAttribute.Fire;
                    break;
                case -5:
                    break;
                case -7:
                    break;
            }
        }

        SetStaffStat();

        if (rank != ItemRank.Legendary)
        {
            itemName = itemInfo.staffNames[(int)itemAttribute - 1];
            desc = itemInfo.staffDescs[(int)itemAttribute - 1];
            moveSpeed = 0;
        }

        switch (rank)
        {
            case ItemRank.Common:
                itemSprite = itemInfo.commonStaffImgaes[(int)itemAttribute - 1];
                break;
            case ItemRank.Rare:
                itemSprite = itemInfo.rareStaffImgaes[(int)itemAttribute - 1];
                break;
            case ItemRank.Epic:
                itemSprite = itemInfo.epicStaffImgaes[(int)itemAttribute - 1];
                break;
            case ItemRank.Legendary:
                itemSprite = itemInfo.legendaryStaffImgaes[(int)itemAttribute - 1];
                itemName = itemInfo.legendaryStaffNames[(int)itemAttribute - 1];
                desc = itemInfo.legendaryStaffDescs[(int)itemAttribute - 1];
                skillNum = (int)itemAttribute - 1;
                break;
        }

        if (createType == 0) // 0일때 (보상)
        {
            ItemDatabase.instance.GetStaff(type, rank, quality, itemSprite, itemAttribute, itemName, attack, rate, moveSpeed, desc, skillNum);
            if (itemRank > -1)
            {
                rewardSlot.item = ItemDatabase.instance.Set(ItemDatabase.instance.itemCount() - 1);
                rewardSlot.itemImage.sprite = rewardSlot.item.itemSprite;
            }
            else
            {
                GameManager.instance.inventory.mainEqquipment.item = ItemDatabase.instance.Set(0);
                GameManager.instance.inventory.mainEqquipment.item.isLoad = true;
                GameManager.instance.inventory.EquipStaff();
            }

        }
        else // 2일 때  (상점)
        {
            Item item = new Item();
            item.Staff(type, rank, quality, itemSprite, itemAttribute, itemName, attack, rate, moveSpeed, desc, skillNum);
            GameManager.instance.shop.StaffCreate(item);
        }
    }
    private void BookCreate() // 마법 책 생성
    {
        // 스킬 번호
        if (itemRank > -1)
        {
            skillNumber = Random.Range(7, GameManager.instance.magicManager.magicInfo.Length);
        }
        else // 게임 시작 시 캐릭터 별 마법
        {
            switch (itemRank)
            {
                case -2:
                    skillNumber = (int)MagicName.Shovel;
                    break;
                case -4:
                    skillNumber = (int)MagicName.FireBall;
                    break;
                case -6:
                    break;
                case -8:
                    break;
            }
        }

        // 품질 별로 부여되는 마법의 갯수가 다름
        aditionalAbility = new int[(int)quality];

        for (int i = 0; i < aditionalAbility.Length; i++)
        {
            int rand = Random.Range(0, itemInfo.aditionalAbility.Length);

            aditionalAbility[i] = rand;
        }


        itemName = GameManager.instance.magicManager.magicInfo[skillNumber].magicName;
        itemSprite = itemInfo.magicBookSprite;

        if (createType == 1)
        {
            ItemDatabase.instance.GetBook(type, quality, itemSprite, itemName, skillNumber, aditionalAbility);
            if (itemRank > -1)
            {
                rewardSlot.item = ItemDatabase.instance.Set(ItemDatabase.instance.itemCount() - 1);
                rewardSlot.itemImage.sprite = rewardSlot.item.itemSprite;
            }
            else
            {
                GameManager.instance.inventory.subEqquipments[0].item = ItemDatabase.instance.Set(1);
                GameManager.instance.inventory.subEqquipments[0].item.isLoad = true;
                GameManager.instance.inventory.EquipBooks();
            }
        }
        else
        {
            Item item = new Item();
            item.Book(type, quality, itemSprite, itemName, skillNumber, aditionalAbility);
            GameManager.instance.shop.SkillBookCreate(item);
        }
    }
    private void SetStaffStat() // Quality에 따라 Attack값과 , Rate 값 대입
    {
        if (quality == ItemQuality.Low)
        {
            attack = itemInfo.baseAttack[(int)rank - 1];
        }
        else if (quality == ItemQuality.Normal)
        {
            attack = itemInfo.baseAttack[(int)rank - 1] + itemInfo.increaseAttack[(int)rank - 1];
            //    rate = itemInfo.baseRate[(int)rank - 1] + itemInfo.increaseRate[(int)rank - 1];
        }
        else if (quality == ItemQuality.High)
        {
            attack = itemInfo.baseAttack[(int)rank - 1] + itemInfo.increaseAttack[(int)rank - 1] * 2;
            //   rate = itemInfo.baseRate[(int)rank - 1] + itemInfo.increaseRate[(int)rank - 1] * 2;
        }

        rate = itemInfo.baseRate[(int)rank - 1];


        if (itemAttribute == ItemAttribute.Dark)
        {
            attack += Mathf.FloorToInt((float)attack * 0.5f);

            rate += 0.1f;
        }


    }

}


