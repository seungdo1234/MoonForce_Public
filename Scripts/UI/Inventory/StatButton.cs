using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatButton : MonoBehaviour
{

    public GameObject noitceText;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0f;
        noitceText.SetActive(false);
    }

    private void OnEnable()
    {
        if(GameManager.instance.availablePoint > 0)
        {
            anim.speed = 1f;
            noitceText.SetActive(true);
        }
    }
    private void OnDisable()
    {
        anim.speed = 0f;
        noitceText.SetActive(false);
    }
}
