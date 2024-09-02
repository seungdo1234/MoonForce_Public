using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackEnemy : MonoBehaviour
{
    private Player player;
    private Enemy enemy;

    public float delay; // �غ� �Ϸ� ���� �� Shot������ �ð�
    public float coolTime; // Shot�� �� �� ���� Shot������ ��Ÿ��
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
                float distacne = Vector3.Distance(transform.position, target.position); // Player���� �Ÿ� ����
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
                    dir = dir.normalized; // ����ȭ

                    Transform bullet = GameManager.instance.pool.Get((int)PoolList.EnemyBullet).transform;

                    bullet.position = transform.position; // bullet�� ��ġ
                                                          // FromToRotation : ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�

                    // ���Ÿ� ������ Count�� �����
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
