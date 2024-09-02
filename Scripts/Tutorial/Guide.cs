using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide : MonoBehaviour
{
    public CharactorSelect charactorSelect;
    public GameObject gameStartWindow;
    [Header("# Guide")]
    private int pageNum = 0;
    public string[] guideTitles;
    public Text titleText;
    public GameObject[] guideAreas;

    [Header("# Button")]
    public Button leftBtn;
    public Button rightBtn;
    public Text pageText;


    private void PageShift(int dir)
    {
        AudioManager.instance.SelectSfx();
        pageNum += -dir;
        titleText.text = guideTitles[pageNum];
        guideAreas[pageNum].SetActive(true);
        guideAreas[pageNum + dir].SetActive(false);
        pageText.text = string.Format("{0}/{1}", pageNum + 1, guideTitles.Length);
    }

    public void PageLeftShift()
    {
        PageShift(1);

        if (pageNum == 0)
        {
            leftBtn.interactable = false;
        }
        else if (!rightBtn.interactable)
        {
            rightBtn.interactable = true;
        }
    }

    public void PageRightShift()
    {
        PageShift(-1);

        if (pageNum == guideTitles.Length - 1)
        {
            rightBtn.interactable = false;
        }
        else if (!leftBtn.interactable)
        {
            leftBtn.interactable = true;
        }
    }

    public void GuideOff()
    {
        AudioManager.instance.SelectSfx();
        if (GameManager.instance.level == 0)
        {
            gameStartWindow.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
   }
    public void GameStart()
    {
        PageInit();
        gameStartWindow.SetActive(false);
        AudioManager.instance.SelectSfx();
        charactorSelect.GameStart();
        gameObject.SetActive(false);

    }

    public void PageInit()
    {
        rightBtn.interactable = true;
        leftBtn.interactable = false;
        guideAreas[pageNum].SetActive(false);
        pageNum = 0;
        titleText.text = guideTitles[pageNum];
        guideAreas[pageNum].SetActive(true);
        pageText.text = string.Format("{0}/{1}", pageNum + 1, guideTitles.Length);
    }
    public void GuideOn()
    {
        gameObject.SetActive(true);
        AudioManager.instance.SelectSfx();
    }
}
