using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    // enum : 열거형
    public enum InfoType { Goal, Level, Kill, Time, Health, Gold }

    public InfoType type;


    private Text myText;
    private Slider mySlider;

    private void Awake()
    {
        mySlider = GetComponent<Slider>();
        if(type != InfoType.Health)
        {
            myText = GetComponent<Text>();
        }
        else
        {
            myText = GetComponentInChildren<Text>();
        }
     

    }


    private void Update()
    {
        if (GameManager.instance.gameStop)
        {
            return;
        }

    }
    private void LateUpdate()
    {
        if (GameManager.instance.gameStop)
        {
            return;
        }
        switch (type)
        {

            case InfoType.Level:
                // level은 int 형이므로 str으로 변환 -> string.Format을 활용하여 int형을 str 형으로 변환
                // Format ("문자열 + { 순번 : 나타내는 형태} ",형변환을 할 데이터)
                // F0, F1, F2... => 소수점 자릿수
                // D0, D1, D2... => 고정 자릿수
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level + 1);
                break;

            case InfoType.Kill:
                int curKill = GameManager.instance.kill;
                int maxKill = GameManager.instance.enemyMaxNum;

                myText.text = string.Format("{0}", maxKill - curKill);
                break;

            case InfoType.Time:
                float remainTime = GameManager.instance.curGameTime;
                int min = Mathf.FloorToInt(remainTime / 60); // int형 소수점 버림
                int sec = Mathf.FloorToInt(remainTime % 60); // int형 소수점 버림
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;

            case InfoType.Health:
                float maxHealth = GameManager.instance.statManager.maxHealth;
                float curHealth = GameManager.instance.statManager.curHealth;

                myText.text = string.Format("{0:F0}/{1:F0}", curHealth, maxHealth);
                mySlider.value = curHealth / maxHealth;
                break;
            case InfoType.Gold:

                myText.text = string.Format("{0}", GameManager.instance.gold);
                break;

        }
    }
}
