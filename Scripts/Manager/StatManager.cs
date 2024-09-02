using UnityEngine;

public class StatManager : MonoBehaviour
{
    [Header("# Basic Stat")]
    public float maxHealth;
    public float curHealth;
    public int attack;
    public int baseAttack;
    public float rate;
    public float baseRate;
    public float moveSpeed;
    public float baseMoveSpeed;
    public int penetration; // 관통력
    public int weaponNum; // 총 : 한번에 발사 되는 총탄, 근접 : 삽의 갯수
    public float knockBackValue; // Enemy가 마력탄을 맞았을 때 넉백 수치

    [Header("# Init Stat")]
    public float initHealth;
    public int initAttack;
    public float initRate;
    public float initMoveSpeed;


    [Header("# Essence")]
    public bool essenceOn;
    public int activeEssenceNum;
    public float[] essenceStat;

    [Header("# Stat Level")]
    public int[] statLevels;
    public int[] statMaxLevels;
    public float[] statUpValue;

    [Header("# Fire Attribute Stat")]
    public float burningDamagePer; // 화상 데미지
    public float baseBurningDamagePer; // 화상 데미지
    public float burningEffectTime; // 화상 시간

    [Header("# Water Attribute Stat")]
    public float wettingDamagePer; // 젖은 상태일 때 마법 추가 데미지량
    public float baseWettingDamagePer; // 젖은 상태일 때 마법 추가 데미지량
    public float wettingEffectTime; // 젖은 상태 시간

    [Header("# Non Attribute Stat")]
    public float bulletDamagePer; // 마력탄 데미지 증가량
    public float baseBulletDamagePer; // 마력탄 데미지 증가량

    [Header("# Grass Attribute Stat")]
    public float restraintTime; // 속박 시간
    public float baseRestraintTime; // 속박 시간

    [Header("# Earth Attribute Stat")]
    public float speedReducedEffectTime; // 이동속도 감소 시간
    public float speedReducePer; // 이동속도 감소량
    public float baseSpeedReducePer; // 이동속도 감소량

    [Header("# Dark Attribute Stat")]
    public float darkExplosionDamagePer; // 폭발 데미지 
    public float baseDarkExplosionDamagePer; // 폭발 데미지 
    public float darknessExpTime; // 몇 초뒤 폭발할 것 인지 

    [Header("# Holy Attribute Stat")]
    public float instantKillPer; // 즉사 확률



    public void GameStart()
    {
        penetration = 1 + GameManager.instance.enforce.enforceInfo[(int)EnforceName.PenetrationUp].curLevel;
        weaponNum = 1;
        maxHealth = initHealth;
        curHealth = maxHealth;
        attack = initAttack;
        baseAttack = initAttack;
        baseRate = initRate -( GameManager.instance.enforce.enforceInfo[(int)EnforceName.RateUp].statIncrease * GameManager.instance.enforce.enforceInfo[(int)EnforceName.RateUp].curLevel);
        rate = baseRate  ;
        baseMoveSpeed = initMoveSpeed + (GameManager.instance.enforce.enforceInfo[(int)EnforceName.SpeedUp].statIncrease * GameManager.instance.enforce.enforceInfo[(int)EnforceName.SpeedUp].curLevel); ;
        moveSpeed = baseMoveSpeed;
        burningDamagePer = baseBurningDamagePer;
        wettingDamagePer = baseWettingDamagePer;
        bulletDamagePer = baseBulletDamagePer;
        restraintTime = baseRestraintTime;
        speedReducePer = baseSpeedReducePer;
        darkExplosionDamagePer = baseDarkExplosionDamagePer;

        for (int i = 0; i < statLevels.Length; i++)
        {

            statLevels[i] = 0;
        }

    }


    public void EssenceOn(int essenceNum, float stat)
    {

        essenceOn = true;
        activeEssenceNum = essenceNum;

        switch (essenceNum)
        {
            case 0:
                if(stat < 1)
                {
                    essenceStat[0] = Mathf.Floor(attack * stat);
                }
                attack += (int)essenceStat[0];
                break;
            case 1:
                essenceStat[1] = stat;
                moveSpeed += essenceStat[1];
                break;
            case 2:
                essenceStat[2] = stat;
                rate -= essenceStat[2];
                break;
            case 3:
                essenceStat[3] = stat;
                penetration += (int)essenceStat[3];
                break;
        }


    }
    public void EssenceOff()
    {
        essenceOn = false;

        switch (activeEssenceNum)
        {
            case 0:
                attack -= (int)essenceStat[0];
                break;
            case 1:
                moveSpeed -= essenceStat[1];
                break;
            case 2:
                rate += essenceStat[2];
                break;
            case 3:
                penetration -= (int)essenceStat[3];
                break;
        }

        essenceStat[activeEssenceNum] = 0;
        activeEssenceNum = -1;
    }
    public void StatValueUp(int statNumber)
    {
        switch (statNumber)
        {
            case 0:
                attack -= baseAttack;
                baseAttack += (int)statUpValue[statNumber];
                attack += baseAttack;
                break;
            case 1:
                rate -= baseRate;
                baseRate -= statUpValue[statNumber];
                rate += baseRate;
                break;
            case 2:
                float value = moveSpeed - baseMoveSpeed;
                baseMoveSpeed += statUpValue[statNumber];
                moveSpeed = baseMoveSpeed + value;
                break;
            case 3:
                maxHealth += statUpValue[statNumber];
                curHealth += statUpValue[statNumber];
                break;
            case 4:
                penetration += (int)statUpValue[statNumber];
                break;
            case 5:
                burningDamagePer += statUpValue[statNumber];
                break;
            case 6:
                wettingDamagePer += statUpValue[statNumber];
                break;
            case 7:
                bulletDamagePer += statUpValue[statNumber];
                break;
            case 8:
                restraintTime += statUpValue[statNumber];
                break;
            case 9:
                speedReducePer += statUpValue[statNumber];
                break;
            case 10:
                darkExplosionDamagePer += statUpValue[statNumber];
                break;
        }
    }

    public void PlayerHealthRecovery()
    {
        float recoveryHealthPer = GameManager.instance.enforce.enforceInfo[(int)EnforceName.HealthRecoveryUp].statIncrease * GameManager.instance.enforce.enforceInfo[(int)EnforceName.HealthRecoveryUp].curLevel;
        float recoveryHealth = maxHealth * recoveryHealthPer;

        Debug.Log(recoveryHealthPer);
        Debug.Log(recoveryHealth);
        curHealth = Mathf.Min(maxHealth, curHealth + recoveryHealth);

        Vector3 pos = GameManager.instance.player.transform.position + new Vector3(0,0.75f,0);
        GameManager.instance.damageTextPool.Get((int)TextType.Heal,(int) recoveryHealth, pos);
        AudioManager.instance.PlayerSfx(Sfx.Heal);

    }
}
