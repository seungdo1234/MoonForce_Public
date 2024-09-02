using UnityEngine;


[System.Serializable]
public class Magic
{
    public bool isMagicActive; // Ȱ��ȭ ����

    [Header("# BaseInfo")]
    public int magicNumber; // ���� ��ȣ
    public string magicName; // ���� �̸�
    public GameObject magicEffect; // ���� ����Ʈ 

    [Header("# Detail Bool Value")]
    public bool magicCountIncrease; // �ѹ��� ��µǴ� ������ ������ ���� �������� 
    public bool isFlying; // ���ư����� ex)  ȭ����
    public bool isNonTarget; // Ÿ���� �ʿ����� ������ ?

    [Header("# DetailValue")]
    public float damagePer; // ������ ex) 1.5��� ���� ���ݷ��� 1.5���� �������� ��
    public int magicCount; // �ѹ��� �߻� �Ǵ� ������ ����
    public float magicCoolTime; // ������ ��Ÿ��
    public int penetration; // �����
    public float knockBackPer; // Enemy�� ������ �¾��� �� �˹� ��ġ



    [Header("# Skill Increase Value")]
    public float damageIncreaseValue;
    public float coolTimeDecreaseValue;
    public int magicRateStep;
    public int magicSizeStep;


}
