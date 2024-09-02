using System.Collections;
using UnityEngine;

public enum EnemyStatusEffect { Defalt, Burn, Wet, Earth, GrassRestraint, Darkness }
public class Enemy : MonoBehaviour
{
    [Header("# EnemyBaseStat")]
    public int damage;
    public float speed;
    public float health;
    public float maxHealth;
    public int enemyType;

    [Header("# EnemyHit")]
    public bool enemyKnockBack; // �˹��� ��
    public bool enemyDamaged; // �������� �޾Ҵٸ� ���� �ð����� �ȹް���
    public float damagedTime; // ���ʵ��� �����ϰ���
    public float knockBackValue; // �ǰ� �� �˹� ��ġ

    [Header("# EnemyStatusEffect")]
    public EnemyStatusEffect statusEffect;
    public bool isWetting; // wet �������� (�´ٸ� ���� ������ ++)
    public bool isRestraint;

    private int burnningDamage;
    private float lerpTime;

    [Header("# TargetPlayer")]
    public Rigidbody2D target; // Ÿ��


    public bool isLive; // Enmey�� ����ִ���

    private EnemyManager enemyManager;
    private RushEnemy rush;
    private RangeAttackEnemy rangeAttackEnemy;
    private int hitAnimID;
    public Rigidbody2D rigid;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    private Animator anim;
    private Collider2D col; // 2D�� Collider2D�� ��� �ݶ��̴��� ������ �� ����
    // ���� FixedUpdate���� ��ٸ�
    private WaitForFixedUpdate wait;

    private void Awake()
    {
        enemyManager = EnemyManager.instance;
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        hitAnimID = Animator.StringToHash("Hit");
        rush = GetComponent<RushEnemy>();
        rangeAttackEnemy = GetComponent<RangeAttackEnemy>();
    }


    private void FixedUpdate() // �������� �̵��� FixedUpdate
    {
        if (GameManager.instance.gameStop || !isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || rush.isReady || !rangeAttackEnemy.isReady || GameManager.instance.demeterOn || isRestraint) // Enemy�� �׾��ٸ� return
        {

            if (GameManager.instance.gameStop || GameManager.instance.demeterOn || !isLive || !rangeAttackEnemy.isReady || isRestraint)
            {
                rigid.velocity = Vector2.zero;
            }
            return;
        }


        // ������ ����
        Vector2 dirVec = target.position - rigid.position;
        // ���� �̵�
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        // �÷��̾�� �ε������� �ӵ��� ����� ������ �׻� 0���� �ʱ�ȭ
        if (!enemyKnockBack) // �˹� �� �϶� �ӵ��� �ʱ�ȭ�ϸ� �ȵǱ� ������
        {
            rigid.velocity = Vector2.zero;
        }
    }

    private void LateUpdate()
    {

        if ( GameManager.instance.gameStop || isRestraint || !isLive || rush.isReady || !rangeAttackEnemy.isReady || GameManager.instance.demeterOn)
        {
            return;
        }
        // ��������Ʈ ���� ��ȯ -> �÷��̾ Enmey���� ���ʿ� ���� ��
        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }

    // ��ũ��Ʈ�� Ȱ��ȭ �� ��, ȣ���ϴ� �Լ�
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true; // ���� ����
        enemyDamaged = false;
        col.enabled = true; // �ݶ��̴� Ȱ��ȭ
        rigid.simulated = true; // rigidbody2D Ȱ��ȭ
        spriteRenderer.sortingOrder = 3; // OrderLayer�� 3�� ����
        spriteRenderer.color = new Color(1, 1, 1, 1); // �÷� �ʱ�ȭ
        statusEffect = EnemyStatusEffect.Defalt; // ���� �ʱ�ȭ
        gameObject.layer = 6; // ���̾ Enemy�� ����
        anim.SetBool("Dead", false);
    }

    // Enemy ���� �� �ʱ�ȭ
    public void Init(SpawnData data)
    {
        enemyType = data.enemyType;

        // �ִϸ��̼��� �ش� ��������Ʈ�� �°� �ٲ���
        int randomSprite = Random.Range(0, enemyManager.enemyDatas[enemyType].animCon.Length);
        anim.runtimeAnimatorController = enemyManager.enemyDatas[enemyType].animCon[randomSprite];

        // ���� ����
        int quality = enemyManager.EnemyQuality();
        speed = enemyManager.speed + enemyManager.enemyDatas[enemyType].increasedSpeed + (enemyManager.enemyDatas[enemyType].enemyScaleBySpeed * quality);

        float addStat = enemyManager.enemyDatas[enemyType].enemyScaleByHealth * quality;
        maxHealth = Mathf.Floor(enemyManager.health * (enemyManager.enemyDatas[enemyType].healthPer + addStat));
        health = maxHealth;

        addStat = enemyManager.enemyDatas[enemyType].enemyScaleByDamage * quality;
        damage = Mathf.FloorToInt((float)enemyManager.damage * (enemyManager.enemyDatas[enemyType].damagePer + addStat));

        addStat = enemyManager.enemyDatas[enemyType].enemyScaleErrorRange * quality;
        transform.localScale = enemyManager.enemyDatas[enemyType].enemyBaseScale + new Vector3(addStat, addStat, addStat);

        rigid.mass = enemyManager.enemyDatas[enemyType].mass;
        if (enemyType == 3)
        {
            rush.Init();
        }
        else if(enemyType == 4)
        {
            rangeAttackEnemy.Init();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ����
        if (!(collision.gameObject.layer == 10 || collision.gameObject.layer == 11) || !isLive || enemyDamaged)
        {
            return;
        }
        

        if (collision.gameObject.layer == 11) // ���� ����ź�̶��
        {
            knockBackValue = GameManager.instance.statManager.knockBackValue;
            StatusEffect();
            EnemyDamaged(collision.GetComponent<Bullet>().damage, 1);
            damagedTime = 0.1f; // ����ź ���� ���� ���� ���� ���� �ڵ�
            StartCoroutine(IsDamaged());
        }
        else // �����̶�� 
        {
            int number = collision.GetComponent<MagicNumber>().magicNumber;
            float damage = GameManager.instance.statManager.attack * GameManager.instance.magicManager.magicInfo[number].damagePer;

            damagedTime = number != (int)MagicName.Tornado && number != (int)MagicName.ElectricShock ? 0.15f : 1f;
            StartCoroutine(IsDamaged());

            if (number == (int)MagicName.Inferno || number == (int)MagicName.Tornado || number == (int)MagicName.ElectricShock)
            {
                if(number == (int)MagicName.ElectricShock)
                {
                    GameObject elec = collision.gameObject;

                    StartCoroutine(ElectricShock(elec));
                }
            }

            knockBackValue = GameManager.instance.magicManager.magicInfo[number].knockBackPer * GameManager.instance.statManager.knockBackValue;

            EnemyDamaged(damage, 2);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) // �������� ���ظ� �ִ� ������ �浹 �� �϶�
    {
        if (collision.gameObject.layer != 10 || enemyDamaged || !isLive)
        {
            return;
        }

        int number = collision.GetComponent<MagicNumber>().magicNumber;

        if (number != (int)MagicName.Tornado && number != (int)MagicName.ElectricShock) // �������� ���ظ� �ִ� ������ �ƴ϶��
        {
            return;
        }

        StartCoroutine(IsDamaged());

        float damage = GameManager.instance.statManager.attack * GameManager.instance.magicManager.magicInfo[number].damagePer;

        EnemyDamaged(damage, 2);

        EnemyHit();
    }
    private IEnumerator IsDamaged() // Enemy�� �������� ���ظ� �ִ� ���� ���� ������� ���� �ð��ڿ� �������� �ޱ� ���� �ڷ�ƾ
    {
        enemyDamaged = true;
        yield return new WaitForSeconds(damagedTime);
        enemyDamaged = false;
    }
    private void StatusEffect()
    {
        switch (GameManager.instance.attribute)
        {
            case ItemAttribute.Fire: // �ҼӼ� �ǰ�
                lerpTime = GameManager.instance.statManager.burningEffectTime + 1;
                burnningDamage = (int)GameManager.instance.statManager.burningEffectTime;

                if (statusEffect != EnemyStatusEffect.Burn)
                {
                    statusEffect = EnemyStatusEffect.Burn;
                    StartCoroutine(Burning());
                }
                break;
            case ItemAttribute.Water: // ���Ӽ� �ǰ�
                lerpTime = GameManager.instance.statManager.wettingEffectTime;

                if (statusEffect != EnemyStatusEffect.Wet)
                {
                    statusEffect = EnemyStatusEffect.Wet;
                    StartCoroutine(Wetting());
                }

                break;
            case ItemAttribute.Grass: // Ǯ�Ӽ� �ǰ�
                lerpTime = GameManager.instance.statManager.restraintTime;
                if (statusEffect != EnemyStatusEffect.GrassRestraint)
                {
                    statusEffect = EnemyStatusEffect.GrassRestraint;
                    StartCoroutine(Restraint());
                }

                break;
            case ItemAttribute.Eeath: // ���Ӽ� �ǰ�

                lerpTime = GameManager.instance.statManager.speedReducedEffectTime;

                if (statusEffect != EnemyStatusEffect.Earth)
                {
                    statusEffect = EnemyStatusEffect.Earth;
                    StartCoroutine(ReducedSpeed());
                }

                break;
            case ItemAttribute.Dark: // ��ҼӼ� �ǰ�

                if (statusEffect != EnemyStatusEffect.Darkness)
                {
                    statusEffect = EnemyStatusEffect.Darkness;
                    StartCoroutine(DarknessExplosion());
                }
                break;
            default:
                return;

        }
    }
    private IEnumerator KnockBack()
    {
        if (!rush.isReady)
        {
            enemyKnockBack = true;
            yield return wait; // ���� �ϳ��� ���� �������� ������
            rigid.mass = 100;
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 dirVec = transform.position - playerPos;
            rigid.AddForce(dirVec.normalized * knockBackValue, ForceMode2D.Impulse);
            yield return wait; // ���� �ϳ��� ���� �������� ������
            rigid.mass = 1;
            enemyKnockBack = false;
        }
        else
        {
            yield return wait; // ���� �ϳ��� ���� �������� ������
        }
       

    }
    private IEnumerator ElectricShock(GameObject elec) // �����ũ�� �¾��� �� ����
    {
        float speed = this.speed;

        spriteRenderer.color = new Color(1, 1, 0.5f, 1);

        isRestraint = true;
        anim.speed = 0f;
        this.speed = 0f;
        rigid.velocity = Vector2.zero;

        while (true)
        {
            if (!elec.activeSelf)
            {
                break;
            }

            yield return null;
        }

        isRestraint = false;
        anim.speed = 1f;
        this.speed = speed;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    private IEnumerator Burning() // ȭ��
    {
        spriteRenderer.color = new Color(1, 0.7f, 0.7f, 1);

        while (lerpTime > 0)
        {
            lerpTime -= Time.deltaTime;
            if (lerpTime < burnningDamage)
            {
                burnningDamage--;
                EnemyDamaged(Mathf.Floor(GameManager.instance.statManager.attack * GameManager.instance.statManager.burningDamagePer), 2);
                anim.SetTrigger("Hit");
            }
            yield return null;
        }

        statusEffect = EnemyStatusEffect.Defalt;

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.color = new Color(1, 1, 1, 1);

    }

    private IEnumerator Wetting() // ���� ���¶�� ���� �������� ++ 
    {

        spriteRenderer.color = new Color(0.6f, 0.6f, 1, 1);

        isWetting = true;

        while (lerpTime > 0)
        {
            lerpTime -= Time.deltaTime;

            yield return null;
        }

        statusEffect = EnemyStatusEffect.Defalt;

        isWetting = false;

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    private IEnumerator ReducedSpeed() // �� �Ӽ� ������ ���� ���¶�� �̵��ӵ� --
    {
        float speed = this.speed;

        spriteRenderer.color = new Color(1, 0.6f, 0.3f, 1);

        anim.speed = 1 - GameManager.instance.statManager.speedReducePer;

        this.speed -= this.speed * GameManager.instance.statManager.speedReducePer;

        while (lerpTime > 0)
        {
            lerpTime -= Time.deltaTime;

            yield return null;
        }

        statusEffect = EnemyStatusEffect.Defalt;

        this.speed = speed;
        anim.speed = 1;

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    private IEnumerator Restraint() // Ǯ �Ӽ� ������ ���� ���¶�� �ӹ�
    {
        float speed = this.speed;

        spriteRenderer.color = new Color(0, 1, 0, 1);

        isRestraint = true;
        anim.speed = 0f;
        gameObject.layer = 9;
        spriteRenderer.sortingOrder = 2; // �ӹڴ��� ���� �������� Enemy�� ������ �ʱ��ϱ����� 
        this.speed = 0f;
        rigid.velocity = Vector2.zero;

        anim.ResetTrigger(hitAnimID); // �ִϸ��̼��� ����
        while (lerpTime > 0)
        {
            lerpTime -= Time.deltaTime;

            yield return null;
        }

        isRestraint = false;

        statusEffect = EnemyStatusEffect.Defalt;

        anim.speed = 1f;
        spriteRenderer.sortingOrder = 3;
        gameObject.layer = 6;
        this.speed = speed;
        spriteRenderer.color = new Color(1, 1, 1, 1);

    }
    private IEnumerator DarknessExplosion() 
        // ��� �Ӽ� ������ �¾��� �� ���� �ð� �� ����
    {

        spriteRenderer.color = new Color(1, 0.45f, 1, 1);

        lerpTime = GameManager.instance.statManager.darknessExpTime;

        while (lerpTime > 0)
        {
            lerpTime -= Time.deltaTime;

            yield return null;
        }

        ExplosionSpawn();

        statusEffect = EnemyStatusEffect.Defalt;

        spriteRenderer.color = new Color(1, 1, 1, 1);

    }

    private void ExplosionSpawn()
    {

        Transform exp = GameManager.instance.magicManager.Get(0).transform;

        GameManager.instance.magicManager.magicInfo[0].damagePer = GameManager.instance.statManager.darkExplosionDamagePer;

        exp.position = transform.position;

        float expScale = (GameManager.instance.statManager.penetration - 1) * 0.15f;
        exp.localScale = new Vector3(1, 1, 1) + new Vector3(expScale, expScale, expScale);
    }

    public void EnemyDamaged(float damage, int hitType) // hitType == 1 ����ź, 2 ����
    {

        int damageValue = 0;
        switch (hitType) // ���� ���� �������� �ٸ��� ��� 
        {
            case 1:
                if (GameManager.instance.attribute == ItemAttribute.Holy)
                {
                    int random = Random.Range(1, 101);

                    if (GameManager.instance.statManager.instantKillPer >= random) // ���
                    {
                        Transform instantMotion = GameManager.instance.magicManager.Get(6).transform;
                        instantMotion.position = transform.position;

                        damageValue = 999;
                        break;
                    }
                }
                // ��ȭ�� ��ŭ ������ ++
                float bulletDamageUpPer = GameManager.instance.enforce.enforceInfo[(int)EnforceName.BulletDamageUp].curLevel * GameManager.instance.enforce.enforceInfo[(int)EnforceName.BulletDamageUp].statIncrease;
                damage += damage * bulletDamageUpPer;
                break;
            case 2:
                // ��ȭ�� ��ŭ ������ ++
                float magicDamageUpPer = GameManager.instance.enforce.enforceInfo[(int)EnforceName.MagicDamageUp].curLevel * GameManager.instance.enforce.enforceInfo[(int)EnforceName.MagicDamageUp].statIncrease;
                damage += damage * magicDamageUpPer;

                if (isWetting)
                {
                    damage *= GameManager.instance.statManager.wettingDamagePer;
                }
                break;
        }


        if (damageValue == 0) // ��簡 �ƴ϶��
        {
            damageValue = (int)Mathf.Floor(damage);
        }

        health -= damageValue;

        Vector3 textPos = transform.position + new Vector3(0, 0.5f, 0);

        GameManager.instance.damageTextPool.Get((int)TextType.Damage,damageValue, textPos);

        EnemyHit();
    }
    private void EnemyHit()
    {

        if (health > 0)
        {
            AudioManager.instance.PlayerSfx(Sfx.EnemyHit);
            // Live, Hit Action
            anim.SetTrigger("Hit");
            StartCoroutine(KnockBack());

        }
        else
        {
            if (isLive) // �ߺ� ų ���� �ذ�
            {
                Death();
            }
        }
    }
    private void Death()
    {
        AudioManager.instance.PlayerSfx(Sfx.Dead);

        StopAllCoroutines();

        // �׾��� �� �ڷ�ƾ�� ���ư��°� ������
        if (enemyType == 3)
        {
            rush.isReady = false;
            rush.isAttack = false;
            rush.isRushing = false;
            rush.StopAllCoroutines();
        }
        else if (enemyType == 4)
        {
            rangeAttackEnemy.isReady = true;
            rangeAttackEnemy.StopAllCoroutines();
        }

        if (anim.speed != 1) // ����� ���¿��� �״´ٸ�
        {
            anim.speed = 1f;
        }
        isRestraint = false;
        spriteRenderer.color = new Color(1, 1, 1, 1); // ���� �� ������
        isLive = false; // �׾��� üũ
        col.enabled = false; // �ݶ��̴� ��Ȱ��ȭ
        rigid.simulated = false; // rigidbody2D ����
        spriteRenderer.sortingOrder = 1; // ���� ENemy�� �ٸ� Enemy�� ������ �ʵ��� OrderLayer�� 1�� ����
        anim.SetBool("Dead", true);
        GameManager.instance.kill++;

        HealthUpDrop();
        GameManager.instance.gold += enemyType + 1;
        if (statusEffect == EnemyStatusEffect.Darkness)
        {
            ExplosionSpawn();
        }
    }

    private void HealthUpDrop()
    {
        int random = Random.Range(1, 101);


        if(random <= ChestManager.instance.healthUpDropPer)
        {
            random = ChestManager.instance.Percent(ChestManager.instance.healthUpQualityPer);

            Transform heart = GameManager.instance.pool.Get((int)PoolList.Heart).transform;
            heart.position = transform.position;
            heart.GetComponent<Heart>().Init(random);
        }
    }
    private void Dead()
    {
        gameObject.SetActive(false);
    }


}
