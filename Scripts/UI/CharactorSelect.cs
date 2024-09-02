using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorSelect : MonoBehaviour
{
    [Header("# Select UI")]
    public GameObject gameStartBtn;
    public Button[] charactor;
    public int charactorNum;
    public string[] charactorName;
    public string[] charactorNameColor;
    public Text[] charactorText;
    public Text noticeText;

    [Header("# Charactor Apply")]
    public RuntimeAnimatorController[] charactorAnimCon; // ĳ���� ���ϸ��̼� ��Ʈ�ѷ�
    public Animator playerAnim; // ĳ���� ���ϸ��̼� ��Ʈ�ѷ�
    public Sprite[] charactorSprites;
    public Image playerImage; // �κ��丮 �̹���
    public Text playerName;

    public void Charactor(int charactorNum) // ĳ���� ����
    {
        AudioManager.instance.SelectSfx();

        this.charactorNum = charactorNum;

        noticeText.text = string.Format("<color={0}>{1}</color>(��)�� ������\n�����Ͻðڽ��ϱ� ?", charactorNameColor[charactorNum], charactorName[charactorNum]);

        gameStartBtn.SetActive(true);
    }
    private void OnEnable()
    {
        gameStartBtn.SetActive(false);
        charactorNum = 0;

        /*
        for (int i = 0; i< charactor.Length; i++)
        {
            if (charactor[i].interactable)
            {
                charactorText[i].text = string.Format("<color={0}>{1}</color>", charactorNameColor[i], charactorName[i]);
            }
        }
        */
    }

    public void GameStart() // ���� ����
    {
        CharactorApply();

        ItemApply();

        GameManager.instance.GameStart();

        gameObject.SetActive(false);
    }

    public void CharactorApply()
    {
        // �ִϸ��̼� ����
        playerAnim.runtimeAnimatorController = charactorAnimCon[charactorNum];
        playerImage.sprite = charactorSprites[charactorNum];
        playerName.text = string.Format("<color={0}>{1}</color>", charactorNameColor[charactorNum], charactorName[charactorNum]);
    }
    public void ItemApply()
    {
        // ���� �ʱ�ȭ
        GameManager.instance.statManager.GameStart();

        // ĳ���� �� �ʱ� ������, ���� å
        int charactorItem = -(2 * charactorNum);
        GameManager.instance.rewardManager.ItemCreate(charactorItem - 1, 0);
        GameManager.instance.rewardManager.ItemCreate(charactorItem - 2, 1);
    }
}
