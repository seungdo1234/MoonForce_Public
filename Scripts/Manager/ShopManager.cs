using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    [Header("# Staff")]
    public int[] staffRankPrices;
    public int[] staffRankSalePrices;

    [Header("# Skill Book")]
    public int[] bookQualityPrices;
    public int[] bookQualitySalePrices;

    [Header("# Essence")]
    public Essence[] essences; // 정수

    [Header("# HealingPosion")]
    public HealingPosion[] healingPosions; // 체력 포션
    public int[] healingPosionPercent;

}
[System.Serializable]
public class Essence
{
    public Sprite essenceSprite; // 다음 스테이지 한정으로 해당 능력치를 올려주는 정수
    public int essencePrice; // 가격
    public string essenceName; // 이름
    public float essenceIncreaseStat; // 이름
}

[System.Serializable]
public class HealingPosion
{
    public Sprite healingPosionSprite; // 체력 포션
    public string healingPosionName; // 체력 포션
    public int healingPosionPrice;
    public int healingPosionRecoveryHealth;
}