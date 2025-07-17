using System;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public String itemName,itemDescription;
    public int itemQuantity;
    public Sprite itemSprite;
    public bool isFull;
    public TMP_Text quantityText;
    public Image itemImage;
    public GameObject selectedShader;
    public bool selectedItem;
    public InventoryManager inventoryManager;

    public TMP_Text ItemDescriptionText;
    public TMP_Text ItemDescriptionName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }
    public void addItem(string name, int quantity, Sprite sprite, String description)
    {
        this.itemName = name;
        this.itemQuantity = quantity;
        this.itemSprite = sprite;
        this.itemDescription = description;
        isFull = true;
        quantityText.text = quantity.ToString();
        quantityText.enabled = itemSprite;
        itemImage.sprite = sprite;
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

        }
    }

    public void OnLeftClick()
    {
        if (selectedItem)
        {
            inventoryManager.UseIem(itemName);
        }
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        selectedItem = true;
        ItemDescriptionText.text = itemDescription;
        ItemDescriptionName.text = itemName;
    }
}
