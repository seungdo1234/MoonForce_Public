using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Sell : MonoBehaviour
{
    public ItemSlot[] waitSpaces;


    public void OnEnable()
    {
        ItemLoad();
    }
    public void ItemLoad() // �������� ��� ���Կ� ����
    {
        WaitSpaceReset();

        // �������� ��� ���Կ� �ֱ�
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            waitSpaces[i].item = ItemDatabase.instance.Set(i);

        }
        StartCoroutine(LoadImages());
    }
    private IEnumerator LoadImages() // ������ �ε�
    {
        // �̹��� �ε��� ���� �����ӱ��� ����
        yield return null;
        WaitSpaceImageLoading();
    }
    private void WaitSpaceImageLoading() // ��� ���� �̹��� �ε�
    {
        for (int i = 0; i < waitSpaces.Length; i++)
        {
            waitSpaces[i].ImageLoading();
            waitSpaces[i].ItemPriceLoad();

        }
    }
    private void WaitSpaceReset() // ��� ���� �̹��� �ε�
    {
        for (int i = 0; i < waitSpaces.Length; i++)
        {
            waitSpaces[i].item = null;
        }
    }


}
