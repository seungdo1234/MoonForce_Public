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
        
        if(rewardType == 0) // ������
        {
            value = ChestManager.instance.Percent(ChestManager.instance.chestPer[GameManager.instance.spawner.spawnPerLevelUp].percent);
        }
        else // ���� å
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

            // rewardType: 0,2�� ������, 1,3�� ����å
            GameManager.instance.rewardManager.ItemCreate(value, rewardType);
        }
        else
        {
            GameManager.instance.rewardManager.ItemCreate(chestType, 1);
        }

    }

    // ���Ƿ� ����
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

    // ���ڸ� ���� Next ��ư ����
    private IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        chestOpenBtn.gameObject.SetActive(false);

        nextBtn.gameObject.SetActive(true);
    }

    public void ShopItemCreate(int[] percent, int rewardType) // ���� ������ ���� �Լ�
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
