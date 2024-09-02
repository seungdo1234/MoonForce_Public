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
    public void ItemLoad() // 아이템을 대기 슬롯에 넣음
    {
        WaitSpaceReset();

        // 아이템을 대기 슬롯에 넣기
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            waitSpaces[i].item = ItemDatabase.instance.Set(i);

        }
        StartCoroutine(LoadImages());
    }
    private IEnumerator LoadImages() // 아이템 로드
    {
        // 이미지 로딩을 다음 프레임까지 연기
        yield return null;
        WaitSpaceImageLoading();
    }
    private void WaitSpaceImageLoading() // 대기 슬롯 이미지 로딩
    {
        for (int i = 0; i < waitSpaces.Length; i++)
        {
            waitSpaces[i].ImageLoading();
            waitSpaces[i].ItemPriceLoad();

        }
    }
    private void WaitSpaceReset() // 대기 슬롯 이미지 로딩
    {
        for (int i = 0; i < waitSpaces.Length; i++)
        {
            waitSpaces[i].item = null;
        }
    }


}
