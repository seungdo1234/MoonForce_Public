using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantManager : MonoBehaviour
{
    // �ٸ� ��ũ��Ʈ������ ���� �����ϱ� ���� GameManager �ν��Ͻ� ȭ
    public static EnchantManager instance;
    public int maxEnchantStep; // �ִ� ��ȭ ��ġ

    public int[] bookenchantPercents; // ���� å�� ǰ���� ���̿� ���� ��ȭ Ȯ�� �迭
    public int[] staffEnchantPercents; // �������� ����� ���̿� ���� ��ȭ Ȯ�� �迭
    public int[] staffAttackIncrease; // �� ��ũ�� ��ȭ �� ���ݷ� ������

    private void Awake()
    {
        instance = this;
    }
}
