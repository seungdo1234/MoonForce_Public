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


    // createType�� �������� �����ϴ� ���� (0�� ������, 1�� ����å , 2�� ���� ������, 3�� ���� ����å)
    public void ItemCreate(int itemRank, int createType) // ���� ������ ����
    {
        this.createType = createType;
        this.itemRank = itemRank;

        if (itemRank > -1)
        {
            switch (createType)
            {
                // ������
                case 0:
                case 2:
                    rank = (ItemRank)itemRank + 1;
                    type = ItemType.Staff;
                    if (rank == ItemRank.Legendary) // �����帮�� ������ ǰ�� ��
                    {
                        quality = ItemQuality.High;
                    }
                    else
                    {
                        // ������ ǰ�� ���ϱ�
                        int value = ChestManager.instance.Percent(ChestManager.instance.qualityPer[GameManager.instance.spawner.spawnPerLevelUp].percent);
                        quality = (ItemQuality)value + 1;
                    }
                    break;
                // ����å
                case 1:
                case 3:
                    quality = (ItemQuality)itemRank + 1;
                    type = ItemType.Book;
                    break;
            }
        }
        else // ó�� ���� ������ �� ������, ���� å ����
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

        if (type == ItemType.Staff) // ������ ����
        {
            StaffCreate();
        }
        else // ����å ����
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
        else // ���� ���� �� ������ ������ �Ӽ� 
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

        if (createType == 0) // 0�϶� (����)
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
        else // 2�� ��  (����)
        {
            Item item = new Item();
            item.Staff(type, rank, quality, itemSprite, itemAttribute, itemName, attack, rate, moveSpeed, desc, skillNum);
            GameManager.instance.shop.StaffCreate(item);
        }
    }
    private void BookCreate() // ���� å ����
    {
        // ��ų ��ȣ
        if (itemRank > -1)
        {
            skillNumber = Random.Range(7, GameManager.instance.magicManager.magicInfo.Length);
        }
        else // ���� ���� �� ĳ���� �� ����
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

        // ǰ�� ���� �ο��Ǵ� ������ ������ �ٸ�
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
    private void SetStaffStat() // Quality�� ���� Attack���� , Rate �� ����
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


