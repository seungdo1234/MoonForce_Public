using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    // enum : ������
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
                // level�� int ���̹Ƿ� str���� ��ȯ -> string.Format�� Ȱ���Ͽ� int���� str ������ ��ȯ
                // Format ("���ڿ� + { ���� : ��Ÿ���� ����} ",����ȯ�� �� ������)
                // F0, F1, F2... => �Ҽ��� �ڸ���
                // D0, D1, D2... => ���� �ڸ���
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level + 1);
                break;

            case InfoType.Kill:
                int curKill = GameManager.instance.kill;
                int maxKill = GameManager.instance.enemyMaxNum;

                myText.text = string.Format("{0}", maxKill - curKill);
                break;

            case InfoType.Time:
                float remainTime = GameManager.instance.curGameTime;
                int min = Mathf.FloorToInt(remainTime / 60); // int�� �Ҽ��� ����
                int sec = Mathf.FloorToInt(remainTime % 60); // int�� �Ҽ��� ����
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
