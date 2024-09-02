using UnityEngine;
using UnityEngine.UI;

public enum ItemPreviewType { Inventory , EnchantCheck}
public class ItemPreview : MonoBehaviour
{
    public ItemPreviewType type;
    public Text[] itemInfomation;

    public Image coin;
    
    string itemNameColor;
    string itemRankColor;
    string itemQuality;
    string itemAttributeColor;
    string itemAttribute;
    private void Awake()
    {
        itemInfomation = GetComponentsInChildren<Text>(true);
    }

    public void IsSell(bool active , int price)
    {
        if (active)
        {
            itemInfomation[5].text = string.Format("{0}", price);
        }

        itemInfomation[5].gameObject.SetActive(active);
        coin.gameObject.SetActive(active);
    }
    private void BookInfoSet(Item item)
    {
        switch (item.quality)
        {
            case ItemQuality.Low:
                itemQuality = "�ʱ�";
                break;
            case ItemQuality.Normal:
                itemQuality = "�߱�";
                break;
            case ItemQuality.High:
                itemQuality = "���";
                break;
        }

        itemInfomation[2].text = null;
        itemInfomation[3].text = null;

        for (int i = 0; i < item.aditionalAbility.Length; i++)
        {
            if (i > 0)
            {
                itemInfomation[3].text += "\n";
            }
            switch (item.aditionalAbility[i])
            {
                case 0:
                    itemInfomation[3].text += "���� ���ط� ����";
                    break;
                case 1:

                    if (GameManager.instance.magicManager.magicInfo[item.skillNum].magicCoolTime == 0)
                    {
                        itemInfomation[3].text += "���� ���� �ӵ� ����";
                    }
                    else
                    {
                        itemInfomation[3].text += "���� ��Ÿ�� ����";
                    }
                    break;
                case 2:
                    if (GameManager.instance.magicManager.magicInfo[item.skillNum].magicCountIncrease)
                    {
                        itemInfomation[3].text += "���� ��� ���� ����";
                    }
                    else
                    {
                        itemInfomation[3].text += "���� ũ�� ����";
                    }
                    break;
            }
        }
        itemInfomation[0].text = string.Format("<color=black>{0}</color>", item.bookName);
        itemInfomation[1].text = itemQuality;
        itemInfomation[4].text = "����ź ���� + 1";
    }
    private void StaffInfoSet(Item item)
    {
        switch (item.quality)
        {
            case ItemQuality.Low:
                itemNameColor = "green";
                break;
            case ItemQuality.Normal:
                itemNameColor = "blue";
                break;
            case ItemQuality.High:
                itemNameColor = "red";
                break;
        }

        switch (item.rank)
        {
            case ItemRank.Common:
                itemRankColor = "black";
                break;
            case ItemRank.Rare:
                itemRankColor = "blue";
                break;
            case ItemRank.Epic:
                itemRankColor = "purple";
                break;
            case ItemRank.Legendary:
                itemRankColor = "yellow";
                break;
        }

        switch (item.itemAttribute)
        {
            case ItemAttribute.Non:
                itemAttributeColor = "white";
                itemAttribute = "�� �Ӽ�";
                break;
            case ItemAttribute.Fire:
                itemAttributeColor = "orange";
                itemAttribute = "�� �Ӽ�";
                break;
            case ItemAttribute.Water:
                itemAttributeColor = "blue";
                itemAttribute = "�� �Ӽ�";
                break;
            case ItemAttribute.Eeath:
                itemAttributeColor = "brown";
                itemAttribute = "�� �Ӽ�";
                break;
            case ItemAttribute.Grass:
                itemAttributeColor = "green";
                itemAttribute = "Ǯ �Ӽ�";
                break;
            case ItemAttribute.Dark:
                itemAttributeColor = "black";
                itemAttribute = "��� �Ӽ�";
                break;
            case ItemAttribute.Holy:
                itemAttributeColor = "yellow";
                itemAttribute = "�� �Ӽ�";
                break;

        }
        itemInfomation[0].text = string.Format("<color={0}>{1}</color> ", itemNameColor, item.itemName);
        itemInfomation[1].text = string.Format("(<color={0}>{1}</color>)", itemRankColor, item.rank);
        itemInfomation[2].text = string.Format("<color={0}>{1}</color>", itemAttributeColor, itemAttribute);
        if (type == ItemPreviewType.Inventory)
        {
            itemInfomation[3].text = string.Format("���ݷ� + {0}\n���ݼӵ� + {1}%\n{2}", item.attack, Mathf.Floor(item.rate * 100), item.itemDesc);
        }
        else if(type == ItemPreviewType.EnchantCheck)
        {
            itemInfomation[3].text = string.Format("���ݷ� + {0}\n���ݼӵ� + {1}%", item.attack, Mathf.Floor(item.rate * 100));
        }
        itemInfomation[4].text = string.Format("<color=black>��ȭ �ܰ� :</color> <color=red>{0}</color>", item.enchantStep + 1);

    }
   private void PosionInfoSet(Item item)
    {
        itemInfomation[1].text = null;
        itemInfomation[2].text = null;
        itemInfomation[4].text = null;

        itemInfomation[0].text = string.Format("<color=black>{0}</color> ", item.itemName);
        itemInfomation[3].text = string.Format("�ִ�ü���� <color=red>{0}%</color>�� ü���� ȸ���մϴ�.", item.attack);

    }
    private void EssenceInfoSet(Item item)
    {
        string statName = null ;
        switch(item.skillNum)
        {
            case 0:
                statName = "���ݷ���";
                break;
            case 1:
                statName = "�̵��ӵ���";
                break;
            case 2:
                statName = "���ݼӵ���";
                break;
            case 3:
                statName = "�������";
                break;
        }
        itemInfomation[1].text = null;
        itemInfomation[2].text = null;
        itemInfomation[4].text = null;

        itemInfomation[0].text = string.Format("<color=black>{0}</color> ", item.itemName);
        itemInfomation[3].text = string.Format("{0} ���� �������� �������� �����մϴ�.", statName);

    }
    public void ItemInfoSet(Item item)
    {
        if (item.type == ItemType.Staff)
        {
            StaffInfoSet(item);
        }
        else if (item.type == ItemType.Book)
        {
            BookInfoSet(item);
        }
        else if(item.type == ItemType.Posion)
        {
            PosionInfoSet(item);
        }
        else if (item.type == ItemType.Esscence)
        {
            EssenceInfoSet(item);
        }
    }
}
