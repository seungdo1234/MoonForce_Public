using UnityEngine;
using System.Collections.Generic;
public class Scanner : MonoBehaviour
{
    // ����, ���̾�, ��ĵ ��� �迭, ���� ����� Ÿ��
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform[] nearestTarget;

    private void FixedUpdate()
    {
        // CircleCastAll : ������ ĳ��Ʈ�� ��� ��� ����� ��ȯ�ϴ� �Լ�
        // CircleCastAll(ĳ���� ���� ��ġ, ���� ������, ĳ���� ����, ĳ���� ����, ��� ���̾�)
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer); // ���� �����ȿ� ���� ������ Ÿ�ٹ迭�� ����
        nearestTarget = GetNearest();
    }

    // ���� ����� Enemy Transform ���� ��ȯ
    private Transform[] GetNearest()
    {
        Transform[] result = new Transform[GameManager.instance.statManager.weaponNum]; // �߻��ϴ� źȯ�� ���� ��ŭ �ʱ�ȭ
        List<Transform> uniqueTargets = new List<Transform>(); // �̹� Target���� ������ Enemy ����Ʈ

        for (int i = 0; i < GameManager.instance.statManager.weaponNum; i++) // ���� ����� ������� �迭�� ����
        {
            float closestDistance = float.MaxValue;
            Transform closestTarget = null;

            foreach (RaycastHit2D target in targets)
            {
                if (uniqueTargets.Contains(target.transform)) // �ش� ���� �̹� scan�� �ƴٸ� Pass
                {
                    continue;
                }

                float distance = Vector3.Distance(transform.position, target.transform.position); // ���� ��ġ�� ���� ��ġ�� ����

                if (distance < closestDistance) // ���� ����� �� ������ ����
                {
                    closestDistance = distance;
                    closestTarget = target.transform;
                }
            }

            if (closestTarget != null)  // ���� ����� ���� scanner �迭�� ����
            {
                uniqueTargets.Add(closestTarget);
                result[i] = closestTarget;
            }

            if(targets.Length == i + 1) // ������ ���� ������ Ÿ���̶�� Break;
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
