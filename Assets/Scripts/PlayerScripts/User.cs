
using Unity.VisualScripting;
using UnityEngine;
//this is a script for the user
public class User : MonoBehaviour
{
    float speed = 12f;
    float sprintCost = 18;
    public CharacterController cc;
    public float gravity = -9.81f;
    public Vector3 currentGravity = Vector3.zero;
    public bool isSprinting = false;
    public bool isAttacking = false;
    public PlayerStats playerStats;
    public Weapon currentWeapon;
    public Transform weaponHoldPoint;
    public AnimationStateChanger animationStateChanger;
    public InventoryManager inventoryManager;
    void Start()
    {
        cc = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
    }
    void Update()
    {
        SimulateGravity();
    }

    public void Move(Vector3 direction)
    {
        if (isAttacking) return;
        if (direction == Vector3.zero)
        {
            animationStateChanger.ChangeState("Breathing Idle", 1f);
            return;
        }
        if (isSprinting && playerStats.currentStamina > 0)
        {
            cc.Move(direction * (speed * 1.4f) * Time.deltaTime);
            playerStats.SpendStamina(sprintCost * Time.deltaTime);
            animationStateChanger.ChangeState("Running", 1f);
        }
        else
        {
            animationStateChanger.ChangeState("Walking", 1f);
            cc.Move(direction * speed * Time.deltaTime);
        }
    }

    public void SimulateGravity()
    {
        currentGravity.y += gravity * Time.deltaTime;
        cc.Move(currentGravity * Time.deltaTime);

        if (cc.isGrounded)
        {
            currentGravity = new Vector3(0, -1, 0);
        }
    }
    public void Attack(Vector3 direction, Transform cameraTransform)
    {
        if (playerStats.currentStamina <= 0 || isAttacking || playerStats.currentStamina < playerStats.currentDamage)
        {
            return;
        }
        isAttacking = true;
        playerStats.SpendStamina(playerStats.currentDamage);
        Vector3 origin = cameraTransform.position;

        Debug.DrawRay(origin, direction * 3f, Color.red, 4f);
        animationStateChanger.ChangeState("Stable Sword Outward Slash", 1f);
        playerStats.PlaySFX(playerStats.attackSound);
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 3f))
        {

            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(playerStats.currentDamage);
            }
        }

        if (isSprinting && playerStats.currentStamina > 0)
        {
            playerStats.SpendStamina(sprintCost * Time.deltaTime);
        }
         Invoke(nameof(EndAttack), 1.0f);

    }
    public void EndAttack()
    {
        isAttacking = false;
    }

     public void EquipWeapon(WeaponSO weaponSO)
    {
        if (currentWeapon != null)
        {
            Debug.Log("Unequipping current weapon: " + currentWeapon.weaponSO.name);
            UnequipWeapon();
        }

        Debug.Log("Equipping weapon: " + weaponSO.name);
        if (weaponSO == null)
        {
            Debug.LogError("WeaponSO is null. Cannot equip weapon.");
            return;
        }
        if (weaponSO != null)
        {
            Debug.Log("WeaponSO is not null. Proceeding to equip weapon.");
            GameObject weaponInstance = Instantiate(weaponSO.worldPrefab);
            Weapon weapon = weaponInstance.GetComponent<Weapon>();
            weapon.weaponSO = weaponSO;
            currentWeapon = weapon;
            Transform gripPoint = currentWeapon.transform.Find("GripPoint");
        
            currentWeapon.transform.SetParent(weaponHoldPoint);

             currentWeapon.transform.localPosition = -gripPoint.localPosition;
            currentWeapon.transform.localRotation = Quaternion.Inverse(gripPoint.localRotation);

            if (currentWeapon.TryGetComponent<Collider>(out var col))
            {
                col.enabled = false;
            }
            if (currentWeapon.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = true;
            }
            playerStats.currentDamage = weaponSO.damage;
        }
    }

    public void UnequipWeapon()
    {
        if (currentWeapon == null) return;

        currentWeapon.transform.SetParent(null);
        if (currentWeapon.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
        }
        if (currentWeapon.TryGetComponent<Collider>(out var col))
        {
            col.enabled = true;
        }
        currentWeapon.transform.position = transform.position + transform.forward;

        playerStats.currentDamage = playerStats.damage;
        currentWeapon = null;
    }
}
