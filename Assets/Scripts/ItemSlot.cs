using System;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public String itemName, itemDescription;
    public int itemQuantity;
    public Sprite itemSprite,emptySprite;
    public bool isFull,selectedItem;
    public Image itemImage;
    public GameObject selectedShader;
    public InventoryManager inventoryManager;
    public ItemSO itemSO;
    public TMP_Text ItemDescriptionText,ItemDescriptionName,quantityText;
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
        this.itemSprite = itemSO.sprite;
        this.itemName = itemSO.itemName;
        this.itemDescription = itemSO.itemDescription;
        isFull = true;
        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite =  itemSprite;
        itemImage.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRightClick();
        }
    }

    public void OnLeftClick()
    {
        if (selectedItem)
        {
            inventoryManager.UseIem(itemName);
            this.itemQuantity -= 1;
            quantityText.text = this.itemQuantity.ToString();
            if (this.itemQuantity <= 0)
            {
                EmptySlot();
            }
        }
        else
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            selectedItem = true;
            ItemDescriptionText.text = itemDescription;
            ItemDescriptionName.text = itemName; 
        }
    }

    public void onRightClick()
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
    }
    public void EmptySlot()
    {
        selectedItem = false;
        quantityText.enabled = false;
        itemImage.sprite = emptySprite;
        ItemDescriptionText.text = "";
        ItemDescriptionName.text = "";
    }
}
