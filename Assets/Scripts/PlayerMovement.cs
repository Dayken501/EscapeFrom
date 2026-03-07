using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;
    private Rigidbody rb;
    private Animator animator;
    private CameraFollow cameraFollow;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        float yaw = cameraFollow.GetYaw();
        Vector3 camForward = Quaternion.Euler(0, yaw, 0) * Vector3.forward;
        Vector3 camRight = Quaternion.Euler(0, yaw, 0) * Vector3.right;

        Vector3 moveDirection = Vector3.zero;
        if (Keyboard.current.wKey.isPressed) moveDirection += camForward;
        if (Keyboard.current.sKey.isPressed) moveDirection -= camForward;
        if (Keyboard.current.aKey.isPressed) moveDirection -= camRight;
        if (Keyboard.current.dKey.isPressed) moveDirection += camRight;

        moveDirection = moveDirection.normalized;
        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            animator.SetBool("isRunning", true);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            animator.SetBool("isRunning", false);
        }

        animator.SetBool("isWalkingBack", false);
    }
}