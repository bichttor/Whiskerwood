using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour, IInteractable  
{
    public InventoryManager inventoryManager;
    public WeaponSO weaponSO;
    public int quantity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }

    public void Interact()
    {
        Debug.Log("Interacting with weapon: " + weaponSO.itemName);
        inventoryManager.AddItem(weaponSO, quantity); 
        Destroy(gameObject);
    }
}
