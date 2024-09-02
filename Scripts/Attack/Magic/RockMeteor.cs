using System.Collections;
using UnityEngine;

public class RockMeteor : MonoBehaviour
{
    public Transform[] wayPoints;
    public Vector3[] rockLocalPos;

    public float delayTime;
    public float coolTime;
    private int slotNum;
    private Player player;
    private void Start()
    {
        player = GameManager.instance.player;

        rockLocalPos = new Vector3[wayPoints.Length]; 

        for (int i = 0; i < wayPoints.Length; i++) // 돌들의 초기 로컬 포지션 값을 저장함
        {
            rockLocalPos[i] = wayPoints[i].localPosition;
        }

    }

    public void Init(float coolTime, int slotNum)
    {
        this.coolTime = coolTime;
        this.slotNum = slotNum;

        for (int i = 0; i < wayPoints.Length; i++) // 활성화된 돌이 있을 경우 비활성화
        {
            if (wayPoints[i].gameObject.activeSelf)
            {
                wayPoints[i].gameObject.SetActive(false);
            }
        }

        StartCoroutine(RockMeteorStart());
    }

    private IEnumerator RockMeteorStart()
    {
        int point = 0;
        float curTime = 0;
        bool ready = false;

        while (!GameManager.instance.gameStop)
        {
            if (!ready)
            {
                curTime += Time.deltaTime;
                if (curTime >= coolTime)
                {
                    wayPoints = ArrayShuffle(wayPoints); // 셔플
                    ready = true;
                }
                yield return null;

            }
            else
            {
                // 플레이어의 이동 좌표에 맞게 돌을 떨어뜨림
                wayPoints[point].localPosition = new Vector3(rockLocalPos[point].x + player.transform.position.x, rockLocalPos[point].y + player.transform.position.y, 0);
            
                wayPoints[point].gameObject.SetActive(true);
                point++;
                yield return new WaitForSeconds(delayTime);

                if (point == wayPoints.Length)
                {
                    ready = false;
                    curTime = 0;
                    point = 0;
                    GameManager.instance.magicManager.coolTimeUI.CoolTimeStart(slotNum);
                }

            }



        }
    }

    private T[] ArrayShuffle<T>(T[] array) // 셔플
    {
        T temp;
        int random1, random2;

        for (int i = 0; i < array.Length; ++i)
        {
            random1 = Random.Range(0, array.Length);
            random2 = Random.Range(0, array.Length);

            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp;
        }

        return array;
    }

}
