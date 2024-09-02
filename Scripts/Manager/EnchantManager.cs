using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantManager : MonoBehaviour
{
    // 다른 스크립트에서도 쉽게 참조하기 위해 GameManager 인스턴스 화
    public static EnchantManager instance;
    public int maxEnchantStep; // 최대 강화 수치

    public int[] bookenchantPercents; // 마법 책의 품질의 차이에 따른 강화 확률 배열
    public int[] staffEnchantPercents; // 스테프의 등급의 차이에 따른 강화 확률 배열
    public int[] staffAttackIncrease; // 각 랭크별 강화 시 공격력 증가량

    private void Awake()
    {
        instance = this;
    }
}
