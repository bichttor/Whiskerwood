using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;

    public bool menuOn;
    public ItemSlot[] itemSlot;
    public ItemSO[] itemSOs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
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

    }

    public void UseIem(string name)
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == name)
            {
                itemSOs[i].UseItem();
            }
        }
    }
    public void AddItem(string name, int quantity, Sprite sprite, String description)
    {
        Debug.Log("starting loop");
        for (int i = 0; i < itemSlot.Length; i++)
        {
            Debug.Log("loop" + i);
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].addItem(name, quantity, sprite, description);
                return;
            }
            else
            {
                Debug.Log("is full" + i);
            }
        }
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
        }
    }
}
