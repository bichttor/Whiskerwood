using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public InventoryManager inventoryManager;
    public ItemSO itemSO;
    public int quantity;
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }
     public void Interact()
    {
        inventoryManager.AddItem(itemSO, quantity); 
        Destroy(gameObject);
    }
}
