using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCutter : MonoBehaviour
{

    public float speed;

    private Rigidbody2D rigid;
    private int magicCount;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Init()
    {

        rigid.velocity = speed * transform.right;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }

        gameObject.SetActive(false);
    }
}
