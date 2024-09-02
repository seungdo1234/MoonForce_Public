using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantText : MonoBehaviour
{
    public Enchant enchant;
    private void Awake()
    {
    }
    public void NegativeText()
    {
        enchant.EnchantSelectTextOn(EnchantNoticeType.EncahntItemSelect);
        enchant.NoiticeTextAnimatorOff();
    }
}
