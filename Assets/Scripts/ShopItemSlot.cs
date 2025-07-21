using System;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ShopItemSlot : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public Image itemIcon;
    public Button buyButton;

    private ItemSO currentItem;
    private ShopManager shopManager;

    public void SetItem(ItemSO item)
    {
        currentItem = item;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
        itemIcon.sprite = item.sprite;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => shopManager.BuyItem(currentItem));
    }

    public void ClearSlot()
    {
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemIcon.sprite = null;
        buyButton.onClick.RemoveAllListeners();
    }

    public void SetShopManager(ShopManager manager)
    {
        shopManager = manager;
    }
}

