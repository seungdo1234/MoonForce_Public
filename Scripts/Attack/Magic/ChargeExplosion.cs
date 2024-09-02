using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeExplosion : MonoBehaviour
{
    public float lerpTime;
    public Vector3 magicScale;

    private Animator anim;
    private CircleCollider2D col;
    public int slotNum;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();
    }

    public void Init(float coolTime , int magicSizeStep , int slotNum) // 충전 폭발 초기화
    {
        lerpTime = coolTime;
        this.slotNum = slotNum;
        transform.localScale = Vector3.one;

        if (magicSizeStep != 0)
        {
            magicScale = new Vector3(magicScale.x + (0.25f * magicSizeStep), magicScale.y + (0.25f * magicSizeStep), magicScale.z + (0.25f * magicSizeStep));
        }
        StartCoroutine(SkillStart());
    }
    public void ScaleReset()
    {
        transform.localScale = Vector3.one;
    }
    private IEnumerator SkillStart()
    {
        float curTime = 0;
        while (true)
        {
            curTime += Time.deltaTime;

            if (curTime > lerpTime)
            {
                curTime = 0;
                anim.SetTrigger("Explosion");
                transform.localScale = magicScale;
                GameManager.instance.magicManager.coolTimeUI.CoolTimeStart(slotNum);
            }

            yield return null;
        }
    }

    private void Update()
    {
        transform.position = GameManager.instance.player.transform.position;
    }
}
