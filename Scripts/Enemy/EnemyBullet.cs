using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage;
    public float rotationSpeed;
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(int damage,  Vector3 dir, float throwSpeed)
    {
        this.damage = damage;
        rigid.velocity = dir * throwSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        collision.GetComponent<Player>().PlayerHit(damage);
        // �ӵ� �ʱ�ȭ
        rigid.velocity = Vector2.zero;
        // ��Ȱ���ϱ� ���� bullet ��Ȱ��ȭ
        gameObject.SetActive(false);

      

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
    }
}
