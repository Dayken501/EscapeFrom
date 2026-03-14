using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Движение")]
    public float maxSpeed = 6f;
    [Tooltip("Время разгона (чем меньше, тем быстрее)")]
    public float accelerationTime = 0.1f;
    [Tooltip("Время торможения")]
    public float decelerationTime = 0.1f;

    [Header("Лазер")]
    public LineRenderer laserRenderer;
    public float laserRange = 50f;
    public Transform weaponMuzzle; // Точка выхода лазера (опционально)

    private Rigidbody rb;
    private Animator animator;
    private Camera mainCamera;

    private Vector2 inputDirection;
    private Vector3 currentVelocity;
    private Vector3 smoothVelocityRef;
    private Vector3 lookTargetPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;

        // У Rigidbody лучше включить Interpolate для плавности камеры
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Считываем ввод (новому Input System лучше быть в Update)
        inputDirection = new Vector2(
            (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0),
            (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0)
        ).normalized;

        // 2. Логика поворота к мыши
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            lookTargetPoint = ray.GetPoint(distance);
            Vector3 lookDirection = lookTargetPoint - transform.position;
            lookDirection.y = 0;

            if (lookDirection.sqrMagnitude > 0.01f)
                transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        // 3. Обновление лазера
        if (laserRenderer != null)
            UpdateLaser();

        // Анимации
        animator.SetBool("isRunning", inputDirection.magnitude > 0);
    }

    void FixedUpdate()
    {
        // ДВИЖЕНИЕ В МИРОВЫХ КООРДИНАТАХ (не зависит от того, куда смотрит нос персонажа)
        // Это позволяет нормально стрейфить, как в видео
        Vector3 targetMoveVector = new Vector3(inputDirection.x, 0, inputDirection.y) * maxSpeed;

        float currentSmooth = inputDirection.magnitude > 0 ? accelerationTime : decelerationTime;

        currentVelocity = Vector3.SmoothDamp(currentVelocity, targetMoveVector, ref smoothVelocityRef, currentSmooth);

        // Применяем к Rigidbody (не забываем про гравитацию через rb.linearVelocity.y)
        rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
    }

    void UpdateLaser()
    {
        // Если точка выхода не задана, берем центр + смещение
        Vector3 laserStart = weaponMuzzle != null ? weaponMuzzle.position : transform.position + transform.forward * 0.5f + Vector3.up * 1f;

        Vector3 direction = (lookTargetPoint - laserStart).normalized;
        Vector3 laserEnd;

        // Проверка препятствий для лазера
        if (Physics.Raycast(laserStart, direction, out RaycastHit hit, laserRange))
        {
            laserEnd = hit.point;
        }
        else
        {
            laserEnd = laserStart + direction * laserRange;
        }

        laserRenderer.SetPosition(0, laserStart);
        laserRenderer.SetPosition(1, laserEnd);
    }
}