
using Unity.VisualScripting;
using UnityEngine;
using System;
public class UseInputHandler : MonoBehaviour
{
    public Transform cameraTransform;

    public LayerMask layerMask;
    public User user;
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
        yaw += mouseX;
        user.transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f); // camera tilt

        
        //Attack
        if (Input.GetMouseButtonDown(0))
        {
            user.Attack(cameraTransform.forward, cameraTransform);
        }

        //Pick up item
       if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            Debug.Log("Ray Origin: " + ray.origin + ", Direction: " + ray.direction);
            Debug.DrawRay(ray.origin, ray.direction * 6f, Color.green, 2f);
            if (Physics.Raycast(ray, out RaycastHit hit, 5f, layerMask))
            {
                Debug.Log("Hit " + hit.collider.name);
                IInteractable interactable = hit.transform.GetComponent<IInteractable>();
                interactable?.Interact();
            }
        }

        //Drop item
        if (Input.GetKeyDown(KeyCode.Q))
        {
            user.UnequipWeapon();
        }

        
    }
}
