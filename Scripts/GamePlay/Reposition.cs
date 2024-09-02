using UnityEngine;

public class Reposition : MonoBehaviour
{
    public Vector3 initPos;

    public float distanceRange;
    // 모든 콜라이더를 아우르는 클래스
    private Collider2D col;
    private Enemy enemy;
    private void Awake()
    {
        col = GetComponent<Collider2D>();

        initPos = transform.position;

        if (transform.CompareTag("Enemy"))
        {
            enemy = GetComponent<Enemy>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || GameManager.instance.gameStop )
        {
            return;
        }

        // 거리 확인 (X축으로 벗어났는지 Y축으로 벗어났는지)
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag)
        {
            case "Ground":

                float differX = playerPos.x - myPos.x;
                float differY = playerPos.y - myPos.y;

                float dirX = differX < 0 ? -1 : 1;
                float dirY = differY < 0 ? -1 : 1;

                differX = Mathf.Abs(differX);
                differY = Mathf.Abs(differY);

                if (differX > differY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (differX < differY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                else
                {
                    transform.Translate(Vector3.right * dirX * 40);
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                if (col.enabled) // 만약 콜라이더가 살아있다면
                {                

                    if (enemy.isRestraint)
                    {
                        enemy.isRestraint = false;
                        enemy.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    }

                    Vector3 dist = playerPos - myPos;
                    Vector3 rand = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0); // 랜덤

                    transform.Translate(rand + dist * 2);
                }
                break;
        }

    }


}
