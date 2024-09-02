using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackEnemy : MonoBehaviour
{
    private Player player;
    private Enemy enemy;

    public float delay; // 준비가 완료 됐을 때 Shot까지의 시간
    public float coolTime; // Shot을 한 뒤 다음 Shot까지의 쿨타임
    public float rangeDistance;
    public float throwSpeed;
    public bool isReady = true;
    private void Awake()
    {
        player = GameManager.instance.player;
        enemy = GetComponent<Enemy>();
    }

    public void Init()
    {
        isReady = true;

        StartCoroutine(ShotStart());
    }

    private IEnumerator ShotStart()
    {
        float curTime = 0;
        Transform target = player.transform;

        while (!GameManager.instance.gameStop)
        {

            if (isReady)
            {
                float distacne = Vector3.Distance(transform.position, target.position); // Player와의 거리 구함
                if(distacne < rangeDistance)
                {
                    isReady = false;

                    enemy.rigid.velocity = Vector3.zero;
                }
            }
            else
            {
               
                curTime += Time.deltaTime;

                if(curTime > delay)
                {
                    curTime = 0;

                    Vector3 dir = target.position - transform.position;
                    dir = dir.normalized; // 정규화

                    Transform bullet = GameManager.instance.pool.Get((int)PoolList.EnemyBullet).transform;

                    bullet.position = transform.position; // bullet의 위치
                                                          // FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수

                    // 원거리 공격은 Count는 관통력
                    bullet.GetComponent<EnemyBullet>().Init(enemy.damage,  dir, throwSpeed);

                    yield return new WaitForSeconds(delay);

                    isReady = true;

                    yield return new WaitForSeconds(coolTime);

                }
            }

            yield return null;
        }
    }

}
