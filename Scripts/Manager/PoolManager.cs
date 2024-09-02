using System.Collections.Generic;
using UnityEngine;

public enum PoolList { Enemy,  PlayerBullet, EnemyBullet , Heart}
public class PoolManager : MonoBehaviour
{
    // 필요한 것 (변수나 리스트의 갯수는 무조건 동일해야함.)
    // 프리팹들을 보관할 변수 
    public GameObject[] prefabs;

    // 속성에 따른 마력탄의 스프라이트
    public Sprite[] bulletAttributes;
    private ItemAttribute attribute;
    // 풀 담당을 하는 리스트들
    public List<GameObject>[] pools;


    [Header("RedMoon")]
    public float increaseValue;

    private void Awake()
    {
        // EnemyPrefabs의 길이 만큼 리스트 크기 초기화
        // Pool를 담는 배열 초기화
        pools = new List<GameObject>[prefabs.Length];

        // 배열 안에 들어있는 각각의 리스트들도 초기화
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
    private void Update()
    {

        // 현재 마력탄의 종류가 장착 무기 속성과 다를 때 스프라이트 변경 
        if (GameManager.instance.attribute != ItemAttribute.Default && attribute != GameManager.instance.attribute)
        {
            attribute = GameManager.instance.attribute;
            prefabs[1].gameObject.GetComponent<SpriteRenderer>().sprite = bulletAttributes[(int)attribute - 1];

        }
    }
    // 풀링 함수
    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀의 놀고(비활성화 된) 있는 게임오브젝트 접근 -> 발견하면 select 변수에 할당
        // 이미 생성한 Enemy가 죽었을 때 Destroy하지 않고 재활용
        foreach (GameObject item in pools[index])
        {
            // 내용물 오브젝트가 비활성화(대기 상태)인지 확인
            if (!item.activeSelf)
            {
                // 놀고 있는 게임오브젝트 select 변수에 할당
                select = item;
                select.SetActive(true);
                if (index == 1)
                {
                    SpriteRenderer sprite = select.GetComponent<SpriteRenderer>();
                    if (sprite.sprite != bulletAttributes[(int)attribute - 1])
                    {
                        sprite.sprite = bulletAttributes[(int)attribute - 1];
                    }
                }
                break;
            }
        }
        // 만약 못찾았다면 -> 새롭게 생성하고 select 변수에 할당
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }

    public void PoolingReset()
    {
        // 스테이지를 클리어했기때문에 모든 풀링 오브젝트들 비활성화 

        for (int i = 0; i < pools.Length ; i++){
            foreach (GameObject item in pools[i])
            {
                if (item.activeSelf)
                {
                    if(i == pools.Length- 1)
                    {
                        item.GetComponent<Heart>().StageClear();
                    }
                    else
                    {
                        item.SetActive(false);
                    }
                }
            }
        }
       

    }

    public void redMoon()
    {
        foreach (GameObject enemy in pools[0])
        {
            if (enemy.activeSelf)
            {
                Enemy enemyStat = enemy.GetComponent<Enemy>();
                enemyStat.damage += enemyStat.damage / 2;
                enemyStat.speed += enemyStat.speed * increaseValue;
                enemyStat.health += enemyStat.health * increaseValue;

            }
        }
    }
}
