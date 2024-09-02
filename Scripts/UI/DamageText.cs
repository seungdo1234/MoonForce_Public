using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public TextType type;
    private Text text;


    private Vector3 target;
    private RectTransform rect;
    private void Awake()
    {
        text = GetComponent<Text>();
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
//        StartCoroutine(Alpha(1,0));
    }

    public void Init(int damage, Vector3 target)
    {
        this.target = target;

        if(type == TextType.Damage)
        {
            text.text = string.Format("{0}", damage);
            if (GameManager.instance.attribute == ItemAttribute.Holy && damage == 999)
            {
                text.color = new Color(1, 1, 0.4f);
            }
        }
        else
        {
            text.text = string.Format("+{0}", damage);
        }


        gameObject.SetActive(true);

    }

    public void TextEnd()
    {
        target = Vector3.zero;


        if (type == TextType.Damage)
        {
            text.color = new Color(1, 0.4f, 0.4f);
        }
        else
        {
            text.color = new Color(0.78f, 0.15f, 0.15f);
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(target != Vector3.zero)
        {
            Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(target);
            rect.position = targetScreenPos;
        }
    }
}
