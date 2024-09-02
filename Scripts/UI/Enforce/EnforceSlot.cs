using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnforceSlot : MonoBehaviour
{
    public Enforce enforce;
    private Image[] images;
    public Image[] stepImages;

    public void StepImageChange(int level) // 강화했을때 단계를 알려주는 이미지 넣음
    {
        if(enforce == null) // 초기화가 안됐다면 초기화
        {
            enforce = GetComponentInParent<Enforce>(true);
            images = GetComponentsInChildren<Image>();

            stepImages = new Image[images.Length - 2];

            for (int i = 2; i < images.Length; i++)
            {
                stepImages[i - 2] = images[i];
            }
        }

        for(int i =0; i < level; i++)
        {
            stepImages[i].sprite = enforce.enforceSuccessImage;
        }
    }
}
