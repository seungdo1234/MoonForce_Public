using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageClear : MonoBehaviour
{
    public RuntimeAnimatorController[] animCon;
    public Image reward;
    public ItemSlot rewardSlot;

    private int rewardType;
    private int chestType;
    private Animator chestAnim;
    private Image chestImage;
    public GameObject rewardTypeSelecet;
    public Button chestOpenBtn;
    public Button nextBtn;

    private void Awake()
    {
        chestAnim = GetComponentInChildren<Animator>(true);
        chestImage = GetComponentInChildren<Image>(true);
        rewardSlot = reward.GetComponent<ItemSlot>();
    }

    public void RewardSelect(int rewardType)
    {
        int value = 0;
        AudioManager.instance.SelectSfx();
        this.rewardType = rewardType;
        rewardTypeSelecet.SetActive(false);
        chestOpenBtn.gameObject.SetActive(true);


        chestAnim.gameObject.SetActive(true);
        
        if(rewardType == 0) // 스태프
        {
            value = ChestManager.instance.Percent(ChestManager.instance.chestPer[GameManager.instance.spawner.spawnPerLevelUp].percent);
        }
        else // 마법 책
        {
            value = ChestManager.instance.Percent(ChestManager.instance.qualityPer[GameManager.instance.spawner.spawnPerLevelUp].percent);
        }

        chestType = value;
        chestAnim.runtimeAnimatorController = animCon[value];
    }

    public void ChestOpen()
    {
        chestAnim.SetTrigger("Open");

        AudioManager.instance.PlayerSfx(Sfx.ChestOpen);

        reward.gameObject.SetActive(true);

        GetReward();

        chestOpenBtn.interactable = false;


        StartCoroutine(Delay(1f));
    }
    private void GetReward()
    {

        if(rewardType == 0)
        {
            int value = 0;

            if (chestType == 0)
            {
                value = ChestManager.instance.Percent(ChestManager.instance.bronzeChest);
            }
            else if (chestType == 1)
            { 
                value = ChestManager.instance.Percent(ChestManager.instance.silverChest);
            }
            else if (chestType == 2)
            {
                value = ChestManager.instance.Percent(ChestManager.instance.goldChest);
            }
            else
            {
                value = ChestManager.instance.Percent(ChestManager.instance.specialChest);
            }

            // rewardType: 0,2는 스태프, 1,3은 마법책
            GameManager.instance.rewardManager.ItemCreate(value, rewardType);
        }
        else
        {
            GameManager.instance.rewardManager.ItemCreate(chestType, 1);
        }

    }

    // 대기실로 가기
    public void NextStage()
    {
        gameObject.SetActive(false);
        chestImage.color = new Color(1, 1, 1, 1);
        chestAnim.gameObject.SetActive(false);
        reward.color = new Color(1, 1, 1, 0);
        rewardTypeSelecet.SetActive(true);
        chestOpenBtn.interactable = true;
        nextBtn.gameObject.SetActive(false);
        reward.gameObject.SetActive(false);
        rewardSlot.SkillSpriteReset();

    }

    // 상자를 열고 Next 버튼 띄우기
    private IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        chestOpenBtn.gameObject.SetActive(false);

        nextBtn.gameObject.SetActive(true);
    }

    public void ShopItemCreate(int[] percent, int rewardType) // 상점 아이템 생성 함수
    {
        int value = ChestManager.instance.Percent(percent);

        if(rewardType >= 0)
        {
            GameManager.instance.rewardManager.ItemCreate(value, rewardType);
        }
        else
        {
            GameManager.instance.shop.PosionCreate(value);
        }
    }
   
}
