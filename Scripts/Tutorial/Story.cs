using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoreanTyper;
using UnityEngine.EventSystems;

// 튜토리얼에서 스토리 텍스트 스크립트
public class Story : MonoBehaviour , IPointerUpHandler , IPointerDownHandler
{
    [Header("# TutorialManager")]
    public Tutorial tutorial;

    [Header("# Story Text")]
    private string originText;
    public Text text;
    public GameObject touchText;
    public float baseTypingSpeed; // 기본 타이핑 속도
    public float fastTypingSpeed; // 터치했을때 타이핑 속도
    private float typingSpeed;
    private char cursor_char = '|';
    [HideInInspector]
    public bool skipBtnActive;
    private bool storyTextEnd;


    private void OnEnable()
    {
        if(originText == null)
        {
            originText = text.text;
        }

        typingSpeed = baseTypingSpeed;
        storyTextEnd = false;
        touchText.SetActive(false);

        text.text = "";

        StartCoroutine(TypingRoutine());
    }

    private IEnumerator TypingRoutine()
    {
        int typingLength = originText.GetTypingLength();
        int point = 0;

        while (true)
        {
            if (!skipBtnActive) // 스킵 창이 활성화 돼 있다면 텍스트 작성 X
            {
                if(point < typingLength)
                {
                    if (point % 2 == 0)
                    {
                        AudioManager.instance.PlayerSfx(Sfx.Text);
                    }
                    text.text = originText.Typing(point);
                    text.text += cursor_char;
                    point++;
                    yield return new WaitForSeconds(typingSpeed);
                    if (point == typingLength) // 터치 활성화
                    {
                        text.text = originText;
                        yield return new WaitForSeconds(0.5f);
                        text.text = originText + cursor_char;
                        storyTextEnd = true;
                        touchText.SetActive(true);
                    }
                }
                else // 커서 깜빡임
                {
                    text.text = originText;
                    yield return new WaitForSeconds(0.25f);
                    text.text = originText + cursor_char;
                    yield return new WaitForSeconds(0.25f);
                }
            }
            else
            {
                yield return null;
            }

        }

    }

    // 터치가 끝날 때 이벤트
    public void OnPointerUp(PointerEventData eventData)
    {
        if (typingSpeed == fastTypingSpeed && eventData.button == PointerEventData.InputButton.Left)
        {
            typingSpeed = baseTypingSpeed;
        }
    }
    // 터치를 시작했을 때 이벤트
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (storyTextEnd)
            {
                tutorial.GuideOn();
            }
            else
            {
                typingSpeed = fastTypingSpeed;
            }
        }
    }
}
