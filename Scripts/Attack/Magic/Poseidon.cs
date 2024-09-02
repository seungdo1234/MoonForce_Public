using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poseidon : MonoBehaviour
{
    public GameObject[] posedions;
    public float coolTime;
    private int slotNum;
    public void Init(float coolTime, int slotNum)
    {
        this.coolTime = coolTime;
        this.slotNum = slotNum;

        for (int i = 0; i < posedions.Length; i++) // 생성전 초기화
        {
            if (posedions[i].activeSelf)
            {
                posedions[i].SetActive(false);
            }
        }

        StartCoroutine(PosedionsStart());
    }
    private IEnumerator PosedionsStart()
    {

        while (!GameManager.instance.gameStop)
        {
            yield return new WaitForSeconds(coolTime);

            transform.position = GameManager.instance.player.transform.position;

            for (int i =0;i <posedions.Length; i++)
            {
                posedions[i].SetActive(true);
            }

            GameManager.instance.magicManager.coolTimeUI.CoolTimeStart(slotNum);
        }
    }
}
