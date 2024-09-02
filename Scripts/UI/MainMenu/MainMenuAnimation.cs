using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimation : MonoBehaviour
{
    public Transform[] wayPoints;
    public Transform charactor;
    public float lerpTime;

    public bool isRotation;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(RunStart());
    }

    private IEnumerator RunStart() // �޸��� �ִϸ��̼� ����
    {
        int point = 0; // ó�� ���� ����Ʈ
        int dir = 1; // x����
        while (true)
        {
            // ���� -��� �ø�
            bool isFlip = wayPoints[point + dir].position.x - wayPoints[point].position.x > 0 ? false : true; 
            isRotation = isFlip ? true : false; 

            charactor.rotation = isRotation ? Quaternion.Euler(0, 0, -180) : Quaternion.Euler(0, 0, 0); // ȸ��

            float currentTime = 0f; 
            while (currentTime < lerpTime) // lerpTime ���� �޸���
            {
                currentTime += Time.deltaTime;
                float xValue = Mathf.Lerp(wayPoints[point].position.x, wayPoints[point + dir].position.x, currentTime / lerpTime);

                charactor.position = new Vector3(xValue, wayPoints[point].position.y, 0);
                yield return null;
            }
            yield return new WaitForSeconds(1f); // ��� ����

            point += 2 * dir; // ��������Ʈ�� 2���� ¦�����Ƿ� 2�� + Ȥ�� -

            if ((dir == 1 && point >= wayPoints.Length ) || (dir == -1 && point < 0)) // ������ Waypoint�϶� 
            {
                dir *= -1; // ���� �ٲ��ֱ�

                point +=  dir; // + dir (2�� Ŀ���ų� �۾����� ������ point ���� 6 Ȥ�� -1 �� ����)
            }

        }
    }
    private void Update()
    {
        
    }
}
