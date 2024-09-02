using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnchantCheckUI : MonoBehaviour
{
    public Enchant enchant;
    public ItemPreview[] itemPreview;
    public Text enchantPercentText;
    public Button enchantStartBtn;

    public GameObject touchBlock; // 강화하는 순간 다른 곳 터치를 막는 Block 이미지
    public GameObject enchantResultNotice; // 강화 시 나오는 알림 오브젝트
    private Text enchantResultNoticeText; // 텍스트

    private ItemSlot[] itemSlots;
    private int enchantPercent;

    [Header("Skill Book")]
    public AditionalSelectBtn[] aditionalSelectBtn;
    public int selectEnchantAditionalNum;
    public int selectMaterialAditionalNum;
    private void Awake()
    {
        enchantResultNoticeText = enchantResultNotice.GetComponentInChildren<Text>(true);
        itemSlots = GetComponentsInChildren<ItemSlot>();
    }
    private void SkillBookEnchantInit() // 마법 책 강화 초기화
    {
        if(selectEnchantAditionalNum != -1)
        {
            aditionalSelectBtn[0].InitButtonImage(selectEnchantAditionalNum);
            selectEnchantAditionalNum = -1;
        }
        if(selectMaterialAditionalNum != -1)
        {
            aditionalSelectBtn[1].InitButtonImage(selectMaterialAditionalNum);
            selectMaterialAditionalNum = -1;
        }

    }
    public void UIOn(Item enchant_Item , Item material_Item) // 강화 창 On
    {
        gameObject.SetActive(true);

        itemSlots[0].item = enchant_Item;
        itemSlots[1].item = material_Item;

        for(int i =0; i < itemSlots.Length; i++) // 이미지, 아이템 정보 로딩
        {
            itemSlots[i].ImageLoading();
            itemPreview[i].ItemInfoSet(itemSlots[i].item);
        }

        int enchantPerStep;
        if (enchant_Item.type == ItemType.Staff)
        {
            enchantStartBtn.interactable = true;
            enchantPerStep = Mathf.Max(0, (int)itemSlots[0].item.rank - (int)itemSlots[1].item.rank);
            enchantPercent = EnchantManager.instance.staffEnchantPercents[enchantPerStep];
        }
        else
        {
            enchantStartBtn.interactable = false;
            StartCoroutine(EnchantStartBtnOn());

            enchantPerStep = Mathf.Max(0, (int)itemSlots[0].item.quality - (int)itemSlots[1].item.quality);
            enchantPercent = EnchantManager.instance.bookenchantPercents[enchantPerStep];

            aditionalSelectBtn[0].ButtonOn(enchant_Item.aditionalAbility.Length);
            aditionalSelectBtn[1].ButtonOn(material_Item.aditionalAbility.Length);
        }

        enchantPercentText.text = string.Format("<color=black>강화 성공 확률:</color> <color=red>{0}%</color>", enchantPercent);
    }
   
    public void EnchantStart() // 강화 시작
    {
        enchantStartBtn.interactable = false;

        bool enchantResult;
        int random = Random.Range(1, 101);

        if(random <= enchantPercent) // 강화 성공
        {
            Item enchantItem = itemSlots[0].item;
            switch (enchantItem.type)
            {
                case ItemType.Staff:
                    enchantItem.attack += EnchantManager.instance.staffAttackIncrease[(int)enchantItem.rank - 1];
                    enchantItem.enchantStep++;
                    if (enchantItem.isEquip)
                    {
                        GameManager.instance.inventory.EquipStaff();
                    }
                    break;
                case ItemType.Book:
                    Item materialItem = itemSlots[1].item;

                    if (enchantItem.isEquip)
                    {
                        GameManager.instance.inventory.SkillBookInit();
                        enchantItem.aditionalAbility[selectEnchantAditionalNum] = materialItem.aditionalAbility[selectMaterialAditionalNum];
                        GameManager.instance.inventory.SkillBookActive();
                    }
                    else
                    {
                        enchantItem.aditionalAbility[selectEnchantAditionalNum] = materialItem.aditionalAbility[selectMaterialAditionalNum];
                    }
                    break;
            }

            enchantResult = true;
        }
        else // 강화 실패
        {
            enchantResult = false;
        }

        // 재료 아이템 삭제
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            Item item = ItemDatabase.instance.Set(i);
            if (item == itemSlots[1].item)
            {
                if (item.isEquip) // 장착중인 아이템이라면 
                {
                    if (item.type == ItemType.Book) // 마법책
                    {
                        GameManager.instance.inventory.SkillBookInit();
                        itemSlots[1].item.reset();
                        GameManager.instance.inventory.SkillBookActive();
                    }
                    else // 스태프
                    {
                        if (itemSlots[1].item.rank == ItemRank.Legendary)
                        {
                            if (itemSlots[1].item.skillNum != 0)
                            {
                                GameManager.instance.magicManager.magicInfo[itemSlots[i].item.skillNum].isMagicActive = false;
                            }
                            else
                            {
                                GameManager.instance.inventory.UranusOff();
                            }
                        }
                        itemSlots[1].item.reset();
                        GameManager.instance.inventory.EquipStaff();
                    }
                    ItemDatabase.instance.ItemRemove(i);
                    break;
                }
                itemSlots[1].item.reset();
                ItemDatabase.instance.ItemRemove(i);
                break;
            }
        }

        StartCoroutine(EnchantResultNoticeOn(enchantResult));
    }
    private IEnumerator EnchantResultNoticeOn(bool enchantResult) // 강화 결과 알림 창
    {
        touchBlock.SetActive(true);
        enchantResultNotice.SetActive(true);
        yield return new WaitForSeconds(1);

        if (enchantResult)
        {
            enchantResultNoticeText.text = "강화 성공 !";
            AudioManager.instance.PlayerSfx(Sfx.EnchantSuccess);
        }
        else
        {
            enchantResultNoticeText.text = "강화 실패 ㅠㅠ";
            AudioManager.instance.PlayerSfx(Sfx.EnchantFail);
        }
        enchantResultNoticeText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);
        enchantResultNoticeText.gameObject.SetActive(false);
        enchantResultNotice.SetActive(false);
        EnchantCancel();
        touchBlock.SetActive(false);
    }
    public void EnchantAditionalSelect(int num) // 마법책 강화일 때 강화할 마법책의 스킬 선택
    {
        if(selectEnchantAditionalNum != -1)
        {
            aditionalSelectBtn[0].InitButtonImage(selectEnchantAditionalNum);
        }
        selectEnchantAditionalNum = num;
        aditionalSelectBtn[0].SelectButtonImage(selectEnchantAditionalNum);

    }
    public void MaterialAditionalSelect(int num)// 마법책 강화일 때 재료 마법책의 스킬 선택
    { 
        if (selectMaterialAditionalNum != -1)
        {
            aditionalSelectBtn[1].InitButtonImage(selectMaterialAditionalNum);
        }
        selectMaterialAditionalNum = num;
        aditionalSelectBtn[1].SelectButtonImage(selectMaterialAditionalNum);

    }
    public void EnchantCancel() // 인첸트를 취소하거나 실행했을 때 원래 강화창이 나오게하는 함수
    {
        gameObject.SetActive(false);

        SkillBookEnchantInit();
        aditionalSelectBtn[0].gameObject.SetActive(false);
        aditionalSelectBtn[1].gameObject.SetActive(false);

        enchant.enchantNoticeText.gameObject.SetActive(true);
        enchant.EnchantItemoff();
    }
    private IEnumerator EnchantStartBtnOn() // 마법책 인첸트 시 스킬 두개를 선택해야만 강화할 수 있게 하는 코루틴 
    {
        while(enchantStartBtn.interactable != true)
        {
            if (selectMaterialAditionalNum != -1 && selectEnchantAditionalNum != -1)
            {
                enchantStartBtn.interactable = true;
            }
            yield return null;
        }
    }
}
