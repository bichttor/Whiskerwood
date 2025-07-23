using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    public ItemSlot selectedSlot;
    public bool menuOn;
    public ItemSlot[] itemSlot;
    public ItemSO[] itemSOs;
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (menuOn)
            {
                InventoryMenu.SetActive(false);
                menuOn = false;
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                InventoryMenu.SetActive(true);
                menuOn = true;
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        if (menuOn && Input.GetKeyDown(KeyCode.Q) && selectedSlot != null && selectedSlot.itemSO != null)
        {
        //selectedSlot.DropItem();
        }
    }

    public void UseItem(string name)
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            Debug.Log("Checking item: " + itemSOs[i].itemName);
            if (itemSOs[i].itemName == name)
            {
                Debug.Log("Using item for IM: " + name);
                itemSOs[i].UseItem();
            }
        }
        
    }
    public void AddItem(ItemSO itemSO, int quantity)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].itemSO == null)
            {
                Debug.Log("Adding item: " + itemSO.itemName + " with quantity: " + quantity);
                itemSlot[i].AddItem(itemSO, quantity);
                return;
            }
        }
    }       
}
