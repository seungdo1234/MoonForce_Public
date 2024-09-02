using UnityEngine;
using UnityEngine.UI;

public class NextStageBtn : MonoBehaviour // 다음스테이지로 가는 버튼 (무기를 장착하지 않았거나, 인벤토리가 꽉 찼다면 활성화 X)
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
        btnTexts[0].text = string.Format("{0} 스테이지 시작", level + 1);
    }
    private void Update()
    {
        if (!GameManager.instance.gameStop) // 스테이지 플레이 중일 때는 Return
        {
            return;
        }

        bool equipStaff = false;
        int waitItemNum = 0;
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            if (!ItemDatabase.instance.Set(i).isEquip) // 장착하지 않은 아이템이 몇개 있는지
            {
                waitItemNum++;
            }
            else if (ItemDatabase.instance.Set(i).isEquip && ItemDatabase.instance.Set(i).type == ItemType.Staff) // 장착중인 스태프가 있는지
            {
                equipStaff = true; // 있다면 True
            }
        }

        if (waitItemNum >= GameManager.instance.inventory.waitEqquipments.Length || !equipStaff) // 인벤토리가 다 찼거나, 장착중인 스태프가 없다면 스테이지 버튼 비활성화
        {
            isActive = false;
            btn.interactable = false;
            btnTexts[0].gameObject.SetActive(false);
            btnTexts[1].gameObject.SetActive(true);
        }
        else // 반대라면 활성화
        {
            isActive = true;
            btn.interactable = true;
            btnTexts[0].gameObject.SetActive(true);
            btnTexts[1].gameObject.SetActive(false);
        }
    }
}
