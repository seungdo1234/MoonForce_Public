using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoe : MonoBehaviour
{
    public float rotationSpeed; // 회전 속도
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

    private IEnumerator ThrowStart() { // 던지는 코루틴
        Vector3 initialPosition = transform.position;  // 던졌을 때 위치 저장


        rigid.velocity = throwDirection * throwSpeed; // Target 방향으로 속도 설정
        while (true)
        {
            if (!isReturning)
            {
                Vector3 currentPosition = transform.position; // 현재 위치 저장
                float distance = Vector3.Distance(initialPosition, currentPosition); // 현재 위치와 던질 때 위치와 maxDistance 보다 크다면 Hoe를 Player에게 리턴

                if (distance >= maxDistance)
                {
                    sprite.flipX = !sprite.flipX;
                    isReturning = true;
                }
            }
            else
            {
                Vector3 targetDir = (player.position - transform.position).normalized; // 플레이어는 움직이기 때문에 매 프레임 마다 방향 값을 정함

                rigid.velocity = targetDir * throwSpeed; // 해당 방향으로 속도 설정
            }

            if (isReturning && Vector3.Distance(player.position, transform.position) < 0.5f) // Player와 충돌
            {
                // Hoe 비활성화
                gameObject.SetActive(false);
            }

            yield return null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (sprite.flipX) // 플레이어의 방향에 따라 Hoe가 회전하는 방향이 다르기 떄문에 회전을 다르게 줌
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        }
        else
        {
            transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        }
    }
}
