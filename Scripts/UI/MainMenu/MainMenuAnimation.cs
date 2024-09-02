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

    private IEnumerator RunStart() // 달리기 애니메이션 시작
    {
        int point = 0; // 처음 시작 포인트
        int dir = 1; // x방향
        while (true)
        {
            // 값이 -라면 플립
            bool isFlip = wayPoints[point + dir].position.x - wayPoints[point].position.x > 0 ? false : true; 
            isRotation = isFlip ? true : false; 

            charactor.rotation = isRotation ? Quaternion.Euler(0, 0, -180) : Quaternion.Euler(0, 0, 0); // 회전

            float currentTime = 0f; 
            while (currentTime < lerpTime) // lerpTime 동안 달리기
            {
                currentTime += Time.deltaTime;
                float xValue = Mathf.Lerp(wayPoints[point].position.x, wayPoints[point + dir].position.x, currentTime / lerpTime);

                charactor.position = new Vector3(xValue, wayPoints[point].position.y, 0);
                yield return null;
            }
            yield return new WaitForSeconds(1f); // 잠깐 쉬기

            point += 2 * dir; // 웨이포인트는 2개씩 짝지으므로 2씩 + 혹은 -

            if ((dir == 1 && point >= wayPoints.Length ) || (dir == -1 && point < 0)) // 마지막 Waypoint일때 
            {
                dir *= -1; // 방향 바꿔주기

                point +=  dir; // + dir (2씩 커지거나 작아지기 때문에 point 값이 6 혹은 -1 이 나옴)
            }

        }
    }
    private void Update()
    {
        
    }
}
