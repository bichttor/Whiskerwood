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
    public void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    void OnEnable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.OnItemPickedUp += AddItem;
        }
    }
    void OnDisable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.OnItemPickedUp -= AddItem;
        }
    }
    public void AddItem(ItemSO itemSO, int quantity)
    {
        this.itemSO = itemSO;
        this.itemQuantity = quantity;
        quantityText.text = quantity.ToString();
        quantityText.gameObject.SetActive(true);
        this.itemSO.quantity = quantity;
    }

    public void EmptySlot()
    {
        quantityText.gameObject.SetActive(false);
        itemImage.sprite = emptySprite;
        ItemDescriptionText.text = "";
        ItemDescriptionName.text = "";
        itemSO = null;
        itemQuantity = 0;
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
            Debug.Log("No item to use or item quantity is zero.");
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
    
    public void OnDropItem()
    {
        if (itemSO == null || itemQuantity <= 0 || playerTransform == null)
        {
            return;
        }

        GameObject itemInstance = Instantiate(itemSO.worldPrefab, playerTransform.position, Quaternion.identity);
        var itemComponent = itemInstance.GetComponent<Item>();
        if (itemComponent != null)
        {
            itemComponent.itemSO = itemSO;
            itemComponent.quantity = itemQuantity;
        }
        GameEventsManager.Instance.TriggerItemDropped(itemSO, itemQuantity); 
        EmptySlot();
    }
}
