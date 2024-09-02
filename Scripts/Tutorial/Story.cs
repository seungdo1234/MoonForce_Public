using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoreanTyper;
using UnityEngine.EventSystems;

// Ʃ�丮�󿡼� ���丮 �ؽ�Ʈ ��ũ��Ʈ
public class Story : MonoBehaviour , IPointerUpHandler , IPointerDownHandler
{
    [Header("# TutorialManager")]
    public Tutorial tutorial;

    [Header("# Story Text")]
    private string originText;
    public Text text;
    public GameObject touchText;
    public float baseTypingSpeed; // �⺻ Ÿ���� �ӵ�
    public float fastTypingSpeed; // ��ġ������ Ÿ���� �ӵ�
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
            if (!skipBtnActive) // ��ŵ â�� Ȱ��ȭ �� �ִٸ� �ؽ�Ʈ �ۼ� X
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
                    if (point == typingLength) // ��ġ Ȱ��ȭ
                    {
                        text.text = originText;
                        yield return new WaitForSeconds(0.5f);
                        text.text = originText + cursor_char;
                        storyTextEnd = true;
                        touchText.SetActive(true);
                    }
                }
                else // Ŀ�� ������
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

    // ��ġ�� ���� �� �̺�Ʈ
    public void OnPointerUp(PointerEventData eventData)
    {
        if (typingSpeed == fastTypingSpeed && eventData.button == PointerEventData.InputButton.Left)
        {
            typingSpeed = baseTypingSpeed;
        }
    }
    // ��ġ�� �������� �� �̺�Ʈ
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
