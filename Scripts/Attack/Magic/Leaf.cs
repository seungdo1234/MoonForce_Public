using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    public Vector3 leafPos;
    public float coolTime;

    private ParticleSystem leaf;
    private int slotNum;
    private void Awake()
    {
        leaf = GetComponent<ParticleSystem>();
    }
    public void Init(float coolTime , int slotNum)
    {
        this.coolTime = coolTime;
        this.slotNum = slotNum;

        StartCoroutine(LeafStart());
    }
    private IEnumerator LeafStart()
    {

        while (!GameManager.instance.gameStop)
        {
            yield return new WaitForSeconds(coolTime);

            leaf.Play();
            StartCoroutine(Demeter());
            GameManager.instance.magicManager.coolTimeUI.CoolTimeStart(slotNum);
        }
    }

    private IEnumerator Demeter()
    {

        yield return new WaitForSeconds(0.5f);

        GameManager.instance.demeterOn = true;

        yield return new WaitForSeconds(5f);

        GameManager.instance.demeterOn = false;


    }


    private void Update()
    {
        transform.position = GameManager.instance.player.transform.position + leafPos;
    }
}
