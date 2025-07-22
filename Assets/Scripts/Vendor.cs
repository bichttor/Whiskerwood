using UnityEngine;
using System.Collections.Generic;
public class Vendor : MonoBehaviour, IInteractable
{
    public string vendorName;
 
    public ItemSO[] itemsForSale;
    public ShopManager shopManager;

    public void Interact()
    {
        shopManager.OpenShop(itemsForSale);
    }
}
