using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLoadExit : MonoBehaviour
{
    public GameObject inventoryStructure;
    public GameObject inventoryGameObject;
    public GameObject exitBtn;
    public ItemPreview preview;

    public void Load()
    {
        inventoryStructure.SetActive(true);
        exitBtn.SetActive(true);
    }

    public void Exit()
    {
        inventoryStructure.SetActive(false);
        exitBtn.SetActive(false);
        inventoryGameObject.SetActive(false);
        preview.gameObject.SetActive(false);
        GameManager.instance.inventory.EquipReadyCancel();
    }
    public void StatWindowOn()
    {
        inventoryStructure.SetActive(false);
        preview.gameObject.SetActive(false);
        GameManager.instance.inventory.EquipReadyCancel();
        AudioManager.instance.SelectSfx();
    }
}
