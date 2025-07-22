using System;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int itemQuantity;
    public Sprite itemSprite, emptySprite;
    public Image itemImage;
    public InventoryManager inventoryManager;
    public ItemSO itemSO;
    public TMP_Text ItemDescriptionText, ItemDescriptionName, quantityText;
    public Transform playerTransform;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    public void AddItem(ItemSO itemSO, int quantity)
    {
        this.itemSO = itemSO;
        this.itemQuantity = quantity;
        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        this.itemSO.quantity = quantity;
    }
    /*public void onRightClick()
    {
        Vector3 dropPosition = playerTransform.position + playerTransform.forward * 1.5f + Vector3.up * 0.5f;
        GameObject dropped = Instantiate(itemSO.worldPrefab, dropPosition, Quaternion.identity);
        Item newItem = dropped.GetComponent<Item>();
        newItem.itemSO = itemSO;
        newItem.quantity = 1;

        newItem.transform.SetParent(null);
        if (newItem.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
        }
        if (newItem.TryGetComponent<Collider>(out var col))
        {
            col.enabled = true;
        }
        newItem.transform.position = transform.position + transform.forward;
        this.itemQuantity -= 1;
        quantityText.text = this.itemQuantity.ToString();
        if (this.itemQuantity <= 0)
        {
            EmptySlot();
        }
    }*/
    public void EmptySlot()
    {
        quantityText.enabled = false;
        itemImage.sprite = emptySprite;
        ItemDescriptionText.text = "";
        ItemDescriptionName.text = "";
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSO != null)
        {
            inventoryManager.selectedSlot = this;
            ItemDescriptionName.text = itemSO.itemName;
            ItemDescriptionText.text = itemSO.itemDescription;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventoryManager.selectedSlot == this)
        {
            inventoryManager.selectedSlot = null;
        }
        ItemDescriptionName.text = "";
        ItemDescriptionText.text = "";
    }
    public void OnUseItem()
    {
        if (itemSO == null || itemQuantity <= 0)
        {
           return; 
        } 

        inventoryManager.UseItem(itemSO.itemName);
        itemQuantity--;
        quantityText.text = itemQuantity.ToString();

        if (itemQuantity <= 0)
        {
            EmptySlot();
        }
    }
}
