using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
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
        // Плавный поворот персонажа к курсору
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
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        // Движение только вперёд по W
        Vector2 input = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) input.y += 1;

        Vector3 moveDirection = (transform.forward * input.y).normalized;
        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);

        animator.SetBool("isRunning", moveDirection != Vector3.zero);
    }
}