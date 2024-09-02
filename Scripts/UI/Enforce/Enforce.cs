using UnityEngine;
using UnityEngine.UI;


public enum EnforceName { BulletDamageUp, RateUp, SpeedUp, DeffenseUp, MagicDamageUp, MagicCoolTimeDown, HealthRecoveryUp, GoldUp, PenetrationUp }
public class Enforce : MonoBehaviour
{
    [Header("# EnforceSlot")]
    public EnforceInfo[] enforceInfo;
    public EnforceSlot[] enforceSlot;
    public Sprite enforceSuccessImage;

    [Header("# GemStone")]
    public Text gemStoneText;

    [Header("# EnforceInfoWindow")]
    public Text enforceNameText;
    public Text enforceDescText;
    public Text enforcePriceText;
    public GameObject gemStone;
    public Button buyBtn;
    public int selectNum = -1;

    private void OnEnable()
    {
        EnforceInit();
        GemStoneTextSet();
        selectNum = -1;
    }

    private void EnforceInit() // �ʱ�ȭ
    {
        enforceNameText.text = null;
        enforceDescText.text = null;
        enforcePriceText.text = null;
        buyBtn.gameObject.SetActive(false);
        buyBtn.interactable = false;
        gemStone.SetActive(false);
    }
    public void EnforceLoad() // ����� ��ȭ ���� �ҷ�����
    {
        for (int i = 0; i < enforceInfo.Length; i++)
        {
            if (PlayerPrefs.HasKey(enforceInfo[i].name))
            {
                int level = PlayerPrefs.GetInt(enforceInfo[i].name);
                enforceInfo[i].curLevel = level;
                enforceSlot[i].StepImageChange(level);
            }

        }

    }
    public void EnforceSelect(int num) // ��ȭ ���� ����
    {
        AudioManager.instance.SelectSfx();

        selectNum = num;
        
        DescLoad();
    }
    
    public void DescLoad() // ��ȭ ���� �ε�
    {
        if (enforceInfo[selectNum].curLevel < enforceInfo[selectNum].maxLevel) // MAX ������ �ƴ϶��
        {
            int price = enforceInfo[selectNum].initPrice + (enforceInfo[selectNum].priceIncrease * enforceInfo[selectNum].curLevel);

            enforceNameText.text = string.Format("{0} Lv.{1}", enforceInfo[selectNum].name, enforceInfo[selectNum].curLevel + 1);
            enforcePriceText.text = string.Format("{0}", price);
            buyBtn.interactable = PlayerPrefs.GetInt("GemStone") < price ? false : true;
            gemStone.SetActive(true);
            buyBtn.gameObject.SetActive(true);
        }
        else // MAX���� �̶��
        {
            enforceNameText.text = string.Format("{0} Lv.Max", enforceInfo[selectNum].name);
            buyBtn.interactable = false;
            gemStone.SetActive(false);
        }
        enforceDescText.text = enforceInfo[selectNum].enforceDesc;
    }
    public void Buy() // ��ȭ�ϱ�
    {
        int price = enforceInfo[selectNum].initPrice + (enforceInfo[selectNum].initPrice * enforceInfo[selectNum].curLevel);
        PlayerPrefs.SetInt("GemStone", PlayerPrefs.GetInt("GemStone") - price);
       
        enforceInfo[selectNum].curLevel++;
        PlayerPrefs.SetInt(enforceInfo[selectNum].name, enforceInfo[selectNum].curLevel);

        AudioManager.instance.PlayerSfx(Sfx.BuySell);
        GemStoneTextSet();
        enforceSlot[selectNum].StepImageChange(enforceInfo[selectNum].curLevel);
        DescLoad();
    }


    public void GemStoneTextSet() // ������ ���� �ҷ�����
    {
        gemStoneText.text = string.Format("{0}", PlayerPrefs.GetInt("GemStone"));
    }

    public void ExtraGold() // �߰� ���
    {
        int getGold = 0;
        for(int i = 0; i<GameManager.instance.spawner.enemySpawnNum.Length; i++)
        {
            getGold += GameManager.instance.spawner.enemySpawnNum[i] * (i + 1);
        }

        float percent = enforceInfo[(int)EnforceName.GoldUp].statIncrease * enforceInfo[(int)EnforceName.GoldUp].curLevel;

        int gold = Mathf.FloorToInt(getGold * percent);

        GameManager.instance.gold += gold;
    }


    private void Update()
    {
    }

}

[System.Serializable]
public class EnforceInfo
{
    public string name;
    public string enforceDesc;
    public int maxLevel;
    public int curLevel;
    public int initPrice;
    public int priceIncrease;
    public float statIncrease;


}
