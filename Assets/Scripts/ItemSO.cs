using UnityEngine;
[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName, itemDescription;
    public StatToChange statToChange;
    public int amountToChageStat, quantity, bottlecapsPrice;
    public GameObject worldPrefab;
    public Sprite sprite;

    public virtual void UseItem()
    {
        Debug.Log("Using item: " + itemName);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        PlayerStats stats = player.GetComponent<PlayerStats>();
        if (stats == null) return;

        switch (statToChange)
        {
            case StatToChange.health:
                stats.Heal((float)amountToChageStat);
                break;
            case StatToChange.stamina:
                stats.AddStamina((float)amountToChageStat);
                break;
            case StatToChange.xp:
                stats.AddExperience((float)amountToChageStat);
                break;
        }
    }
    public enum StatToChange
    {
        none,
        health,
        stamina,
        xp
    };
}
