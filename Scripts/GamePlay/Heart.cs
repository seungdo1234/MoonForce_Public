using System.Collections;
using UnityEngine;

public class Heart : MonoBehaviour
{

    public Sprite[] heartSprite;
    public float[] healthUpValue;
    public float lerpTime;
    public float stageClearRange; // 스테이지 클리어 시 늘어하는 범위 배수
    public float baseScanRange;
    public float scanRange;
    public Vector3 textPos;
    private int healthType;
    private SpriteRenderer sprite;
    private Player player;
    private Animator anim;

    // 초기화
    public void Init(int type)
    {
        healthType = type;

        if (sprite == null)
        {
            anim = GetComponent<Animator>();
            player = GameManager.instance.player;
            sprite = GetComponent<SpriteRenderer>();
        }

        scanRange = baseScanRange;
        anim.speed = 1f;
        sprite.sprite = heartSprite[healthType];

        StartCoroutine(PlayerScan());
    }
    public void StageClear() {
        StartCoroutine(DistanceUp());
    }

    // 스테이지 클리어 시 주변 하트들을 빨아드림
    private IEnumerator DistanceUp()
    {
        scanRange *= stageClearRange;
        anim.speed = 0f;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    // 플레이어 스캔
    private IEnumerator PlayerScan()
    {

        while (true)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if(GameManager.instance.statManager.curHealth > 0 && distance <= scanRange)
            {
                ScanOn();
                break;
            }

            yield return null;
        }
    }

    // 스캔 O
    public void ScanOn()
    {
        anim.speed = 0f;
        StartCoroutine(HeartMove());
    }

    // 플레이어에게 이동
    private IEnumerator HeartMove()
    {
        float timer = 0;

        while (timer < lerpTime)
        {
            timer += Time.deltaTime;

            float posX = Mathf.Lerp(transform.position.x, player.transform.position.x, timer / lerpTime);
            float posy = Mathf.Lerp(transform.position.y, player.transform.position.y, timer / lerpTime);

            transform.position = new Vector3(posX, posy, 0);

            yield return null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) // 힐
    {
        if ((!collision.CompareTag("Player") && !collision.CompareTag("PlayerDamaged")))
        {
            return;
        }

        GameManager.instance.statManager.curHealth = Mathf.Min(GameManager.instance.statManager.curHealth + healthUpValue[healthType], GameManager.instance.statManager.maxHealth);

        Vector3 pos = player.transform.position + textPos;
        GameManager.instance.damageTextPool.Get((int)TextType.Heal, (int)healthUpValue[healthType], pos);
        AudioManager.instance.PlayerSfx(Sfx.Heal);

        gameObject.SetActive(false);
    }
}
