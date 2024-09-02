using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTimeSlot : MonoBehaviour
{


    private Image magicImage;
    private Image hideImage;
    private float coolTime;


    private void ImageInit()
    {
        Image[] images = GetComponentsInChildren<Image>();

        magicImage = images[1];
        hideImage = images[2];
    }
    public void Init(Sprite skillSprite , float coolTime)
    {
        if(magicImage == null)
        {
            ImageInit();
        }

        magicImage.sprite = skillSprite;
        magicImage.SetNativeSize();
        hideImage.fillAmount = 0;

        this.coolTime = coolTime;

    }

    public void CoolTime()
    {
        StartCoroutine(CoolTimeStart());
    }
    private IEnumerator CoolTimeStart()
    {
        float timer = 0;

        while (timer < coolTime)
        {
            timer += Time.deltaTime;

            float amount = Mathf.Lerp(1, 0, timer / coolTime);

            hideImage.fillAmount = amount;

            yield return null;
        }
    }
}
