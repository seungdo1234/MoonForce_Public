using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    public Text[] statusLevels;
    public Text availablePointText;

    private Button[] button;

    private void OnEnable()
    {
        StatusWindowActive();
    }
    private void Awake()
    {
        button = GetComponentsInChildren<Button>();
    }
    public void StatUp(int statNumber)
    {
        AudioManager.instance.PlayerSfx(Sfx.StatUp);

        GameManager.instance.statManager.statLevels[statNumber]++;

        GameManager.instance.statManager.StatValueUp(statNumber); 

        GameManager.instance.availablePoint--;

        StatusWindowActive();
    }

    private void StatusWindowActive()
    {

        
        availablePointText.text = string.Format("��� ���� ����Ʈ : {0}", GameManager.instance.availablePoint);

        
        for (int i = 0; i < button.Length - 1; i++)
        {
            // ���� ����
            statusLevels[i].text = string.Format("Lv.{0} / {1}", GameManager.instance.statManager.statLevels[i]  ,GameManager.instance.statManager.statMaxLevels[i]  );
            // ��ư Ȱ��ȭ/��Ȱ��ȭ
            button[i].interactable = GameManager.instance.availablePoint != 0 && GameManager.instance.statManager.statMaxLevels[i]  > GameManager.instance.statManager.statLevels[i] ? true : false;
        }

    }
}
