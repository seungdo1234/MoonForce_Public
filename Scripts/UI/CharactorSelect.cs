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
    public RuntimeAnimatorController[] charactorAnimCon; // 캐릭터 에니메이션 컨트롤러
    public Animator playerAnim; // 캐릭터 에니메이션 컨트롤러
    public Sprite[] charactorSprites;
    public Image playerImage; // 인벤토리 이미지
    public Text playerName;

    public void Charactor(int charactorNum) // 캐릭터 선택
    {
        AudioManager.instance.SelectSfx();

        this.charactorNum = charactorNum;

        noticeText.text = string.Format("<color={0}>{1}</color>(으)로 게임을\n시작하시겠습니까 ?", charactorNameColor[charactorNum], charactorName[charactorNum]);

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

    public void GameStart() // 게임 시작
    {
        CharactorApply();

        ItemApply();

        GameManager.instance.GameStart();

        gameObject.SetActive(false);
    }

    public void CharactorApply()
    {
        // 애니메이션 적용
        playerAnim.runtimeAnimatorController = charactorAnimCon[charactorNum];
        playerImage.sprite = charactorSprites[charactorNum];
        playerName.text = string.Format("<color={0}>{1}</color>", charactorNameColor[charactorNum], charactorName[charactorNum]);
    }
    public void ItemApply()
    {
        // 스탯 초기화
        GameManager.instance.statManager.GameStart();

        // 캐릭터 별 초기 스태프, 마법 책
        int charactorItem = -(2 * charactorNum);
        GameManager.instance.rewardManager.ItemCreate(charactorItem - 1, 0);
        GameManager.instance.rewardManager.ItemCreate(charactorItem - 2, 1);
    }
}
