using UnityEngine;

public class CollectSpecialItemQuestStep : QuestStep
{
    public int itemToCollect = 5;
    public int itemCollected = 0;

    public void OnEnable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.OnItemPickedUp += OnItemPickedUp;
        }
    }
    public void OnDisable()
    {
        if (GameEventsManager.Instance != null)
            GameEventsManager.Instance.OnItemPickedUp -= OnItemPickedUp;
    }
    public void OnItemPickedUp(ItemSO item, int amount)
    {
        Debug.Log($"Item picked up: {item.itemName}, Amount: {amount}");
        if (item.itemName == "Catnip")
        {
            itemCollected += amount;
            if (itemCollected >= itemToCollect)
            {
                FinishQuestStep();
            }
        }
    }
}
