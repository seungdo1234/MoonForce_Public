using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrow : MonoBehaviour
{
    public float throwSpeed;

    private BoxCollider2D col;
    private Rigidbody2D rigid;
    private Animator anim;
    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    public void Init(Vector3 dir)
    {
        col.enabled = true;
        rigid.velocity = throwSpeed * dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }

        col.enabled = false;

        rigid.velocity = Vector2.zero;

        anim.SetTrigger("RockThrowHit");
    }
}
