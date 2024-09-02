using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Ʃ�丮�� ���� ��ũ��Ʈ
public class Tutorial : MonoBehaviour
{
    public CharactorSelect charactorSelect;
    public Guide guide;
    public void StoryOn()
    {
        AudioManager.instance.EndBgm();
        charactorSelect.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }


    public void TutorialSkip()
    {
        gameObject.SetActive(false);
        charactorSelect.GameStart();
    }

    public void GuideOn()
    {
        gameObject.SetActive(false);
        guide.GuideOn();
    }
}
