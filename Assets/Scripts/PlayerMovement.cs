using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
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
        // Поворот к курсору
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 lookPoint = ray.GetPoint(distance);
            Vector3 lookDirection = lookPoint - transform.position;
            lookDirection.y = 0;
            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }

        // Движение вперёд и назад
        Vector3 moveDirection = Vector3.zero;
        bool isBackward = false;

        if (Keyboard.current.wKey.isPressed)
            moveDirection = transform.forward;

        if (Keyboard.current.sKey.isPressed)
        {
            moveDirection = -transform.forward;
            isBackward = true;
        }

        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);

        animator.SetBool("isRunning", moveDirection != Vector3.zero && !isBackward);
        animator.SetBool("isWalkingBack", isBackward);
    }
}