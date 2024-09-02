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
    public Essence[] essences; // ����

    [Header("# HealingPosion")]
    public HealingPosion[] healingPosions; // ü�� ����
    public int[] healingPosionPercent;

}
[System.Serializable]
public class Essence
{
    public Sprite essenceSprite; // ���� �������� �������� �ش� �ɷ�ġ�� �÷��ִ� ����
    public int essencePrice; // ����
    public string essenceName; // �̸�
    public float essenceIncreaseStat; // �̸�
}

[System.Serializable]
public class HealingPosion
{
    public Sprite healingPosionSprite; // ü�� ����
    public string healingPosionName; // ü�� ����
    public int healingPosionPrice;
    public int healingPosionRecoveryHealth;
}