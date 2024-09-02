using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inferno : MonoBehaviour // 화염속성 레전드리 무기의 스킬
{

    public GameObject[] infernos; // 인페르노 오브젝트


    public float coolTime;
    private int slotNum;
    private Player player;
    private void Start()
    {
        player = GameManager.instance.player;
    }
    public void Init(float coolTime , int slotNum)
    {
        this.coolTime = coolTime;
        this.slotNum = slotNum;

        for(int i =0; i < infernos.Length; i++) // 생성전 초기화
        {
            if (infernos[i].activeSelf)
            {
                infernos[i].SetActive(false);
            }
        }

        StartCoroutine(InfernoStart());
    }

    private IEnumerator InfernoStart() //시작
    {
        int point = 0;
        float timer = 0;
        bool skillOn = false;
        while(true)
        {
            if(!skillOn)
            {
                timer += Time.deltaTime;
                if(timer >= coolTime)
                {
                    skillOn = true;
                    timer = 0;
                    transform.position = player.transform.position;
                }
                yield return null;
            }
            else
            {
                infernos[point].SetActive(true);
                point++;
                if(point == infernos.Length)
                {
                    GameManager.instance.magicManager.coolTimeUI.CoolTimeStart(slotNum);
                    point = 0;
                    skillOn = false;
                }
                yield return new WaitForSeconds(0.1f);
            }

        }
    }

}
