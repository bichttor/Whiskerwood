
using Unity.VisualScripting;
using UnityEngine;
//this is a script for the user
public class User : MonoBehaviour
{

    float speed = 12f;
    float sprintCost = 18;
    CharacterController cc;
    public float gravity = -9.81f;
    Vector3 currentGravity = Vector3.zero;
    public bool isSprinting = false;
    public PlayerStats playerStats;
    public GameObject currentWeapon;
    public Transform weaponHoldPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
        {
            return;
        }
        if (isSprinting && playerStats.currentStamina > 0)
        {
            cc.Move(direction * (speed * 1.4f) * Time.deltaTime);
            playerStats.SpendStamina(sprintCost * Time.deltaTime);
        }
        else
        {
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
    public void Attack(Vector3 direction)
    {
        if (playerStats.currentStamina <= 0)
        {
            return;
        }
        playerStats.SpendStamina(playerStats.currentDamage);
        Vector3 origin = transform.position;
        Debug.DrawRay(origin, direction * 3f, Color.red, 4f);


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

    }
    // Update is called once per frame
    void Update()
    {
        SimulateGravity();
    }
    
     public void EquipWeapon(GameObject weaponObject)
    {
        Weapon weapon = weaponObject.GetComponent<Weapon>();
        if (weapon != null)
        {
            currentWeapon = weaponObject;
            Transform gripPoint = weaponObject.transform.Find("GripPoint");
            weaponObject.transform.SetParent(weaponHoldPoint);
            weaponObject.transform.localPosition = -gripPoint.localPosition;
            weaponObject.transform.localRotation = Quaternion.Inverse(gripPoint.localRotation);

            if (weaponObject.TryGetComponent<Collider>(out var col))
            {
                col.enabled = false;
            }
            if (weaponObject.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = true;
            }
            

            playerStats.currentDamage = weapon.damage;
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
