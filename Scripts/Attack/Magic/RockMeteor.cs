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

        for (int i = 0; i < wayPoints.Length; i++) // ������ �ʱ� ���� ������ ���� ������
        {
            rockLocalPos[i] = wayPoints[i].localPosition;
        }

    }

    public void Init(float coolTime, int slotNum)
    {
        this.coolTime = coolTime;
        this.slotNum = slotNum;

        for (int i = 0; i < wayPoints.Length; i++) // Ȱ��ȭ�� ���� ���� ��� ��Ȱ��ȭ
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
                    wayPoints = ArrayShuffle(wayPoints); // ����
                    ready = true;
                }
                yield return null;

            }
            else
            {
                // �÷��̾��� �̵� ��ǥ�� �°� ���� ����߸�
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

    private T[] ArrayShuffle<T>(T[] array) // ����
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
