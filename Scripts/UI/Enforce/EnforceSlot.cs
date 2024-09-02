using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnforceSlot : MonoBehaviour
{
    public Enforce enforce;
    private Image[] images;
    public Image[] stepImages;

    public void StepImageChange(int level) // ��ȭ������ �ܰ踦 �˷��ִ� �̹��� ����
    {
        if(enforce == null) // �ʱ�ȭ�� �ȵƴٸ� �ʱ�ȭ
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
