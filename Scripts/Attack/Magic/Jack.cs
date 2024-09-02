using System.Collections;
using UnityEngine;

public class Jack : MonoBehaviour
{
    public GameObject jack;
    public GameObject mainBackgroud;

    public float coolTime;
    public float lerpTime;
    public int slotNum;
    private Player player;
    private SpriteRenderer sprite;
    private void Awake()
    {
        player = GameManager.instance.player;
        sprite = GetComponent<SpriteRenderer>();
        mainBackgroud = GameManager.instance.background;
    }
    public void Init(float coolTime , int slotNum)
    {
        jack.SetActive(false);
        this.coolTime = coolTime;
        this.slotNum = slotNum;

        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);


        StartCoroutine(JackStart());
    }
    private IEnumerator JackStart()
    {

        while (!GameManager.instance.gameStop)
        {
            yield return new WaitForSeconds(coolTime);

            StartCoroutine(BG_Alpha());
            jack.SetActive(true);
            GameManager.instance.magicManager.coolTimeUI.CoolTimeStart(slotNum);
        }
    }

    private IEnumerator BG_Alpha()
    {
        mainBackgroud.SetActive(false);
        float currentTime = 0f;
        GameManager.instance.demeterOn = true;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.16f, 0.55f, currentTime / lerpTime);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            yield return null;
        }

        currentTime = 0f;
        GameManager.instance.demeterOn = false;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.55f, 0.16f, currentTime / lerpTime);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            yield return null;
        }

        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
        mainBackgroud.SetActive(true);
    }
    private void Update()
    {
        transform.position = GameManager.instance.player.transform.position;
    }
}
