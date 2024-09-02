using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemy : MonoBehaviour
{
    public float rushDistance; // 대쉬 거리
    public float targetDetationDistance; // Player 탐지 거리
    public float castTime; // 시전 시간
    public float rushSpeed; // 몇초만에 대쉬 거리를 갈것인지.
    public float rushDelayTime; // 러쉬 쿨타임
    public float pushForce; // 돌진할 때 다른 Enemy를 밀어내는 힘
    public bool isReady; // 준비가 됐는지 
    public bool isAttack; // 플레이어를 공격 했는지
    public bool isRushing; // 돌진 중 일 때

    private Rigidbody2D rigid;
    private Animator anim;
    private Enemy enemy;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

    }

    public void Init()
    {
        StartCoroutine(RushStart());
    }

    private IEnumerator RushStart()
    {
        float curTime = 0f;
        Transform target = enemy.target.transform;
        Vector3 dir = Vector3.zero;
        Vector3 initialPosition = Vector3.zero;

        while (!GameManager.instance.gameStop)
        {

            if (!isReady)
            {
                float distance = Vector3.Distance(transform.position, target.position); // Player와 Enemy사이의 거리를 구함

                if(!enemy.isRestraint && distance < targetDetationDistance) // targetDetationDistance안에 들어왔다면 돌진할 준비를 함
                {
                    isReady = true;
                    initialPosition = transform.position; // 현재 위치 저장
                    dir = (target.position - transform.position).normalized; // 방향 구함
                    anim.speed = 0f; // 애니메이션 속도 0
                    rigid.velocity = Vector3.zero; // 혹시나 프레임 차이로 속도가 생길 수 있기 떄문에 초기화
                    enemy.spriteRenderer.flipX = dir.x > 0 ? false : true; // Player 방향으로 Flip
                }
            }
            else
            {
                if (enemy.isRestraint) // 준비 중에 속박에 걸린다면 돌진 초기화
                {
                    isReady = false;
                    curTime = 0;
                }
                curTime += Time.deltaTime;

                if(curTime > castTime) // 캐스팅 시간이 되면
                {

                    rigid.velocity = dir * rushSpeed; // 해당 방향으로 돌진
                    anim.speed = 1f;
                    rigid.mass = 1000;
                    isRushing = true;
                    float timer = 0;
                    while (true)
                    {
                        timer += Time.deltaTime;
                        float distance = Vector3.Distance(transform.position, initialPosition); // 처음 위치와 현재 위치가 rushDistance 값만큼 벌어지면 break
                        if (distance> rushDistance || isAttack || enemy.isRestraint || timer > 5) // 혹은 Player을 공격했거나 (isAttacking), enemy의 상태가 움직일 수 없는 상태라면 (isRestraint) 그 즉시 돌진을 끝냄
                        {
                            break;
                        }
                        yield return null;
                    }

                    yield return null;
                    isReady = false;
                    isRushing = false;
                    rigid.mass = 100;
                    isAttack = false;
                    curTime = 0f;

                    yield return new WaitForSeconds(rushDelayTime);
                }
            }
     
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // 플레이어와 충돌했을 때
    {

        if(enemy.enemyType != 3)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player")|| (isRushing && collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<Enemy>().enemyType == 3))
        {
            isAttack = true;
        }
    }

}
