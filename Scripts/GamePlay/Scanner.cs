using UnityEngine;
using System.Collections.Generic;
public class Scanner : MonoBehaviour
{
    // 범위, 레이어, 스캔 결과 배열, 가장 가까운 타겟
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform[] nearestTarget;

    private void FixedUpdate()
    {
        // CircleCastAll : 원형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        // CircleCastAll(캐스팅 시작 위치, 원의 반지름, 캐스팅 방향, 캐스팅 길이, 대상 레이어)
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer); // 공격 범위안에 들어온 적들을 타겟배열에 넣음
        nearestTarget = GetNearest();
    }

    // 가장 가까운 Enemy Transform 정보 반환
    private Transform[] GetNearest()
    {
        Transform[] result = new Transform[GameManager.instance.statManager.weaponNum]; // 발사하는 탄환의 갯수 만큼 초기화
        List<Transform> uniqueTargets = new List<Transform>(); // 이미 Target으로 설정된 Enemy 리스트

        for (int i = 0; i < GameManager.instance.statManager.weaponNum; i++) // 가장 가까운 순서대로 배열에 넣음
        {
            float closestDistance = float.MaxValue;
            Transform closestTarget = null;

            foreach (RaycastHit2D target in targets)
            {
                if (uniqueTargets.Contains(target.transform)) // 해당 적이 이미 scan이 됐다면 Pass
                {
                    continue;
                }

                float distance = Vector3.Distance(transform.position, target.transform.position); // 현재 위치와 적의 위치를 구함

                if (distance < closestDistance) // 가장 가까운 적 정보를 저장
                {
                    closestDistance = distance;
                    closestTarget = target.transform;
                }
            }

            if (closestTarget != null)  // 가장 가까운 적을 scanner 배열에 넣음
            {
                uniqueTargets.Add(closestTarget);
                result[i] = closestTarget;
            }

            if(targets.Length == i + 1) // 범위에 들어온 마지막 타겟이라면 Break;
            {
                break;
            }
        }

        return result;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, scanRange);
    }
}
