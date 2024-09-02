using UnityEngine;
using UnityEngine.UI;

public class NextStageBtn : MonoBehaviour // �������������� ���� ��ư (���⸦ �������� �ʾҰų�, �κ��丮�� �� á�ٸ� Ȱ��ȭ X)
{
    public bool isActive;
    private Button btn;
    private Inventory inventory;
    public Text[] btnTexts;
    private void Awake()
    {
        btn = GetComponent<Button>();
        inventory = GameManager.instance.inventory;
    }

    public void LevelText(int level)
    {
        btnTexts[0].text = string.Format("{0} �������� ����", level + 1);
    }
    private void Update()
    {
        if (!GameManager.instance.gameStop) // �������� �÷��� ���� ���� Return
        {
            return;
        }

        bool equipStaff = false;
        int waitItemNum = 0;
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            if (!ItemDatabase.instance.Set(i).isEquip) // �������� ���� �������� � �ִ���
            {
                waitItemNum++;
            }
            else if (ItemDatabase.instance.Set(i).isEquip && ItemDatabase.instance.Set(i).type == ItemType.Staff) // �������� �������� �ִ���
            {
                equipStaff = true; // �ִٸ� True
            }
        }

        if (waitItemNum >= GameManager.instance.inventory.waitEqquipments.Length || !equipStaff) // �κ��丮�� �� á�ų�, �������� �������� ���ٸ� �������� ��ư ��Ȱ��ȭ
        {
            isActive = false;
            btn.interactable = false;
            btnTexts[0].gameObject.SetActive(false);
            btnTexts[1].gameObject.SetActive(true);
        }
        else // �ݴ��� Ȱ��ȭ
        {
            isActive = true;
            btn.interactable = true;
            btnTexts[0].gameObject.SetActive(true);
            btnTexts[1].gameObject.SetActive(false);
        }
    }
}
