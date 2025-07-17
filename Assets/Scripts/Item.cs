using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName, itemDiscription;
    public int quantity;
    public Sprite sprite;
    public InventoryManager inventoryManager;
    public string itemDescription;
    public ItemSO itemSO;
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }
    
}
