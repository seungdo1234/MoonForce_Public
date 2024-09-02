using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ʃ�丮�� ��ŵ ��ư ��ũ��Ʈ
public class Skip : MonoBehaviour
{
    public Story story;
    public GameObject SkipUI;

    private void OnEnable()
    {
        SkipUI.SetActive(false);
        story.skipBtnActive = false;
    }

    public void SkipUIOn()
    {
        AudioManager.instance.SelectSfx();
        story.skipBtnActive = true;
        SkipUI.SetActive(true);
    }

    public void SkipUIOff()
    {
        AudioManager.instance.SelectSfx();
        SkipUI.SetActive(false);
        story.skipBtnActive = false;
    }

  

}
