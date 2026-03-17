using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 15f;
    private Rigidbody rb;
    private Animator animator;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        // Движение относительно камеры
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = Vector3.zero;
        if (Keyboard.current.wKey.isPressed) moveDirection += camForward;
        if (Keyboard.current.sKey.isPressed) moveDirection -= camForward;
        if (Keyboard.current.aKey.isPressed) moveDirection -= camRight;
        if (Keyboard.current.dKey.isPressed) moveDirection += camRight;

        moveDirection = moveDirection.normalized;
        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);

        // Поворот персонажа в сторону движения
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