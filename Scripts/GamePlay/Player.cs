using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public FloatingJoystick joy;
    public Vector2 inputVec;

    public GameObject resurrection;

    [Header("Body of Rotation")]
    public Transform rotationBody;

    [Header("Attack Target")]
    public Scanner scanner;

    private bool isDamaged;
    private Animator anim;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    private void OnEnable()
    {
        if (isDamaged)
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
            gameObject.tag = "Player";
            gameObject.layer = 7;
            isDamaged = false;
        }

    }

    private void FixedUpdate()
    {
        if (GameManager.instance.gameStop)
        {
            return;
        }

        inputVec = new Vector2(joy.Horizontal, joy.Vertical);

        // 정규화된 벡터를 생성
        Vector2 nextVec = inputVec.normalized * Time.fixedDeltaTime * GameManager.instance.statManager.moveSpeed;

        // 위치 이동
        rigid.MovePosition(rigid.position + nextVec);

    }
    private void LateUpdate()
    {
        if (GameManager.instance.gameStop)
        {
            anim.SetFloat("Run", 0);
            return;
        }

        // magnitude : 벡터 값의 순수 길이
        anim.SetFloat("Run", inputVec.magnitude);

        if(inputVec.magnitude != 0)
        {
            AudioManager.instance.FootStepSfxPlayer();
        }

        if (inputVec.x != 0)
        {
            // inputVec.x < 0이 참이라면 1, 거짓이라면 -1로 참이면 1이므로 flipX가 true가 됨. 
            spriteRenderer.flipX = inputVec.x < 0;
        }
    }

    private IEnumerator OnDamaged() // 데미지를 받았을 때 2초동안 데미지를 받지않는 상태가 되고 이동속도가 빨라짐
    {
        GameManager.instance.cameraShake.ShakeCamera(0.5f, 5, 5);
        gameObject.tag = "PlayerDamaged";
        gameObject.layer = 8;
        GameManager.instance.statManager.moveSpeed *= 1.2f;
        spriteRenderer.color = new Color(1, 1, 1, 0.7f);
        yield return new WaitForSeconds(2f);
        GameManager.instance.statManager.moveSpeed /= 1.2f;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        gameObject.tag = "Player";
        gameObject.layer = 7;
        isDamaged = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.instance.gameStop || !collision.gameObject.CompareTag("Enemy") || isDamaged)
        {
            return;
        }
        isDamaged = true; 

        int damage = collision.gameObject.GetComponent<Enemy>().damage;

        PlayerHit(damage);

    }
    private IEnumerator Resurrection()
    {
        resurrection.SetActive(true);

        yield return new WaitForSeconds(1.4f);

        resurrection.SetActive(false);
    }
    public void PlayerHit(int damage)
    {
        damage -= GameManager.instance.enforce.enforceInfo[(int)EnforceName.DeffenseUp].curLevel;

        GameManager.instance.statManager.curHealth -= damage;
        //   AudioManager.instance.PlayerSfx(AudioManager.Sfx.Hit);

        if (GameManager.instance.statManager.curHealth > 0)
        {
            AudioManager.instance.PlayerSfx(Sfx.Hurt);

            StartCoroutine(OnDamaged());
        }
        else if (GameManager.instance.statManager.curHealth <= 0)
        {
            if(GameManager.instance.attribute == ItemAttribute.Holy && !GameManager.instance.isResurrection)
            {
                GameManager.instance.isResurrection = true;
                StartCoroutine(OnDamaged());
                GameManager.instance.statManager.curHealth = GameManager.instance.statManager.maxHealth / 2;
                StartCoroutine(Resurrection());
                return;
            }
            isDamaged = false;
            anim.SetTrigger("Dead");
            GameManager.instance.GameEnd(1);
        }
    }
}
