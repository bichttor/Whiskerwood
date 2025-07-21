using UnityEngine;
[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName, itemDescription;
    public StatToChange statToChange;
    public int amountToChageStat, quantity;
    public GameObject worldPrefab;
    public Sprite sprite;

    public void UseItem()
    {
        if (statToChange == StatToChange.health)
        {
            //update health
        }
        if (statToChange == StatToChange.stamina)
        {
            //update stamina
        }
        if (statToChange == StatToChange.xp)
        {
            //update xp
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
