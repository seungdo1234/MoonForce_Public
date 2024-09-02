using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoe : MonoBehaviour
{
    public float rotationSpeed; // ȸ�� �ӵ�
    public float lerpTime;


    private Transform player;
    private Vector3 throwDirection;
    public float throwSpeed = 10f;
    public float maxDistance = 10f;
    private bool isReturning = false;
    private SpriteRenderer sprite;
    private Rigidbody2D rigid;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameManager.instance.player.transform;
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector3 dir)
    {
        isReturning = false;

        sprite.flipX = dir.x < 0 ? true : false;

        throwDirection = dir;
        StartCoroutine(ThrowStart());
    }

    private IEnumerator ThrowStart() { // ������ �ڷ�ƾ
        Vector3 initialPosition = transform.position;  // ������ �� ��ġ ����


        rigid.velocity = throwDirection * throwSpeed; // Target �������� �ӵ� ����
        while (true)
        {
            if (!isReturning)
            {
                Vector3 currentPosition = transform.position; // ���� ��ġ ����
                float distance = Vector3.Distance(initialPosition, currentPosition); // ���� ��ġ�� ���� �� ��ġ�� maxDistance ���� ũ�ٸ� Hoe�� Player���� ����

                if (distance >= maxDistance)
                {
                    sprite.flipX = !sprite.flipX;
                    isReturning = true;
                }
            }
            else
            {
                Vector3 targetDir = (player.position - transform.position).normalized; // �÷��̾�� �����̱� ������ �� ������ ���� ���� ���� ����

                rigid.velocity = targetDir * throwSpeed; // �ش� �������� �ӵ� ����
            }

            if (isReturning && Vector3.Distance(player.position, transform.position) < 0.5f) // Player�� �浹
            {
                // Hoe ��Ȱ��ȭ
                gameObject.SetActive(false);
            }

            yield return null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (sprite.flipX) // �÷��̾��� ���⿡ ���� Hoe�� ȸ���ϴ� ������ �ٸ��� ������ ȸ���� �ٸ��� ��
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        }
        else
        {
            transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        }
    }
}
