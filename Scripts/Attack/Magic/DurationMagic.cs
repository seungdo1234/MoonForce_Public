using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationMagic : MonoBehaviour
{
    public float lerpTime;

    private Animator anim;
    private PolygonCollider2D col;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<PolygonCollider2D>();
    }

    private void OnEnable()
    {
        col.enabled = true;

        StartCoroutine(MagicPlay());
    }

    private IEnumerator MagicPlay()
    {

        yield return new WaitForSeconds(lerpTime);

        anim.SetTrigger("End");

        col.enabled = false;

    }
}
