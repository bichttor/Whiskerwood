using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string npcName;
    
   public void Interact()
    {
        // Implement interaction logic here, such as opening a dialogue or quest menu
        Debug.Log($"Interacting with NPC: {npcName}");
    }
}
