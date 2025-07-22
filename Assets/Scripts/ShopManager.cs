using UnityEngine;
using System;
public class ShopManager : MonoBehaviour
{
    public ShopItemSlot[] itemSlots;
    public InventoryManager inventory;
    public bool menuOn = false;
    public GameObject shopMenu;
    public User user;
      void Update()
    {
        if (menuOn && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }
    public void OpenShop(ItemSO[] itemsForSale)
    {
        
        shopMenu.SetActive(true);
        menuOn = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;     
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < itemsForSale.Length)
            {
                itemSlots[i].SetItem(itemsForSale[i]);
            }
            else
            {
                itemSlots[i].ClearSlot();
            }
        }
    }
      public void CloseShop()
    {
        shopMenu.SetActive(false);
        menuOn = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void BuyItem(ItemSO item)
    {
        if (user.playerStats.BottleCaps >= item.bottlecapsPrice)
        {
            inventory.AddItem(item, 1); // Add to player inventory
            user.playerStats.SpendBottleCaps(item.bottlecapsPrice);
            //remove the item maybe perhaps
        }
        else
        {
            Debug.Log("not enough bottlecaps to buy");
        }
    }
}
