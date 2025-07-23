using UnityEngine;
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class WeaponSO : ItemSO
{
    public int damage;
    public float attackSpeed;

    public float range;

    public override void UseItem()
    {
        Debug.Log("Using weapon: " + itemName);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        User user = player.GetComponent<User>();
        if (user == null) return;
        user.EquipWeapon(this);
    }
}
