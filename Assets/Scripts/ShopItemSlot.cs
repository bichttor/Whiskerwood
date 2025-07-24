using System;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ShopItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public Image itemIcon;
    public ItemSO currentItem;
    public ShopManager shopManager;


    public void SetItem(ItemSO item)
    {
        currentItem = item;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
        itemIcon.sprite = item.sprite;
    }

    public void ClearSlot()
    {
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemIcon.sprite = null;
    }
    public void OnPointerEnter(PointerEventData eventData)
        {
            if (currentItem != null)
            {
                itemNameText.text = currentItem.itemName;
                itemDescriptionText.text = currentItem.itemDescription;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";
        }
}

