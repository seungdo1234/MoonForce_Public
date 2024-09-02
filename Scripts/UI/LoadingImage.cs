using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// ���θ޴��� ���� ȭ�� ��ȯ ��ũ��Ʈ
public class LoadingImage : MonoBehaviour
{
    private Image loading;
    public float lerpTime;
    private void Awake()
    {
        loading = GetComponent<Image>();    
    }

    public void Loading(int start, int end)
    {
        if(start == 0)
        {
            gameObject.SetActive(true);
        }
        StartCoroutine(LoadingStart(start ,end));
    }
    private IEnumerator LoadingStart(int start, int end)
    {
        float timer = 0;

        while (timer < lerpTime)
        {
            timer += Time.deltaTime;

            float amount = Mathf.Lerp(start, end, timer / lerpTime);

            loading.fillAmount = amount;

            yield return null;
        }


        if(end == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
