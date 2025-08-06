
using Unity.VisualScripting;
using UnityEngine;
using System;

public class UseInputHandler : MonoBehaviour
{
    public Transform cameraTransform;
    public LayerMask layerMask;
    public User user;
    public InventoryManager inventoryManager;
    public CameraTilt cameraTilt;
    float pitch = 0f; 
    float yaw = 0f;
    float mouseSensitivity = 3.5f;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Moving Character
        Vector3 movement = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            movement += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement += Vector3.back;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement += Vector3.right;
        }
        if (Input.GetKeyDown(KeyCode.Space) && user.cc.isGrounded)
        {
            user.currentGravity.y = 5f; 
            user.playerStats.SpendStamina(10f); 
        }
        user.isSprinting = Input.GetKey(KeyCode.LeftShift);
        cameraTilt.Tilt(movement.x);
        Vector3 moveForward = cameraTransform.forward * movement.z;
        Vector3 moveSide = cameraTransform.right * movement.x;
        Vector3 cameraAdjustedMovement = moveForward + moveSide;
        cameraAdjustedMovement.y = 0;
        user.Move(cameraAdjustedMovement);
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity ;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity ;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -85f, 85f);
        user.transform.rotation *= Quaternion.Euler(0f, mouseX, 0f); // smooth rotation
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f); // camera tilt

        
        //Attack
        if (Input.GetMouseButtonDown(0))
        {
            user.Attack(cameraTransform.forward);
        }

        //Pick up item
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit raycastHit, 3f, layerMask))
            {
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 3f, Color.red, 4f);
                IInteractable interactable = raycastHit.transform.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }

            }
        }
        //Drop item
        if (Input.GetKeyDown(KeyCode.Q))
        {
            user.UnequipWeapon();
        }

        
    }
}
