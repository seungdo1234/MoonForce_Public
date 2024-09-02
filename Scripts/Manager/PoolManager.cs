using System.Collections.Generic;
using UnityEngine;

public enum PoolList { Enemy,  PlayerBullet, EnemyBullet , Heart}
public class PoolManager : MonoBehaviour
{
    // �ʿ��� �� (������ ����Ʈ�� ������ ������ �����ؾ���.)
    // �����յ��� ������ ���� 
    public GameObject[] prefabs;

    // �Ӽ��� ���� ����ź�� ��������Ʈ
    public Sprite[] bulletAttributes;
    private ItemAttribute attribute;
    // Ǯ ����� �ϴ� ����Ʈ��
    public List<GameObject>[] pools;


    [Header("RedMoon")]
    public float increaseValue;

    private void Awake()
    {
        // EnemyPrefabs�� ���� ��ŭ ����Ʈ ũ�� �ʱ�ȭ
        // Pool�� ��� �迭 �ʱ�ȭ
        pools = new List<GameObject>[prefabs.Length];

        // �迭 �ȿ� ����ִ� ������ ����Ʈ�鵵 �ʱ�ȭ
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
    private void Update()
    {

        // ���� ����ź�� ������ ���� ���� �Ӽ��� �ٸ� �� ��������Ʈ ���� 
        if (GameManager.instance.attribute != ItemAttribute.Default && attribute != GameManager.instance.attribute)
        {
            attribute = GameManager.instance.attribute;
            prefabs[1].gameObject.GetComponent<SpriteRenderer>().sprite = bulletAttributes[(int)attribute - 1];

        }
    }
    // Ǯ�� �Լ�
    public GameObject Get(int index)
    {
        GameObject select = null;

        // ������ Ǯ�� ���(��Ȱ��ȭ ��) �ִ� ���ӿ�����Ʈ ���� -> �߰��ϸ� select ������ �Ҵ�
        // �̹� ������ Enemy�� �׾��� �� Destroy���� �ʰ� ��Ȱ��
        foreach (GameObject item in pools[index])
        {
            // ���빰 ������Ʈ�� ��Ȱ��ȭ(��� ����)���� Ȯ��
            if (!item.activeSelf)
            {
                // ��� �ִ� ���ӿ�����Ʈ select ������ �Ҵ�
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
        // ���� ��ã�Ҵٸ� -> ���Ӱ� �����ϰ� select ������ �Ҵ�
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }

    public void PoolingReset()
    {
        // ���������� Ŭ�����߱⶧���� ��� Ǯ�� ������Ʈ�� ��Ȱ��ȭ 

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
