using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("Цель")]
    public Transform target;

    [Header("Настройки")]
    public float distance = 6f;
    public float height = 3f;
    public float rotationSpeed = 3f;
    public float smoothSpeed = 10f;

    private float currentYaw = 0f;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // Вращение камеры мышкой
        float mouseX = Mouse.current.delta.ReadValue().x;
        currentYaw += mouseX * rotationSpeed * Time.deltaTime * 10f;

        // Позиция камеры сзади персонажа
        Quaternion rotation = Quaternion.Euler(20f, currentYaw, 0f);
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance + Vector3.up * height;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 1f / smoothSpeed);
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

    public float GetYaw() { return currentYaw; }
}