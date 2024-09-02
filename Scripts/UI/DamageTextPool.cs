using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TextType { Damage, Heal}
public class DamageTextPool : MonoBehaviour
{
    // �ʿ��� �� (������ ����Ʈ�� ������ ������ �����ؾ���.)
    // �����յ��� ������ ���� 
    public GameObject[] textPrefabs;

    // Ǯ ����� �ϴ� ����Ʈ��
    private List<GameObject>[] pools;

    private void Awake()
    {
        // textPrefabs�� ���� ��ŭ ����Ʈ ũ�� �ʱ�ȭ
        // Pool�� ��� �迭 �ʱ�ȭ
        pools = new List<GameObject>[textPrefabs.Length];

        // �迭 �ȿ� ����ִ� ������ ����Ʈ�鵵 �ʱ�ȭ
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }


    // ���� �Լ�
    public void Get(int textType,int damage, Vector3 target)
    {
        GameObject select = null;

        // ������ Ǯ�� ���(��Ȱ��ȭ ��) �ִ� ���ӿ�����Ʈ ���� -> �߰��ϸ� select ������ �Ҵ�
        // �̹� ������ Enemy�� �׾��� �� Destroy���� �ʰ� ��Ȱ��
        foreach (GameObject item in pools[textType])
        {
            // ���빰 ������Ʈ�� ��Ȱ��ȭ(��� ����)���� Ȯ��
            if (!item.activeSelf)
            {
                // ��� �ִ� ���ӿ�����Ʈ select ������ �Ҵ�
                select = item;
                break;
            }
        }
      
        // ���� ��ã�Ҵٸ� -> ���Ӱ� �����ϰ� select ������ �Ҵ�
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
