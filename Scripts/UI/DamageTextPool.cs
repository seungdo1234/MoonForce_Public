using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TextType { Damage, Heal}
public class DamageTextPool : MonoBehaviour
{
    // 필요한 것 (변수나 리스트의 갯수는 무조건 동일해야함.)
    // 프리팹들을 보관할 변수 
    public GameObject[] textPrefabs;

    // 풀 담당을 하는 리스트들
    private List<GameObject>[] pools;

    private void Awake()
    {
        // textPrefabs의 길이 만큼 리스트 크기 초기화
        // Pool를 담는 배열 초기화
        pools = new List<GameObject>[textPrefabs.Length];

        // 배열 안에 들어있는 각각의 리스트들도 초기화
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }


    // 폴링 함수
    public void Get(int textType,int damage, Vector3 target)
    {
        GameObject select = null;

        // 선택한 풀의 놀고(비활성화 된) 있는 게임오브젝트 접근 -> 발견하면 select 변수에 할당
        // 이미 생성한 Enemy가 죽었을 때 Destroy하지 않고 재활용
        foreach (GameObject item in pools[textType])
        {
            // 내용물 오브젝트가 비활성화(대기 상태)인지 확인
            if (!item.activeSelf)
            {
                // 놀고 있는 게임오브젝트 select 변수에 할당
                select = item;
                break;
            }
        }
      
        // 만약 못찾았다면 -> 새롭게 생성하고 select 변수에 할당
        if (!select)
        {
            select = Instantiate(textPrefabs[textType], transform);
            pools[textType].Add(select);
        }

        select.GetComponent<DamageText>().Init(damage, target);

    }

    private void Update()
    {
        //Get(55, transform);
    }
}
