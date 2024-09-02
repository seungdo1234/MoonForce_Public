using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Player player;



    private float timer;
    private void Start()
    {
        player = GameManager.instance.player;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameStop || GameManager.instance.maxGameTime - GameManager.instance.curGameTime < 0.5f)
        {
            return;
        }

        if(timer < GameManager.instance.statManager.rate)
        {
            timer += Time.deltaTime;
        }


        if (player.scanner.nearestTarget[0] && timer > GameManager.instance.statManager.rate) 
        {
            timer = 0;
            Fire();
        }
    }

    private void Fire() // 가장 가까운 Enemy한테 총알 발사
    {
        // Enemy 위치, 방향 구하기
        for(int i = 0; i< GameManager.instance.statManager.weaponNum; i++)
        {
            if(player.scanner.nearestTarget[i] == null)
            {
                break;
            }
            Vector3 targetPos = player.scanner.nearestTarget[i].position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized; // 정규화

            // bullet 생성
            Transform bullet = GameManager.instance.pool.Get((int)PoolList.PlayerBullet).transform;
            bullet.position = transform.position; // bullet의 위치
                                                  // FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // Enemy 방향으로 bullet 회전

            if(GameManager.instance.attribute == ItemAttribute.Dark) // 어둠속성은 관통력 1 고정
            {
                bullet.GetComponent<Bullet>().Init(GameManager.instance.statManager.attack, 0, dir, 10);
            }
            else
            {
                bullet.GetComponent<Bullet>().Init(GameManager.instance.statManager.attack, GameManager.instance.statManager.penetration - 1, dir, 10);
            }
        }
    }


    


}
