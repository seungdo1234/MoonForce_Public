using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PanelType { Inventory, Enchant, Shop , Reward}
public class PanelClickEvent : MonoBehaviour , IPointerClickHandler
{
    public PanelType panelType;
    public ItemPreview preview;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (preview.gameObject.activeSelf && eventData.button == PointerEventData.InputButton.Left )
        {
            preview.gameObject.SetActive(false);

            if(panelType == PanelType.Inventory)
            {
                if (GameManager.instance.inventory.equipWaitItemSlot != null)
                {
                    GameManager.instance.inventory.EquipReadyCancel();
                }
            }
            else if(panelType == PanelType.Enchant)
            {
                GameManager.instance.enchant.EnchantReadyCancel();
            }
            else
            {
                GameManager.instance.shop.ShopReadyCancel();
            }

        }
    }
}
