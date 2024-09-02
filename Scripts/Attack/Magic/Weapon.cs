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

    private void Fire() // ���� ����� Enemy���� �Ѿ� �߻�
    {
        // Enemy ��ġ, ���� ���ϱ�
        for(int i = 0; i< GameManager.instance.statManager.weaponNum; i++)
        {
            if(player.scanner.nearestTarget[i] == null)
            {
                break;
            }
            Vector3 targetPos = player.scanner.nearestTarget[i].position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized; // ����ȭ

            // bullet ����
            Transform bullet = GameManager.instance.pool.Get((int)PoolList.PlayerBullet).transform;
            bullet.position = transform.position; // bullet�� ��ġ
                                                  // FromToRotation : ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // Enemy �������� bullet ȸ��

            if(GameManager.instance.attribute == ItemAttribute.Dark) // ��ҼӼ��� ����� 1 ����
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
