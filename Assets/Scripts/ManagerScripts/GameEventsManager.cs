using UnityEngine;
using System;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance { get; private set; }
    // EVENTS
    public event Action<float, int> OnEnemyKilled;
    public event Action<float> OnPlayerDamaged;
    public event Action<float> OnPlayerHealed;
    public event Action OnPlayerLevelUp;
    public event Action<ItemSO, int> OnItemPickedUp;
    public event Action<ItemSO, int> OnItemDropped;
    public event Action<ItemSO> OnItemUsed;
    public event Action<ItemSO> OnItemPurchased;
    public event Action<int> OnBottleCapsGained;
    public event Action<float> OnExperienceGained;
    public QuestEvents questEvents;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        questEvents = new QuestEvents();
    }
    public void TriggerEnemyKilled(float experience, int bottlecaps)
    {
        Debug.Log($"[GameEventsManager] Triggering enemy killed: XP={experience}, BottleCaps={bottlecaps}");
        OnEnemyKilled?.Invoke(experience, bottlecaps);
    }
    public void TriggerPlayerLevelUp()
    {
        OnPlayerLevelUp?.Invoke();
    }
    public void TriggerPlayerDamaged(float damage)
    {
        Debug.Log($"[GameEventsManager] Triggering player damaged: {damage}");
        OnPlayerDamaged?.Invoke(damage);
    }

    public void TriggerPlayerHealed(float amount)
    {
        OnPlayerHealed?.Invoke(amount);
    }
    public void TriggerItemPickedUp(ItemSO item, int amount)
    {
        OnItemPickedUp?.Invoke(item, amount);
    }
    public void TriggerItemDropped(ItemSO item, int amount)
    {
        OnItemDropped?.Invoke(item, amount);
    }
    public void TriggerItemUsed(ItemSO item)
    {
        OnItemUsed?.Invoke(item);
    }
    public void TriggerItemPurchased(ItemSO item)
    {
        OnItemPurchased?.Invoke(item);
    }
    public void TriggerBottleCapsGained(int amount)
    {
        OnBottleCapsGained?.Invoke(amount);
    }
    public void TriggerExperienceGained(float amount)
    {
        OnExperienceGained?.Invoke(amount);
    }
}
