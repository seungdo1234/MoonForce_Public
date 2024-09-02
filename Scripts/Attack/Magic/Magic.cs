using UnityEngine;


[System.Serializable]
public class Magic
{
    public bool isMagicActive; // 활성화 여부

    [Header("# BaseInfo")]
    public int magicNumber; // 마법 번호
    public string magicName; // 마법 이름
    public GameObject magicEffect; // 마법 이펙트 

    [Header("# Detail Bool Value")]
    public bool magicCountIncrease; // 한번에 출력되는 마법의 갯수가 증가 가능한지 
    public bool isFlying; // 날아가는지 ex)  화염구
    public bool isNonTarget; // 타겟이 필요하지 않은지 ?

    [Header("# DetailValue")]
    public float damagePer; // 데미지 ex) 1.5라면 현재 공격력의 1.5배의 데미지가 들어감
    public int magicCount; // 한번에 발사 되는 마법의 갯수
    public float magicCoolTime; // 마법의 쿨타임
    public int penetration; // 관통력
    public float knockBackPer; // Enemy가 마법을 맞았을 때 넉백 수치



    [Header("# Skill Increase Value")]
    public float damageIncreaseValue;
    public float coolTimeDecreaseValue;
    public int magicRateStep;
    public int magicSizeStep;


}
