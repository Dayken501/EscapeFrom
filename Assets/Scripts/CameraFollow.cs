using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("Цель")]
    public Transform target;

    [Header("Настройки")]
    public float distance = 10f;
    public float height = 8f;
    public float rotationSpeed = 0.3f;
    public float smoothTime = 0.15f;

    private float currentAngle = 0f;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // Вращение просто движением мыши
        float mouseX = Mouse.current.delta.ReadValue().x;
        currentAngle += mouseX * rotationSpeed;

        Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance + Vector3.up * height;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.LookAt(target.position + Vector3.up * 1f);
    }

    public float GetYaw() { return currentAngle; }
}