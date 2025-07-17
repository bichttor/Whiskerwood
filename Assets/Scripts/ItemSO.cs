using UnityEngine;
[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange;
    public int amountToChageStat;
    public GameObject worldPrefab; 

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
