using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedMoonEffect : MonoBehaviour
{
    public float lerpTime;
    public float alphaValue;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void RedMoonStart()
    {
        StartCoroutine(RedMoonAlpha(0 , alphaValue));
    }
    public void RedMoonEnd()
    {
        StartCoroutine(RedMoonAlpha(alphaValue , 0));
    }

   public void Lobby()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }
    private IEnumerator RedMoonAlpha(float start, float end)
    {
        float currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, currentTime / lerpTime);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }

        if(end == alphaValue)
        {
            GameManager.instance.pool.redMoon();
        }
    }
}
